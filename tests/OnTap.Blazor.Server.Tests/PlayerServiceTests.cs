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
    public class PlayerServiceTests
    {
        [Fact]
        public async Task GetPlayersAsync_WhenPlayersDoNotExist_ReturnsEmptyCollection()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetPlayersAsync();
                });

            ok.Should().BeTrue();
            actual.Should().BeEquivalentTo(new List<PlayerViewModel>());
        }

        [Fact]
        public async Task GetPlayersAsync_WhenPlayersDoExist_ReturnsCollection()
        {
            var player = GetPlayer();

            var(ok, actual) = await Execute(
                async context =>
                {
                    context.Players.Add(player);                    
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.GetPlayersAsync();
                });

            ok.Should().BeTrue();
            actual
                .Should()
                .BeEquivalentTo(new List<PlayerViewModel>{ GetMapper().Map<PlayerViewModel>(player) });
        }

        [Fact]
        public async Task AddOrUpdatePlayerAsync_WhenPlayerDoesNotExist_AddsPlayer()
        {            
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    var player = GetMapper().Map<PlayerViewModel>(GetPlayer());
                    return await sut.AddOrUpdatePlayerAsync(player);
                });

            var expected = GetMapper().Map<PlayerViewModel>(GetPlayer());
            expected.Id = 1;                                       

            ok.Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task AddOrUpdatePlayerAsync_WhenPlayerDoesExist_UpdatesPlayer()
        {
            var statisticsVm = new StatisticViewModel { Stat = Statistic.PassingAttempts, Value = 10 };

            var(ok, actual) = await Execute(
                async context =>
                {
                    var player = GetPlayer();
                    context.Players.Add(player);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);
                    var(_, player) = await sut.GetPlayerAsync(1);
                    player.ProjectedStats = new List<StatisticViewModel>{ statisticsVm };
                    return await sut.AddOrUpdatePlayerAsync(player);
                });

            ok.Should().BeTrue();
            actual.Id.Should().Be(1);
            actual.ProjectedStats.First().Should().Be(statisticsVm);
        }

        [Fact]
        public async Task RemovePlayerAsync_WhenPlayerDoesNotExist_ReturnsDefault()
        {
            var(ok, actual) = await Execute(
                context => { },
                async context =>
                {
                    var sut = GetSut(context);;
                    return await sut.RemovePlayerAsync(0);
                });

            ok.Should().BeFalse();
            actual.Should().Be(new PlayerViewModel());
        }

        [Fact]
        public async Task RemovePlayerAsync_WhenPlayerDoesExist_ReturnsCorrectResult()
        {
            var player = GetPlayer();
            var(ok, actual) = await Execute(
                async context =>
                {                    
                    context.Players.Add(player);
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;                    
                    return await sut.RemovePlayerAsync(1);
                });
            
            ok.Should().BeTrue();
            actual.Should().Be(GetMapper().Map<PlayerViewModel>(player));
        }

        [Fact]
        public async Task RemovePlayerAsync_WhenPlayerDoesExist_RemovesPlayer()
        {
            var(ok, _) = await Execute(
                async context =>
                {
                    context.Players.Add(GetPlayer());
                    _ = await context.SaveChangesAsync();
                },
                async context =>
                {
                    var sut = GetSut(context);;
                    _ = await sut.RemovePlayerAsync(1);
                    return await sut.GetPlayerAsync(1);
                });

            ok.Should().BeFalse();
        }
        
        private PlayerService GetSut(DraftContext context) => new PlayerService(NullLogger<PlayerService>.Instance, context, GetMapper());

        private IMapper GetMapper() => new Mapper(new MapperConfiguration(c =>
        {
            c.AddProfiles(new List<Profile>
                {                    
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
       
        private Player GetPlayer() => new Player
        {
            SeasonId = 2019,
            Name = "Test Player",
            Position = Position.Quarterback,
            TeamShortName = "TTT"
        };
    }
}