using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace OnTap.Blazor.Shared.Tests.Unit
{
    public class RosterTests
    {
        [Fact]
        public void ClearProjectedDraftPicks_WhenRosterSpotsHaveProjections_RemovesTheProjections()
        {
            var sut = new Roster();
            sut.AddRosterSpots(new List<RosterSpot>
            {
                new RosterSpot
                {
                    Status = DraftPickStatus.Drafted, 
                    Player = new PlayerViewModel { Position = Position.Quarterback },                    
                },
                new RosterSpot
                {
                    Status = DraftPickStatus.Projected,
                    Player = new PlayerViewModel { Position = Position.RunningBack },                    
                },
                new RosterSpot
                {
                    Status = DraftPickStatus.Empty, 
                    Player = new PlayerViewModel(),                    
                }
            });

            sut.ClearProjectedDraftPicks();

            var expected = new List<RosterSpot>
            {
                new RosterSpot
                {
                    Status = DraftPickStatus.Drafted, 
                    Player = new PlayerViewModel { Position = Position.Quarterback },                    
                },
                new RosterSpot
                {
                    Status = DraftPickStatus.Empty, 
                    Player = new PlayerViewModel(),                    
                },
                new RosterSpot
                {
                    Status = DraftPickStatus.Empty, 
                    Player = new PlayerViewModel(),
                }
            };

            sut.RosterSpots.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void RecommendDraftPicks_GivenValidInput_ReturnsCorrectResult()
        {
            var qb1 = new PlayerViewModel { Position = Position.Quarterback, Adp = 12 };
            var rb1 = new PlayerViewModel { Position = Position.RunningBack, Adp = 8 };
            var rb2 = new PlayerViewModel { Position = Position.RunningBack, Adp = 9 };
            var wr1 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 7 };
            var wr2 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 13 };
            var wr3 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 15 };
            var te1 = new PlayerViewModel { Position = Position.TightEnd, Adp = 14 };

            var players = new List<PlayerViewModel> { qb1, rb1, rb2, wr1, wr2, wr3, te1 };

            var rosterSpots = new List<RosterSpot>
                {
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.Quarterback },
                    IsStarter = true,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.RunningBack },
                    IsStarter = true,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.RunningBack, Position.WideReceiver },
                    IsStarter = true,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.WideReceiver },
                    IsStarter = true,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.WideReceiver, Position.TightEnd },
                    IsStarter = true,
                    }
                };

            var sut = new Roster();
            sut.AddRosterSpots(rosterSpots);

            var expectedRoster = new List<RosterSpot>
                {
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.Quarterback },
                    IsStarter = true,
                    Status = DraftPickStatus.Projected, 
                    Player = qb1,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.RunningBack },
                    IsStarter = true,
                    Status = DraftPickStatus.Projected, 
                    Player = rb1,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.RunningBack, Position.WideReceiver },
                    IsStarter = true,
                    Status = DraftPickStatus.Projected, 
                    Player = wr1,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.WideReceiver },
                    IsStarter = true,
                    Status = DraftPickStatus.Projected, 
                    Player = wr2,
                    },
                    new RosterSpot
                    {
                    PossiblePositions = new [] { Position.WideReceiver, Position.TightEnd },
                    IsStarter = true,
                    Status = DraftPickStatus.Projected, 
                    Player = te1,
                    }
                };

            var expectedRemainingPlayers = new List<PlayerViewModel>
                {
                    rb2,
                    wr3,
                };

            var(roster, remainingPlayers) = sut.RecommendRoster(
                DraftPickRecommender.AdpComparer,
                players);

            _ = roster.RosterSpots.Should().BeEquivalentTo(expectedRoster);
            _ = remainingPlayers.Should().BeEquivalentTo(expectedRemainingPlayers);
        }

        [Fact]
        public void RecommendDraftPick_GivenValidInput_ReturnsCorrectResult()
        {
            var qb1 = new PlayerViewModel { Position = Position.Quarterback, Adp = 12 };
            var rb1 = new PlayerViewModel { Position = Position.RunningBack, Adp = 8 };
            var rb2 = new PlayerViewModel { Position = Position.RunningBack, Adp = 9 };
            var wr1 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 7 };
            var wr2 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 13 };
            var wr3 = new PlayerViewModel { Position = Position.WideReceiver, Adp = 15 };
            var te1 = new PlayerViewModel { Position = Position.TightEnd, Adp = 14 };

            var rosterSpots = new List<RosterSpot>
                {
                    new RosterSpot { Status = DraftPickStatus.Projected, Player = qb1, },
                    new RosterSpot { Status = DraftPickStatus.Projected, Player = rb1, },
                    new RosterSpot { Status = DraftPickStatus.Projected, Player = wr1, },
                    new RosterSpot { Status = DraftPickStatus.Projected, Player = wr2, },
                    new RosterSpot { Status = DraftPickStatus.Projected, Player = te1, },
                };

            var remainingPlayers = new List<PlayerViewModel> { rb2, wr3 };

            var sut = new Roster();
            sut.AddRosterSpots(rosterSpots);

            var expected = (wr1, new List<PlayerViewModel>
            {
                qb1, rb1, rb2, wr2, wr3, te1
            });

            var actual = (sut, remainingPlayers).RecommendDraftPick(DraftPickRecommender.AdpComparer);            

            actual.Should().BeEquivalentTo(expected);
        }
    }
}