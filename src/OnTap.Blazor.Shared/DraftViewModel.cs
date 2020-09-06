using System.Collections.Generic;

namespace OnTap.Blazor.Shared
{
    public class DraftViewModel : ValueObject<DraftViewModel>
    {
        public int Id { get; set; }
        public LeagueViewModel League { get; set; } = new LeagueViewModel();
        public IEnumerable<DraftPositionViewModel> DraftPositions { get; set; } = new List<DraftPositionViewModel>();
        public IEnumerable<DraftPickViewModel> DraftPicks { get; set; } = new List<DraftPickViewModel>();
        public bool IsComplete { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return League;
            foreach (var draftPosition in DraftPositions)            
                yield return draftPosition; 
            foreach (var draftPick in DraftPicks)            
                yield return draftPick;
            yield return IsComplete;            
        }
    }
}