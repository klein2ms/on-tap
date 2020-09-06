using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace OnTap.Blazor.Shared.Tests.Unit
{
    public class DraftPickRecommenderTests
    {
        [Fact]
        public void RecommendPlayerAtPositionBy_GivenValidInput_ReturnsCorrectResult()
        {
            var players = new List<PlayerViewModel>
            {
                new PlayerViewModel { Position = Position.Quarterback, Adp = 10 },
                new PlayerViewModel { Position = Position.Quarterback, Adp = 12 },
                new PlayerViewModel { Position = Position.RunningBack, Adp = 8 },
                new PlayerViewModel { Position = Position.RunningBack, Adp = 9 },
                new PlayerViewModel { Position = Position.WideReceiver, Adp = 7 },
            };

            var positions = new List<Position>
            {
                Position.RunningBack,
                Position.WideReceiver,
                Position.TightEnd
            };

            var expected = new List<PlayerViewModel>
            {
                new PlayerViewModel { Position = Position.WideReceiver, Adp = 7 },                
                new PlayerViewModel { Position = Position.RunningBack, Adp = 8 },
            };

            var actual = DraftPickRecommender
                .RecommendPlayersAtPositions(
                    DraftPickRecommender.AdpComparer,
                    players,
                    positions);
            
            actual.Should().BeEquivalentTo(expected);
            actual.Should().BeInAscendingOrder(p => p.Adp);
        }
    }
}