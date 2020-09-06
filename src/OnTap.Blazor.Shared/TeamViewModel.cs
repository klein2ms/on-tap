using System;
using System.Linq;

namespace OnTap.Blazor.Shared
{
    public class TeamViewModel : ValueObject<TeamViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;        
        public OwnerViewModel Owner { get; set; } = new OwnerViewModel();
        public Roster Roster { get; set; } = new Roster();
        public int DraftPosition {get ; set; }
        public bool IsDrafting { get; set; }
    }

    public static class TeamViewModelExtensions
    {
        public static TeamViewModel SeedRoster(
            this TeamViewModel team, 
            LeagueSettingsViewModel settings)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var rosterSpots = settings.GetRosterSpots();

            team.Roster.AddRosterSpots(rosterSpots.ToList());            

            return team;
        }        
    }
}