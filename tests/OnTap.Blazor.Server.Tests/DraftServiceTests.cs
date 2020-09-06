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
    public class DraftServiceTests
    {
        [Fact]
        public async Task GetDraftAsync_WhenDraftDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetDraftAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().Be(new DraftViewModel());
        }

        [Fact]
        public async Task GetDraftAsync_WhenDraftDoesExist_ReturnsDraft()
        {
            var draft = GetDraft();

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(draft.League);
                    context.Drafts.Add(draft);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetDraftAsync(draft.Id);
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .Be(GetMapper().Map<DraftViewModel>(draft));
        }

        [Fact]
        public async Task AddOrUpdateDraftAsync_WhenDraftDoesNotExist_AddsDraft()
        {
            var vm = GetDraftViewModel();

            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetMapper().Map<League>(vm.League);
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                    vm.League.Id = league.Id;
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.AddOrUpdateDraftAsync(vm);
                });

            var expected = GetDraftViewModel();
            expected.Id = 1;
            expected.League.Id = 1;
            expected.League.LeagueSettings.Id = 1;
            expected.League.LeagueSettings.LeagueId = 1;
            expected.League.LeagueSettings.LeagueName = "Test League";
            expected.League.LeagueSettings.NumberOfTeams = 2;
            expected.League.Teams.First().Id = 1;
            expected.League.Teams.Last().Id = 2;

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task AddOrUpdateDraftAsync_WhenDraftDoesExist_UpdatesDraft()
        {
            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                    var draft = GetDraft();
                    draft.League = league;
                    context.Drafts.Add(draft);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);
                    var(_, draft) = await sut.GetDraftAsync(1);
                    draft.IsComplete = true;
                    return await sut.AddOrUpdateDraftAsync(draft);
                });

            ok.Should().BeTrue();
            actual.Id.Should().Be(1);
            actual.IsComplete.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveDraftAsync_WhenDraftDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.RemoveDraftAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().Be(new DraftViewModel());
        }

        [Fact]
        public async Task RemoveDraftAsync_WhenDraftDoesExist_ReturnsCorrectResult()
        {
            var draft = GetDraft();
            var(ok, actual) = await Execute(
                async context =>
                {
                    var league = GetLeague();
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                    draft.League = league;
                    context.Drafts.Add(draft);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    var(_, draft) = await sut.GetDraftAsync(1);
                    return await sut.RemoveDraftAsync(1);
                });

            draft.League = null;
            ok.Should().BeTrue();
            actual.Should().Be(GetMapper().Map<DraftViewModel>(draft));
        }

        [Fact]
        public async Task RemoveDraftAsync_WhenDraftDoesExist_RemovesDraft()
        {
            var(ok, _) = await Execute(
                async context =>
                {
                    context.Drafts.Add(GetMapper().Map<Draft>(GetDraft()));
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    _ = await sut.RemoveDraftAsync(1);
                    return await sut.GetDraftAsync(1);
                });

            ok.Should().BeFalse();
        }

        [Fact]
        public async Task AddOrUpdateDraftPickAsync_WhenDraftPickDoesNotExist_AddsDraftPick()
        {
            var league = GetLeague();
            var draft = GetDraft();
            var player = GetPlayer();

            var draftPick = new DraftPickViewModel
            {
                DraftId = 1,
                Player = new PlayerViewModel { Id = 1 },
                Team = new TeamViewModel { Id = 1 }
            };

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                    draft.League = league;
                    context.Drafts.Add(draft);
                    context.Players.Add(player);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);
                    return await sut.AddDraftPickAsync(draftPick);
                });

            var expected = new DraftPickViewModel
            {
                Id = 1,
                DraftId = 1,
                Player = GetMapper().Map<PlayerViewModel>(player),
                Team = GetMapper().Map<TeamViewModel>(league.Teams.First())
            };

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }        

        [Fact]
        public async Task RemoveDraftPickAsync_WhenDraftPickDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);
                    return await sut.RemoveDraftPickAsync(0);
                });

            ok.Should().Be(false);
            actual.Should().Be(new DraftPickViewModel());
        }

        [Fact]
        public async Task RemoveDraftPickAsync_WhenDraftPickDoesExist_RemovesDraftPick()
        {
            var league = GetLeague();
            var draft = GetDraft();
            var player = GetPlayer();
            var draftpick = new DraftPick
            {
                DraftId = 1,
                TeamId = 1,
                Team = league.Teams.First(),
                PlayerId = 1,
                Player = player,
                Number = 20
            };

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Leagues.Add(league);
                    _ = await context.SaveChangesAsync();
                    draft.League = league;
                    context.Drafts.Add(draft);
                    context.Players.Add(player);
                    _ = await context.SaveChangesAsync();
                    context.DraftPicks.Add(draftpick);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);
                    return await sut.RemoveDraftPickAsync(1);
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .Be(GetMapper().Map<DraftPickViewModel>(draftpick));
        }

        private DraftService GetSut(DraftContext context) => new DraftService(NullLogger<DraftService>.Instance, context, GetMapper());

        private IMapper GetMapper() => new Mapper(new MapperConfiguration(c =>
        {
            c.AddProfiles(new List<Profile>
                {
                    new DraftProfile(),
                    new LeagueProfile(),
                    new LeagueSettingsProfile(),
                    new TeamProfile(),
                    new DraftPickProfile(),
                    new DraftPositionProfile(),
                    new PlayerProfile()
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

        private Draft GetDraft() => new Draft
        {
            League = GetLeague()
        };

        private DraftViewModel GetDraftViewModel() => GetMapper().Map<DraftViewModel>(GetDraft());

        private Player GetPlayer() => new Player
        {
            SeasonId = 2019,
            Name = "Test Player",
            Position = Position.Quarterback,
            TeamShortName = "TTT"
        };
    }
}