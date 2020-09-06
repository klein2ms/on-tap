using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Client.Shared
{
    public class DraftRankingsBase : ComponentBase
    {
        [Parameter] public PlayerViewModel[] Players { get; set; }

        [Parameter] public Action<PlayerViewModel> OnDraft { get; set; }

        public bool IsSortMenuOpen { get; set; } = false;
        public string SortMenuClass => IsSortMenuOpen ? "is-active" : string.Empty;

        public List<PlayerViewModel> FilteredPlayers =>
            Players
            .Where(p =>
                p.FullName.ToLower().Contains(PlayerName.ToLower())
                && PositionFilters.Contains(p.Position)
                && !p.IsDrafted
                && p.Adp != 0)
            .OrderBy(OrderBy)
            .Take(20)
            .ToList();

        public string PlayerName { get; set; } = string.Empty;
        public List<Position> PositionFilters { get; set; } = GetAllPositions();
        public IComparer<PlayerViewModel> OrderBy { get; set; } = DraftPickRecommender.AdpComparer;

        private static List<Position> GetAllPositions() => new List<Position>
            {
                Position.Quarterback,
                Position.RunningBack,
                Position.WideReceiver,
                Position.TightEnd,
                Position.Defense,
                Position.Kicker
            };

        protected void ShowAll() => PositionFilters = GetAllPositions();

        protected void Filter(Position position) => PositionFilters = new List<Position> { position };

        protected void Sort(IComparer<PlayerViewModel> comparer) => OrderBy = comparer;        
    }
}