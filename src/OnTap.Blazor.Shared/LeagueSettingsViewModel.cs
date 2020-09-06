using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public class LeagueSettingsViewModel : ValueObject<LeagueSettingsViewModel>
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public int SeasonId { get; set; }

            [Required]
            public int LeagueId { get; set; }

            [Required]
            public string LeagueName { get; set; }
            public int NumberOfTeams { get; set; }
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
            public int RosterSize => new []
            {
                NumberOfStartingQuarterbacks,
                NumberOfStartingRunningBacks,
                NumberOfStartingRunningBackOrWideReceivers,
                NumberOfStartingWideReceivers,
                NumberOfStartingWideReceiverOrTightEnds,
                NumberOfStartingTightEnds,
                NumberOfStartingDefenses,
                NumberOfStartingKickers,
                NumberOfBenchPlayers,
            }.Sum();
        }

    public static class SettingsViewModelExtensions
    {
        public static int GetNumberOfPosition(
            this LeagueSettingsViewModel settings,
            Position position)
        {
            switch (position)
            {
                case Position.Quarterback:
                    return settings.NumberOfStartingQuarterbacks;

                case Position.RunningBack:
                    return settings.NumberOfStartingRunningBacks;

                case Position.WideReceiver:
                    return settings.NumberOfStartingWideReceivers;

                case Position.TightEnd:
                    return settings.NumberOfStartingTightEnds;

                case Position.Defense:
                    return settings.NumberOfStartingDefenses;

                case Position.Kicker:
                    return settings.NumberOfStartingKickers;

                default:
                    return 0;
            }
        }

        public static IEnumerable<RosterSpot> GetRosterSpots(this LeagueSettingsViewModel settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var rosterSpots = new List<RosterSpot>();
            rosterSpots.Append(GetRosterSpots(Position.Quarterback, settings.NumberOfStartingQuarterbacks, true));
            rosterSpots.Append(GetRosterSpots(Position.RunningBack, settings.NumberOfStartingRunningBacks, true));
            rosterSpots.Append(GetRosterSpots(
                new List<Position> { Position.RunningBack, Position.WideReceiver }, 
                settings.NumberOfStartingRunningBackOrWideReceivers, 
                true));            
            rosterSpots.Append(GetRosterSpots(Position.WideReceiver, settings.NumberOfStartingWideReceivers, true));
            rosterSpots.Append(GetRosterSpots(
                new List<Position> { Position.WideReceiver, Position.TightEnd }, 
                settings.NumberOfStartingWideReceiverOrTightEnds, 
                true));            
            rosterSpots.Append(GetRosterSpots(Position.TightEnd, settings.NumberOfStartingTightEnds, true));            
            rosterSpots.Append(GetRosterSpots(Position.Defense, settings.NumberOfStartingDefenses, true));
            rosterSpots.Append(GetRosterSpots(Position.Kicker, settings.NumberOfStartingKickers, true));
            rosterSpots.Append(GetRosterSpots(
                new List<Position> { 
                    Position.Quarterback,
                    Position.RunningBack,
                    Position.WideReceiver, 
                    Position.TightEnd,
                    Position.Defense,
                    Position.Kicker }, 
                settings.NumberOfBenchPlayers, 
                false));

            return rosterSpots;           
        }

        private static IEnumerable<RosterSpot> GetRosterSpots(Position position, int number, bool isStarter)
            => GetRosterSpots(new List<Position>{ position }, number, isStarter);        

        private static IEnumerable<RosterSpot> GetRosterSpots(IEnumerable<Position> positions, int number, bool isStarter)
        {
            var toReturn = new List<RosterSpot>();

            for (int i = 0; i < number; i++)
            {
                toReturn.Add(new RosterSpot 
                {
                    Name = $"{string.Join($"/", positions.Select(p => p.ShortName()))}{i + 1}",
                    IsStarter = isStarter,
                    PossiblePositions = positions
                });
            }

            return toReturn;
        }
    }
}