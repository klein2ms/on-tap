using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public static class DraftPickRecommender
    {
        public static IComparer<PlayerViewModel> AdpComparer =>
            Comparer<PlayerViewModel>.Create((x, y) =>
                x.Adp > y.Adp ? 1
                : x.Adp < y.Adp ? -1
                : 0);
        
        public static IComparer<PlayerViewModel> VbdComparer =>
            Comparer<PlayerViewModel>.Create((x,y) =>
                x.VbdScore > y.VbdScore ? -1
                : x.VbdScore < y.VbdScore ? 1
                : 0);

        public static IEnumerable<PlayerViewModel> RecommendPlayersAtPositions(
            IComparer<PlayerViewModel> comparer,
            IReadOnlyCollection<PlayerViewModel> players,
            IReadOnlyCollection<Position> positions)
        {
            var needs = positions
                .GroupBy(p => p)
                .Select(g => (positon: g.Key, count: g.Count()))
                .ToDictionary(k => k.positon, v => v.count);

            return players
                .Where(p => positions.Contains(p.Position))
                .GroupBy(p => p.Position)
                .SelectMany(g => g.OrderBy(comparer).Take(needs[g.Key]))
                .OrderBy(comparer)
                .ToList();
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrderBy<T>(
            this IEnumerable<T> enumerable,
            IComparer<T> comparer)
        {
            var toReturn = enumerable.ToList();
            toReturn.Sort(comparer);
            return toReturn;
        }
    }
}