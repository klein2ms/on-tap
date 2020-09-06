using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public class PlayerViewModel : ValueObject<PlayerViewModel>
    {        
        public int Id { get; set; }
        public string FirstName => FullName.Split(' ').FirstOrDefault() ?? string.Empty;
        public string LastName => FullName.Split(' ').LastOrDefault() ?? string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Position Position { get; set; }
        public string PositionName => Position.Name();
        public string PositionShortName => Position.ShortName();
        public int ByeWeek { get; set; }
        public int Best { get; set; }
        public int Worst { get; set; }
        public decimal Adp { get; set; }
        public decimal FantasyPoints { get; set; }
        public decimal VbdScore { get; set; }
        public int VbdOverallRank { get; set; }
        public int VbdPositionRank { get; set; }
        public string TeamName => GetTeamName(TeamShortName);
        public string TeamShortName { get; set; } = string.Empty;
        public string TeamLogoUrl => GetEspnLogoUrl(TeamShortName);
        public IEnumerable<StatisticViewModel> ProjectedStats { get; set; } = new List<StatisticViewModel>();
        public bool IsDrafted { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return FullName;
            yield return Position;
            yield return ByeWeek;
            yield return Best;
            yield return Worst;
            yield return Adp;
            yield return FantasyPoints;
            yield return VbdScore;
            yield return VbdOverallRank;
            yield return VbdPositionRank;
            yield return TeamShortName;
            foreach (var stat in ProjectedStats)
                yield return stat;         
        }

        private static string GetEspnLogoUrl(string teamShortName) 
            => $"https://a1.espncdn.com/combiner/i?img=/i/teamlogos/nfl/500/scoreboard/{teamShortName}.png";
        
        private static string GetNflLogoUrl(string teamShortName)
            => $"http://i.nflcdn.com/static/site/7.2/img/logos/teams-matte-80x53/{teamShortName}.png";

        private static string GetTeamName(string team)
        {
            if (string.IsNullOrWhiteSpace(team))
                return string.Empty;
                
            switch (team.ToUpper())
            {
                case "ARI":
                    return "Cardinals";
                case "ATL":
                    return "Falcons";
                case "BAL":
                    return "Ravens";
                case "BUF":
                    return "Bills";
                case "CAR":
                    return "Panthers";
                case "CHI":
                    return "Bears";
                case "CIN":
                    return "Bengals";
                case "CLE":
                    return "Browns";
                case "DAL":
                    return "Cowboys";
                case "DEN":
                    return "Broncos";
                case "DET":
                    return "Lions";
                case "GB":
                    return "Packers";
                case "HOU":
                    return "Texans";
                case "IND":
                    return "Colts";
                case "JAC":
                    return "Jaguars";
                case "KC":
                    return "Chiefs";
                case "LAC":
                    return "Chargers";
                case "LAR":
                    return "Rams";
                case "MIA":
                    return "Dolphins";
                case "MIN":
                    return "Vikings";
                case "NE":
                    return "Patriots";
                case "NO":
                    return "Saints";
                case "NYG":
                    return "Giants";
                case "NYJ":
                    return "Jets";
                case "OAK":
                    return "Raiders";
                case "PHI":
                    return "Eagles";
                case "PIT":
                    return "Steelers";
                case "SEA":
                    return "Seahawks";
                case "SF":
                    return "49ers";
                case "TB":
                    return "Buccaneers";
                case "TEN":
                    return "Titans";
                case "WAS":
                    return "Redskins";
                default:
                    return string.Empty;
            }
        }
    }

    public static class PlayerViewModelExtensions
    {
        public static decimal CalculateFantasyPoints(
            this PlayerViewModel player,
            LeagueSettingsViewModel settings)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return player
                .ProjectedStats
                .Select(s => (s.Stat, s.Value).ToFantasyPoints(settings))
                .Sum();
        }
    }
}