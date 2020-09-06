using System.Collections.Generic;

namespace OnTap.Blazor.Server.Models
{
    public class Draft
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public League League { get; set; }   
        public ICollection<DraftPosition> DraftPositions { get; set; }
        public ICollection<DraftPick> DraftPicks { get; set; }
        public bool IsComplete { get; set; }                                           
    }
}