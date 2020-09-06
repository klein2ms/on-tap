using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OnTap.Blazor.Server.Data;
using OnTap.Blazor.Server.Models;
using OnTap.Blazor.Server.Services;
using OnTap.Blazor.Shared;
using Xunit;

namespace OnTap.Blazor.Server.Tests
{
    public class LeagueServiceTests
    {
        [Fact]
        public async Task GetLeagueAsync_WhenLeagueDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetLeagueAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().Be(new LeagueViewModel());
        }

        [Fact]
        public async Task GetLeagueAsync_WhenLeagueDoesExist_ReturnsLeague()
        {
            var league = GetLeague();

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetLeagueAsync(league.Id);
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .Be(GetMapper().Map<LeagueViewModel>(league));
        }

        [Fact]
        public async Task AddOrUpdateLeagueAsync_WhenLeagueDoesNotExist_AddsLeague()
        {
            var vm = GetLeagueViewModel();

            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateLeagueAsync(vm);
                });

            var expected = GetLeagueViewModel();
            expected.Id = 1;
            expected.LeagueSettings.Id = 1;
            expected.LeagueSettings.LeagueId = 1;
            expected.LeagueSettings.LeagueName = expected.Name;
            expected.LeagueSettings.NumberOfTeams = 2;
            expected.Teams.First().Id = 1;
            expected.Teams.Last().Id = 2;

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task AddOrUpdateLeagueAsync_WhenLeagueDoesExist_UpdatesLeague()
        {
            const string name = "Some Other Test Name";
            const int id = 1;

            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    var(_, league) = await sut.GetLeagueAsync(id);
                    league.Name = name;
                    return await sut.AddOrUpdateLeagueAsync(league);
                });

            ok.Should().BeTrue();
            actual.Id.Should().Be(id);
            actual.Name.Should().Be(name);
        }

        [Fact]
        public async Task RemoveLeagueAsync_WhenLeagueDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.RemoveLeagueAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().Be(new LeagueViewModel());
        }

        [Fact]
        public async Task RemoveLeagueAsync_WhenLeagueDoesExist_ReturnsCorrectResult()
        {
            var league = GetLeague();
            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    var(_, league) = await sut.GetLeagueAsync(1);
                    return await sut.RemoveLeagueAsync(1);
                });

            league.LeagueSettings = null;
            league.Teams = new List<Team>();
            ok.Should().BeTrue();
            actual.Should().Be(GetMapper().Map<LeagueViewModel>(league));
        }

        [Fact]
        public async Task RemoveLeagueAsync_WhenLeagueDoesExist_RemovesLeague()
        {
            var(ok, _) = await Execute(
                async context =>
                {
                    var league = GetLeagueViewModel();
                    context.Leagues.Add(GetMapper().Map<League>(league));
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    _ = await sut.RemoveLeagueAsync(1);
                    return await sut.GetLeagueAsync(1);
                });

            ok.Should().BeFalse();
        }

        [Fact]
        public async Task GetTeamsAsync_WhenLeagueDoesNotExist_ReturnsEmptyCollection()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetTeamsAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTeamsAsync_WhenLeagueExists_ReturnsCollection()
        {
            var league = GetLeague();

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetTeamsAsync(league.Id);
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .BeEquivalentTo(league.Teams.Select(t => GetMapper().Map<TeamViewModel>(t)));
        }

        [Fact]
        public async Task AddOrUpdateTeamsAsync_WhenTeamsIsNull_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateTeamsAsync(1, null);
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>());
        }

        [Fact]
        public async Task AddOrUpdateTeamsAsync_WhenLeagueDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateTeamsAsync(0, new List<TeamViewModel>());
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>());
        }

        [Fact]
        public async Task AddOrUpdateTeamsAsync_WhenLeagueExistsAndHasNoTeams_AddsTheTeams()
        {
            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(new League { Name = "First League" });
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var teams = new List<TeamViewModel>
                    {
                    new TeamViewModel { Name = "First Team", Owner = new OwnerViewModel { Name = "First Owner" } },
                    new TeamViewModel { Name = "Second Team", Owner = new OwnerViewModel { Name = "Second Owner" } },
                        };
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateTeamsAsync(1, teams);
                });

            ok.Should().BeTrue();
            actual.First().Id.Should().Be(1);
            actual.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task AddOrUpdateTeamsAsync_WhenTeamIsNewAndLeagueHasExistingTeams_AddsTheNewTeam()
        {
            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(new League 
                    { 
                        Name = "First League",
                        Teams = new List<Team>
                        {
                            new Team { Name = "First Team", OwnerName = "First Owner" }
                        } 
                    });
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var teams = new List<TeamViewModel>
                    {
                    new TeamViewModel { Id = 1, Name = "First Team Changed", Owner = new OwnerViewModel { Name = "First Owner" } },
                    new TeamViewModel { Name = "Second Team", Owner = new OwnerViewModel { Name = "Second Owner" } },
                    };
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateTeamsAsync(1, teams);
                });

            ok.Should().BeTrue();
            actual.First().Id.Should().Be(1);
            actual.First().Name.Should().Be("First Team Changed");
            actual.Last().Id.Should().Be(2);
        }        
        
        [Fact]
        public async Task RemoveTeamsAsync_WhenTeamsIsNull_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.RemoveTeamsAsync(1, null);
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>());
        }

        [Fact]
        public async Task RemoveTeamsAsync_WhenLeagueDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var league = GetLeagueViewModel();
                    var sut = GetSut(context);;
                    return await sut.RemoveTeamsAsync(1, league.Teams.ToList());
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>());
        }

        [Fact]
        public async Task RemoveTeamsAsync_WhenLeagueExistsButTeamDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var nonExistentTeam = new Team { Name = "I Do Not Exist", OwnerName = "In This Database" };
                    var sut = GetSut(context);;
                    return await sut.RemoveTeamsAsync(1, new List<TeamViewModel>
                        {
                            GetMapper().Map<TeamViewModel>(nonExistentTeam)
                        });
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>());
        }

        [Fact]
        public async Task RemoveTeamsAsync_WhenLeagueExistsAndTeamExists_RemovesTeam()
        {
            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    var(_, league) = await sut.GetLeagueAsync(1);
                    var toRemove = league.Teams.FirstOrDefault();
                    return await sut.RemoveTeamsAsync(1, new List<TeamViewModel>
                        {
                            toRemove
                        });
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .BeEquivalentTo(new List<TeamViewModel>
                    {
                        new TeamViewModel
                        {
                            Id = 1,
                                Name = "Test Team One",
                                Owner = new OwnerViewModel { Name = "Test Owner One" }
                        }
                    });
        }

        [Fact]
        public async Task GetLeagueSettingsAsync_WhenLeagueDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetLeagueSettingsAsync(0);
                });

            ok.Should().BeFalse();
            actual
                .Should()
                .Be(new LeagueSettingsViewModel());
        }

        [Fact]
        public async Task GetLeagueSettingsAsync_WhenLeagueExists_ReturnsLeagueSettings()
        {
            var league = GetLeague();

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetLeagueSettingsAsync(league.Id);
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .Be(GetMapper().Map<LeagueSettingsViewModel>(league.LeagueSettings));
        }

        [Fact]
        public async Task AddOrUpdateLeagueSettingsAsync_WhenLeagueExistsAndLeagueSettingsDoNotExist_AddsLeagueSettings()
        {
            var leagueSettings = GetLeague().LeagueSettings;

            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    leagueSettings.Id = 1;
                    leagueSettings.LeagueId = 1;
                    leagueSettings.SeasonId = 2019;
                    leagueSettings.NumberOfStartingQuarterbacks = 1;
                    return await sut.AddOrUpdateLeagueSettingsAsync(GetMapper().Map<LeagueSettingsViewModel>(leagueSettings));
                });

            var expected = GetMapper().Map<LeagueSettingsViewModel>(leagueSettings);
            expected.LeagueName = "Test League";
            expected.NumberOfTeams = 2;

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task AddOrUpdateLeagueSettingsAsync_WhenLeagueExistsAndLeagueSettingsDoExist_UpdatesLeagueSettings()
        {
            var leagueSettings = GetLeague().LeagueSettings;

            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Leagues.Add(league);
                    league.LeagueSettings.NumberOfStartingQuarterbacks = 1;
                    context.LeagueSettings.Add(league.LeagueSettings);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    leagueSettings.Id = 1;
                    leagueSettings.LeagueId = 1;
                    leagueSettings.SeasonId = 2019;
                    leagueSettings.NumberOfStartingQuarterbacks = 2;
                    return await sut.AddOrUpdateLeagueSettingsAsync(GetMapper().Map<LeagueSettingsViewModel>(leagueSettings));
                });

            var expected = GetMapper().Map<LeagueSettingsViewModel>(leagueSettings);
            expected.LeagueName = "Test League";
            expected.NumberOfTeams = 2;

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }

        private League GetLeague() => new League
        {
            Name = "Test League",
            LeagueSettings = new LeagueSettings { SeasonId = 2019 },
            Teams = new List<Team>
            {
            new Team { Name = "Test Team One", OwnerName = "Test Owner One" },
            new Team { Name = "Test Team Two", OwnerName = "Test Owner Two" },
            }
        };

        private LeagueViewModel GetLeagueViewModel() => GetMapper().Map<LeagueViewModel>(GetLeague());

        private LeagueService GetSut(DraftContext context) => new LeagueService(NullLogger<LeagueService>.Instance, context, GetMapper());

        private IMapper GetMapper() => new Mapper(new MapperConfiguration(c =>
        {
            c.AddProfiles(new List<Profile>
                {
                    new LeagueProfile(),
                    new TeamProfile(),
                    new LeagueSettingsProfile(),
                });
        }));

        private async Task < (bool ok, T actual) > Execute<T>(
            Action<DraftContext> seed,
            Func < DraftContext, Task < (bool ok, T actual) >> test)
        {
            using var conn = new SqliteConnection("Datasource=:memory:");
            conn.Open();

            var options = new DbContextOptionsBuilder<DraftContext>()
                .UseSqlite(conn)
                .Options;

            using(var context = new DraftContext(options))
            {
                _ = await context.Database.EnsureCreatedAsync();
                seed(context);
            }

            using(var context = new DraftContext(options))
            {
                return await test(context);
            }
        }
    }
}