using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public class Roster : ValueObject<Roster>
    {
        private readonly List<RosterSpot> _rosterSpots = new List<RosterSpot>();
        public IReadOnlyCollection<RosterSpot> RosterSpots => _rosterSpots.ToList();

        public void AddRosterSpots(IReadOnlyCollection<RosterSpot> spots)
            => _rosterSpots.AddRange(spots ?? new List<RosterSpot>());

        public void RemoveRosterSpots(IReadOnlyCollection<RosterSpot> spots)
        {
            if (spots == null)
                return;

            spots.ToList().ForEach(r => _ = _rosterSpots.Remove(r));
        }

        public void ClearProjectedDraftPicks()
            => _rosterSpots
                .ForEach(r => 
                {
                    if (r.Pick.status != DraftPickStatus.Projected)
                        return;

                    r.Status = DraftPickStatus.Empty;
                    r.Player = new PlayerViewModel();                   
                });

        public IReadOnlyCollection<RosterSpot> GetEmptyRosterSpots()
            => RosterSpots
                .Where(r => r.Pick.status != DraftPickStatus.Drafted)
                .ToList();
        
        public bool FillRosterSpot(PlayerViewModel player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            
            var rosterSpot = _rosterSpots
                .Where(r => r.PossiblePositions.Contains(player.Position)
                    && r.Pick.status != DraftPickStatus.Drafted)
                .OrderByDescending(r => r.IsStarter)
                .ThenBy(r => r.PossiblePositions.Count())
                .FirstOrDefault();
            
            if (rosterSpot == null)
                return false;
            
            rosterSpot.Status = DraftPickStatus.Drafted;
            rosterSpot.Player = player;
            return true;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var spot in _rosterSpots)
                yield return spot;
        }        
    }

    public static class RosterExtensions
    {
        public static (Roster roster, IReadOnlyCollection<PlayerViewModel> remainingPlayers) RecommendRoster(
            this Roster roster,
            IComparer<PlayerViewModel> comparer,
            IReadOnlyCollection<PlayerViewModel> players)
        {
            var emptyRosterSpots = roster
                .GetEmptyRosterSpots()
                .ToList();

            var recommendations = DraftPickRecommender
                .RecommendPlayersAtPositions(
                    comparer,
                    players.ToList(),
                    emptyRosterSpots.SelectMany(r => r.PossiblePositions).ToList())
                    .ToList();

            var projections = emptyRosterSpots
                .OrderBy(r => r.IsStarter)
                .ThenBy(r => r.PossiblePositions.Count())
                // .ThenBy(r => 
                //     r.PossiblePositions.Contains(Position.Quarterback)
                //     || r.PossiblePositions.Contains(Position.RunningBack)
                //     || r.PossiblePositions.Contains(Position.WideReceiver)
                //     || r.PossiblePositions.Contains(Position.WideReceiver) ? 1 : -1)
                .Select(r =>
                {
                    var pick = recommendations
                        .Where(p => r.PossiblePositions.Contains(p.Position))
                        .OrderBy(comparer)
                        .FirstOrDefault();

                    _ = recommendations.Remove(pick);

                    r.Status = DraftPickStatus.Projected;
                    r.Player = pick;
                    return r;
                })
                .ToList();                   

            roster.RemoveRosterSpots(emptyRosterSpots);
            roster.AddRosterSpots(projections);

            return (roster, recommendations);
        }

        public static (PlayerViewModel pick, IReadOnlyCollection<PlayerViewModel> remainingPlayers) RecommendDraftPick(
            this Roster roster,
            IReadOnlyCollection<PlayerViewModel> players,
            IComparer<PlayerViewModel> comparer)
            => roster
                .RecommendRoster(comparer, players)
                .RecommendDraftPick(comparer);

        public static (PlayerViewModel pick, IReadOnlyCollection<PlayerViewModel> remainingPlayers) RecommendDraftPick(
            this (Roster roster, IReadOnlyCollection<PlayerViewModel> remainingPlayers) recommendedRoster,
            IComparer<PlayerViewModel> comparer)
        {
            if (recommendedRoster.roster == null)
                throw new ArgumentNullException(nameof(recommendedRoster.roster));
            
            if (recommendedRoster.remainingPlayers == null)
                throw new ArgumentNullException(nameof(recommendedRoster.remainingPlayers));

            // get the projected picks
            var projections = recommendedRoster
                .roster
                .RosterSpots
                .Where(r => r.Pick.status == DraftPickStatus.Projected)
                .ToList();
                        
            if (!projections.Any())
                return (new PlayerViewModel(), recommendedRoster.remainingPlayers);
            
            var projectedPlayers = projections
                .Select(r => r.Pick.player)                
                .OrderBy(comparer);
            
            var projectedPlayer = projectedPlayers.FirstOrDefault();

            var remainingPlayers = recommendedRoster
                .remainingPlayers
                .ToList();

            remainingPlayers.AddRange(projectedPlayers.Skip(1));

            return (projectedPlayer, remainingPlayers);                
        }
    }
}