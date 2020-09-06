using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public enum Statistic
    {
        None = 0,
        PassingAttempts,
        PassingCompletions,
        PassingYards,
        PassingTouchdowns,
        PassingInterceptions,
        RushingAttempts,
        RushingYards,
        RushingTouchdowns,
        Receptions,
        ReceivingYards,
        ReceivingTouchdowns,
        FumblesLost,
        FieldGoals,
        FieldGoalsAttempted,
        FieldGoalsMissed,
        ExtraPoints,
        Sacks,
        Interceptions,
        FumblesRecovered,
        ForcedFumbles,
        DefensiveTouchdowns,
        Safeties,
        PointsAgainst,
        YardsAgainst,
    }

    public static class StatisticExtensions
    {
        public static decimal ToFantasyPoints(
            this(Statistic stat, decimal value)projection,
            LeagueSettingsViewModel settings)
        {
            switch (projection.stat)
            {                
                case Statistic.PassingCompletions:
                    return settings.PointsPerPassingCompletion * projection.value;

                case Statistic.PassingYards:
                    return settings.PointsPerPassingYard * projection.value;

                case Statistic.PassingTouchdowns:
                    return settings.PointsPerPassingTouchdown * projection.value;

                case Statistic.PassingInterceptions:
                    return settings.PointsPerPassingInterception * projection.value;
                
                case Statistic.RushingYards:
                    return settings.PointsPerRushingYard * projection.value;
                
                case Statistic.RushingTouchdowns:
                    return settings.PointsPerRushingTouchdown * projection.value;

                case Statistic.Receptions:
                    return settings.PointsPerReception * projection.value;

                case Statistic.ReceivingYards:
                    return settings.PointsPerReceivingYard * projection.value;

                case Statistic.ReceivingTouchdowns:
                    return settings.PointsPerReceivingTouchdown * projection.value;

                case Statistic.FumblesLost:
                    return settings.PointsPerTotalFumblesLost * projection.value;

                case Statistic.Sacks:
                    return settings.PointsPerSack * projection.value;

                case Statistic.Interceptions:
                    return settings.PointsPerDefensiveInterception * projection.value;

                case Statistic.FumblesRecovered:
                    return settings.PointsPerFumbleRecovery * projection.value;

                case Statistic.ForcedFumbles:
                    return 0 * projection.value;

                case Statistic.DefensiveTouchdowns:
                    return new List<decimal>
                        {
                            (settings.PointsPerKickoffReturnTouchdown * projection.value),
                            (settings.PointsPerPuntReturnTouchdown * projection.value),
                            (settings.PointsPerInterceptionReturnTouchdown * projection.value),
                            (settings.PointsPerFumbleReturnTouchdown * projection.value),
                            (settings.PointsPerBlockedKickForTouchdown * projection.value),
                        }.Average();

                case Statistic.Safeties:
                    return settings.PointsPerSafety * projection.value;

                case Statistic.PointsAgainst:
                    return new List<decimal>
                        {
                            (settings.PointsPer0PointsAgainst * projection.value * 0),
                            (settings.PointsPer1To6PointsAgainst * projection.value * 0.0037m),
                            (settings.PointsPer7To13PointsAgainst * projection.value * 0.0262m),
                            (settings.PointsPer14To17PointsAgainst * projection.value * 0.0524m),
                            (settings.PointsPer18To21PointsAgainst * projection.value * 0.1236m),
                            (settings.PointsPer28To34PointsAgainst * projection.value * 0.2622m),
                            (settings.PointsPer35To45PointsAgainst * projection.value * 0.1910m),
                            (settings.PointsPer46OrMorePointsAgainst * projection.value * 0.0337m),
                        }.Sum();

                case Statistic.FieldGoals:
                    return new List<decimal>
                        {
                            (settings.PointsPerFieldGoalMadeLessThan40Yards * projection.value * 0.6m),
                            (settings.PointsPerFieldGoalMade40To49Yards * projection.value * 0.28m),
                            (settings.PointsPerFieldGoalMadeGreaterThan50Yards * projection.value * 0.12m),
                        }.Sum();

                case Statistic.FieldGoalsMissed:
                    return settings.PointsPerFieldGoalMissedLessThan40Yards * projection.value * 0.13m;

                case Statistic.ExtraPoints:
                    return settings.PointsPerPatMade * projection.value;

                case Statistic.YardsAgainst:
                default:
                    return 0;
            }
        }
    }
}