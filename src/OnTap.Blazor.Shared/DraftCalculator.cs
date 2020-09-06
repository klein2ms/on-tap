using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public static class DraftCalculator
    {
        public static int CalculateDraftRound(int pick, int numberOfTeams)
        {
            ValidateIsPositive(nameof(pick), pick);
            ValidateIsPositive(nameof(numberOfTeams), numberOfTeams);
            return (int)Math.Ceiling((decimal)pick / numberOfTeams);
        }

        public static int CalculateNextDraftPick(int pick, int numberOfTeams)
        {
            ValidateIsPositive(nameof(pick), pick);
            ValidateIsPositive(nameof(numberOfTeams), numberOfTeams);
            return (2 * numberOfTeams * (CalculateDraftRound(pick, numberOfTeams) + 1) - (1 + pick + 2 * (numberOfTeams - 1)));
        }

        public static int CalculateLastDraftPick(int pick, int numberOfTeams)
        {
            ValidateIsPositive(nameof(pick), pick);
            ValidateIsPositive(nameof(numberOfTeams), numberOfTeams);

            if (pick <= numberOfTeams)
                return 0;

            return (2 * numberOfTeams * (CalculateDraftRound(pick, numberOfTeams)) - (1 + pick + 2 * (numberOfTeams - 1)));
        }

        public static IEnumerable<int> CalculateDraftPicks(int firstPick, int numberOfTeams, int numberOfRounds)
        {
            ValidateIsPositive(nameof(firstPick), firstPick);
            ValidateIsPositive(nameof(numberOfTeams), numberOfTeams);
            ValidateIsPositive(nameof(numberOfRounds), numberOfRounds);

            return Enumerable.Range(1, numberOfRounds)
                .Aggregate(new List<int>{ firstPick }, (picks, round) => 
                {
                    if (round == numberOfRounds)
                        return picks;

                    var nextPick = CalculateNextDraftPick(picks.Last(), numberOfTeams);
                    picks.Add(nextPick);
                    return picks;
                });        
        }        

        private static void ValidateIsPositive(string param, int val)
        {
            if (val <= 0)
                throw new ArgumentNullException($"{param} must be greater than zero but was: {val}");
        }
    }
}