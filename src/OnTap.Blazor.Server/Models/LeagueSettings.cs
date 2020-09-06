using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class LeagueSettings
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int LeagueId { get; set; }
        public League League { get; set; }        
        public int NumberOfStartingQuarterbacks { get; set; }
        public int NumberOfStartingRunningBacks { get; set; }
        public int NumberOfStartingRunningBackOrWideReceivers { get; set; }
        public int NumberOfStartingWideReceivers { get; set; }
        public int NumberOfStartingWideReceiverOrTightEnds { get; set; }
        public int NumberOfStartingTightEnds { get; set; }
        public int NumberOfStartingDefenses { get; set; }
        public int NumberOfStartingKickers { get; set; }
        public int NumberOfBenchPlayers { get; set; }
        public decimal PointsPerPassingYard { get; set; }
        public decimal PointsPerPassingCompletion { get; set; }
        public decimal PointsPerPassingTouchdown { get; set; }
        public decimal PointsPerPassingInterception { get; set; }
        public decimal PointsPerPassingTwoPointConversion { get; set; }
        public decimal PointsPerRushingYard { get; set; }
        public decimal PointsPerRushingTouchdown { get; set; }
        public decimal PointsPerRushingTwoPointConversion { get; set; }
        public decimal PointsPerReceivingYard { get; set; }
        public decimal PointsPerReception { get; set; }
        public decimal PointsPerReceivingTouchdown { get; set; }
        public decimal PointsPerReceivingTwoPointConversion { get; set; }
        public decimal PointsPerPatMade { get; set; }
        public decimal PointsPerPatMissed { get; set; }
        public decimal PointsPerFieldGoalMadeLessThan40Yards { get; set; }
        public decimal PointsPerFieldGoalMade40To49Yards { get; set; }
        public decimal PointsPerFieldGoalMadeGreaterThan50Yards { get; set; }
        public decimal PointsPerFieldGoalMissedLessThan40Yards { get; set; }
        public decimal PointsPer25KickoffReturnYards { get; set; }
        public decimal PointsPer10PuntReturnYards { get; set; }
        public decimal PointsPerKickoffReturnTouchdown { get; set; }
        public decimal PointsPerPuntReturnTouchdown { get; set; }
        public decimal PointsPerInterceptionReturnTouchdown { get; set; }
        public decimal PointsPerFumbleReturnTouchdown { get; set; }
        public decimal PointsPerBlockedKickForTouchdown { get; set; }
        public decimal PointsPerSack { get; set; }
        public decimal PointsPerBlockedKick { get; set; }
        public decimal PointsPerDefensiveInterception { get; set; }
        public decimal PointsPerFumbleRecovery { get; set; }
        public decimal PointsPerSafety { get; set; }
        public decimal PointsPer0PointsAgainst { get; set; }
        public decimal PointsPer1To6PointsAgainst { get; set; }
        public decimal PointsPer7To13PointsAgainst { get; set; }
        public decimal PointsPer14To17PointsAgainst { get; set; }
        public decimal PointsPer18To21PointsAgainst { get; set; }
        public decimal PointsPer28To34PointsAgainst { get; set; }
        public decimal PointsPer35To45PointsAgainst { get; set; }
        public decimal PointsPer46OrMorePointsAgainst { get; set; }
        public decimal PointsPerTotalFumblesLost { get; set; }
    }
}
