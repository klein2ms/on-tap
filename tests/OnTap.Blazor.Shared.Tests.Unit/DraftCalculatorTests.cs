using FluentAssertions;
using Xunit;

namespace OnTap.Blazor.Shared.Tests.Unit
{
    public class DraftCalculatorTests
    {
        [Theory]
        [InlineData(13, 12, 2)]
        [InlineData(23, 12, 2)]
        public void CalculateDraftRound_GivenValidPickAndNumberOfTeams_ReturnsCorrectRound(
            int pick,
            int numberOfTeams,
            int expected)
        {
            var actual = DraftCalculator.CalculateDraftRound(pick, numberOfTeams);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 12, 24)]
        [InlineData(12, 12, 13)]
        public void CalulateNextDraftPick_GivenValidPickandNumberOfTeams_ReturnsCorrectPick(
            int pick,
            int numberOfTeams,
            int expected)
        {
            var actual = DraftCalculator.CalculateNextDraftPick(pick, numberOfTeams);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 12, 0)]
        [InlineData(12, 12, 0)]
        [InlineData(24, 12, 1)]
        [InlineData(13, 12, 12)]
        public void CalulateLastDraftPick_GivenValidPickandNumberOfTeams_ReturnsCorrectPick(
            int pick,
            int numberOfTeams,
            int expected)
        {
            var actual = DraftCalculator.CalculateLastDraftPick(pick, numberOfTeams);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 12, 4, 1, 24, 25, 48)]
        [InlineData(12, 12, 4, 12, 13, 36, 37)]
        public void CalulateDraftPicks_GivenValidInputs_ReturnsCorrectPicks(
            int firstPick,
            int numberOfTeams,
            int numberOfRounds,
            params int[] expected)
        {
            var actual = DraftCalculator.CalculateDraftPicks(firstPick, numberOfTeams, numberOfRounds);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}