using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Client.Shared
{
    public class RostersBase : ComponentBase
    {
        [Parameter] public TeamViewModel Team { get; set; }

        public List<RosterSpot> Starters => Team.Roster.RosterSpots.Where(r => r.IsStarter).ToList();

        public List<RosterSpot> Bench => Team.Roster.RosterSpots.Where(r => !r.IsStarter).ToList();
    }
}