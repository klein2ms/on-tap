using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace OnTap.Blazor.Shared.Tests.Unit
{
    public class VbdCalculatorTests
    {
        [Fact]
        public void CalculateDraftValue_WhenBaselineDoesNotContainAllPositions_ReturnsCorrectResult()
        {
            var baselines = new Dictionary<Position, PlayerViewModel>
            {
                [Position.RunningBack] = new PlayerViewModel { FantasyPoints = 400 },
                [Position.WideReceiver] = new PlayerViewModel { FantasyPoints = 300 },
                [Position.Quarterback] = new PlayerViewModel { FantasyPoints = 200 },
                [Position.TightEnd] = new PlayerViewModel { FantasyPoints = 100 },
            };

            var players = new List<PlayerViewModel>
            {
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 425 },
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 400 },
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 375 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 325 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 300 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 275 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 225 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 200 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 175 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 125 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 100 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 75 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25 },
            };

            var actual = VbdCalculator.CalculateDraftValue(baselines, players);

            var expected = new List<PlayerViewModel>
            {
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 425, VbdScore = 25,    VbdOverallRank = 1,  VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 400, VbdScore = 0,     VbdOverallRank = 5,  VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.RunningBack,  FantasyPoints = 375, VbdScore = -25,   VbdOverallRank = 9,  VbdPositionRank = 3 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 325, VbdScore = 25,    VbdOverallRank = 2,  VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 300, VbdScore = 0,     VbdOverallRank = 6,  VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.WideReceiver, FantasyPoints = 275, VbdScore = -25,   VbdOverallRank = 10, VbdPositionRank = 3 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 225, VbdScore = 25,    VbdOverallRank = 3,  VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 200, VbdScore = 0,     VbdOverallRank = 7,  VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.Quarterback,  FantasyPoints = 175, VbdScore = -25,   VbdOverallRank = 11, VbdPositionRank = 3 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 125, VbdScore = 25,    VbdOverallRank = 4,  VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 100, VbdScore = 0,     VbdOverallRank = 8,  VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.TightEnd,     FantasyPoints = 75,  VbdScore = -25,   VbdOverallRank = 12, VbdPositionRank = 3 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50,  VbdScore = -1000, VbdOverallRank = 13, VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50,  VbdScore = -1000, VbdOverallRank = 14, VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.Defense,      FantasyPoints = 50,  VbdScore = -1000, VbdOverallRank = 15, VbdPositionRank = 3 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25,  VbdScore = -1000, VbdOverallRank = 16, VbdPositionRank = 1 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25,  VbdScore = -1000, VbdOverallRank = 17, VbdPositionRank = 2 },
                new PlayerViewModel { Position = Position.Kicker,       FantasyPoints = 25,  VbdScore = -1000, VbdOverallRank = 18, VbdPositionRank = 3 },
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Top100BaseLine_WhenPlayersContainsMultiplePositions_ReturnsLastPlayerOfGroup()
        {
            var players = new List<PlayerViewModel>();

            void addPlayers(int number, int numberInTop100, Position position)
            {
                for (var i = 1; i <= number; i++)
                {
                    players.Add(new PlayerViewModel
                    {
                        FullName = i.ToString(),
                        Adp = i <= numberInTop100 ? i : i + 100,
                        Position = position
                    });
                }
            };

            addPlayers(50, 35, Position.RunningBack);
            addPlayers(50, 42, Position.WideReceiver);
            addPlayers(32, 13, Position.Quarterback);
            addPlayers(32, 10, Position.TightEnd);
            addPlayers(32, 0, Position.Defense);
            addPlayers(32, 0, Position.Kicker);

            var expected = new Dictionary<Position, PlayerViewModel>
            {
                [Position.RunningBack] = new PlayerViewModel
                {
                    FullName = "35",
                    Adp = 35,
                    Position = Position.RunningBack
                },
                [Position.WideReceiver] = new PlayerViewModel
                {
                    FullName = "42",
                    Adp = 42,
                    Position = Position.WideReceiver
                },
                [Position.Quarterback] = new PlayerViewModel
                {
                    FullName = "13",
                    Adp = 13,
                    Position = Position.Quarterback
                },
                [Position.TightEnd] = new PlayerViewModel
                {
                    FullName = "10",
                    Adp = 10,
                    Position = Position.TightEnd
                },
            };

            var actual = VbdCalculator.GetTop100Baselines(players);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ScarcityBaseline_WhenPlayersContainsMultiplePositions_ReturnsWorstStarter()
        {
            var players = new List<PlayerViewModel>();

            void addPlayers(int number, Position position)
            {
                for (var i = 1; i <= number; i++)
                {
                    players.Add(new PlayerViewModel
                    {
                        FullName = i.ToString(),
                        Adp = i,
                        Position = position
                    });
                }
            };

            addPlayers(50, Position.RunningBack);
            addPlayers(50, Position.WideReceiver);
            addPlayers(32, Position.Quarterback);
            addPlayers(32, Position.TightEnd);
            addPlayers(32, Position.Defense);
            addPlayers(32, Position.Kicker);

            var settings = new LeagueSettingsViewModel
            {
                NumberOfStartingQuarterbacks = 1,
                NumberOfStartingRunningBacks = 2,
                NumberOfStartingWideReceivers = 4,
                NumberOfStartingTightEnds = 2,
                NumberOfStartingDefenses = 1,
                NumberOfStartingKickers = 1,
                NumberOfBenchPlayers = 5,
                NumberOfTeams = 12,
            };

            var expected = new Dictionary<Position, PlayerViewModel>
            {
                [Position.RunningBack] = new PlayerViewModel
                {
                    FullName = "24",
                    Adp = 24,
                    Position = Position.RunningBack
                },
                [Position.WideReceiver] = new PlayerViewModel
                {
                    FullName = "48",
                    Adp = 48,
                    Position = Position.WideReceiver
                },
                [Position.Quarterback] = new PlayerViewModel
                {
                    FullName = "12",
                    Adp = 12,
                    Position = Position.Quarterback
                },
                [Position.TightEnd] = new PlayerViewModel
                {
                    FullName = "24",
                    Adp = 24,
                    Position = Position.TightEnd
                },
                [Position.Defense] = new PlayerViewModel
                {
                    FullName = "12",
                    Adp = 12,
                    Position = Position.Defense
                },
                [Position.Kicker] = new PlayerViewModel
                {
                    FullName = "12",
                    Adp = 12,
                    Position = Position.Kicker
                },
            };

            var actual = VbdCalculator.GetScarcityBaselines(players, settings);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
