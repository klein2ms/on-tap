using System.Collections.Generic;

namespace OnTap.Blazor.Server.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LeagueSettings LeagueSettings { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Draft> Drafts { get; set; }
    }
}