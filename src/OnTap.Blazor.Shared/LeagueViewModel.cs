using System.Collections.Generic;

namespace OnTap.Blazor.Shared
{
    public class LeagueViewModel : ValueObject<LeagueViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LeagueSettingsViewModel LeagueSettings { get; set; } = new LeagueSettingsViewModel();
        public IEnumerable<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
            yield return LeagueSettings;
            foreach (var team in Teams)            
                yield return team;            
        }
    }
}