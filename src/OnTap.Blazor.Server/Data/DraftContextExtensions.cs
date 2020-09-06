using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnTap.Blazor.Server.Models;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Data
{
    public static class DraftContextExtensions
    {
        public static async Task<DraftContext> SeedAsync(
            this DraftContext context,
            string playersAsJson)
        {
            _ = await context.Database.EnsureCreatedAsync();

            await context.SeedLeagueAsync();
            await context.SeedPlayersAsync(playersAsJson);

            return context;
        }

        private static async Task SeedLeagueAsync(
            this DraftContext context)
        {
            await Task.Run(async() =>
            {
                if (context.Leagues.Any())
                    return;

                var league = new League
                {
                    Name = "Black Power",
                    LeagueSettings = new LeagueSettings
                    {
                    SeasonId = 2019,
                    NumberOfStartingQuarterbacks = 1,
                    NumberOfStartingRunningBacks = 1,
                    NumberOfStartingRunningBackOrWideReceivers = 1,
                    NumberOfStartingWideReceivers = 2,
                    NumberOfStartingWideReceiverOrTightEnds = 1,
                    NumberOfStartingTightEnds = 1,
                    NumberOfStartingDefenses = 1,
                    NumberOfStartingKickers = 1,
                    NumberOfBenchPlayers = 5,
                    PointsPerPassingYard = 0.04m,
                    PointsPerPassingCompletion = 0.3m,
                    PointsPerPassingTouchdown = 6,
                    PointsPerPassingInterception = -2,
                    PointsPerPassingTwoPointConversion = 2,
                    PointsPerRushingYard = 0.1m,
                    PointsPerRushingTouchdown = 6,
                    PointsPerRushingTwoPointConversion = 2,
                    PointsPerReceivingYard = 0.1m,
                    PointsPerReception = 0.5m,
                    PointsPerReceivingTouchdown = 6,
                    PointsPerReceivingTwoPointConversion = 2,
                    PointsPerPatMade = 1,
                    PointsPerPatMissed = -1,
                    PointsPerFieldGoalMadeLessThan40Yards = 3,
                    PointsPerFieldGoalMade40To49Yards = 4,
                    PointsPerFieldGoalMadeGreaterThan50Yards = 5,
                    PointsPerFieldGoalMissedLessThan40Yards = -1,
                    PointsPer25KickoffReturnYards = 1,
                    PointsPer10PuntReturnYards = 1,
                    PointsPerKickoffReturnTouchdown = 6,
                    PointsPerPuntReturnTouchdown = 6,
                    PointsPerInterceptionReturnTouchdown = 6,
                    PointsPerFumbleReturnTouchdown = 6,
                    PointsPerBlockedKickForTouchdown = 6,
                    PointsPerSack = 1,
                    PointsPerBlockedKick = 2,
                    PointsPerDefensiveInterception = 2,
                    PointsPerFumbleRecovery = 2,
                    PointsPerSafety = 2,
                    PointsPer0PointsAgainst = 12,
                    PointsPer1To6PointsAgainst = 7,
                    PointsPer7To13PointsAgainst = 4,
                    PointsPer14To17PointsAgainst = 1,
                    PointsPer18To21PointsAgainst = 1,
                    PointsPer28To34PointsAgainst = -1,
                    PointsPer35To45PointsAgainst = -4,
                    PointsPer46OrMorePointsAgainst = -7,
                    PointsPerTotalFumblesLost = -2,
                    },
                    Teams = new List<Team>
                    {
                    new Team { Name = @"Hotel ODELL", OwnerName = "Andre Williams" },
                    new Team { Name = @"Jimmy G Unit", OwnerName = "Matthew Hayes" },
                    new Team { Name = @"Baltimore One Time", OwnerName = "Thomas Abernethy" },
                    new Team { Name = @"Midway Mack", OwnerName = "Matthew Ives" },
                    new Team { Name = @"Shit Outta Luck", OwnerName = "Travis Edson" },
                    new Team { Name = @"Mo Harris All-Stars", OwnerName = "Adam McPherson" },
                    new Team { Name = @"SchuttAir Advantages", OwnerName = "Mason Klein" },
                    new Team { Name = @"North Dakota State Bison", OwnerName = "Joshua Evans" },
                    new Team { Name = @"G Lack there of $$$", OwnerName = "Garrett Lindsey" },
                    new Team { Name = @"CAN'Tstop WON'Tstop", OwnerName = "Justin Holloman" },
                    new Team { Name = @"The Commish", OwnerName = @"Wayne Rudd Jr." },
                    new Team { Name = @"Hodors Pro Door Service", OwnerName = "Zachary Sutter" },
                    }
                };

                context.Leagues.Add(league);

                _ = await context.SaveChangesAsync();
            });
        }

        public static async Task SeedPlayersAsync(
            this DraftContext context,
            string json)
        {
            await Task.Run(async() =>
            {
                if (context.Players.Any())
                    return;

                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                var players = JsonConvert.DeserializeObject<List<Player>>(json, settings);

                context.Players.AddRange(players);

                _ = await context.SaveChangesAsync();
            });
        }
    }
}