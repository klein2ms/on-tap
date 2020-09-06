using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public static class VbdCalculator
    {
        // Recalculate based on Need Factor

        public static IDictionary<Position, PlayerViewModel> GetTop100Baselines(
            IReadOnlyCollection<PlayerViewModel> players)
            => players
                .Where(p => p.Adp != 0)
                .OrderBy(p => p.Adp)
                .Take(100)
                .GroupBy(p => p.Position)
                .ToDictionary(
                    g => g.Key,
                    g => g.LastOrDefault() ?? new PlayerViewModel { Position = g.Key });

        public static IDictionary<Position, PlayerViewModel> GetScarcityBaselines(
            IReadOnlyCollection<PlayerViewModel> players,
            LeagueSettingsViewModel settings)
            => players
                .Where(p => p.Adp != 0)                
                .OrderBy(p => p.Adp)
                .GroupBy(p => p.Position)
                .ToDictionary(
                    g => g.Key,
                    g => g.ElementAtOrDefault(
                            settings.GetNumberOfPosition(g.Key) * settings.NumberOfTeams - 1)
                        ?? new PlayerViewModel { Position = g.Key });

        public static IEnumerable<PlayerViewModel> CalculateDraftValue(
            IDictionary<Position, PlayerViewModel> baselines,
            IReadOnlyCollection<PlayerViewModel> players)
        {
            var toReturn = players
                .ToList()
                .Select(p =>
                {
                    baselines.TryGetValue(p.Position, out var baseline);
                    if (baseline != null)
                        p.VbdScore = p.FantasyPoints - baseline.FantasyPoints;
                    else
                        p.VbdScore = -1000;
                    return p;
                })
                .OrderByDescending(p => p.VbdScore)
                .ThenByDescending(p => p.FantasyPoints)
                .Select((p, i) =>
                {
                    p.VbdOverallRank = i + 1;
                    return p;
                });

            IEnumerable<PlayerViewModel> rankBy(Position position)
             => toReturn
                    .Where(p => p.Position == position)
                    .OrderByDescending(p => p.VbdScore)
                    .ThenByDescending(p => p.FantasyPoints)
                    .Select((p, i) =>
                    {
                        p.VbdPositionRank = i + 1;
                        return p;
                    });

            return new List<PlayerViewModel>()
                .Append(rankBy(Position.Quarterback))
                .Append(rankBy(Position.RunningBack))
                .Append(rankBy(Position.WideReceiver))
                .Append(rankBy(Position.TightEnd))
                .Append(rankBy(Position.Defense))
                .Append(rankBy(Position.Kicker))
                .OrderBy(p => p.VbdOverallRank);
        }
    }
}