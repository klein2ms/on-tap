using System.Collections.Generic;

namespace OnTap.Blazor.Shared
{
    public class RosterSpot : ValueObject<RosterSpot>
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Position> PossiblePositions { get; set; } = new List<Position>();
        public bool IsStarter { get; set; }
        public DraftPickStatus Status { get; set; } = DraftPickStatus.Empty;
        public PlayerViewModel Player { get; set; } = new PlayerViewModel();        
        public (DraftPickStatus status, PlayerViewModel player) Pick => (Status, Player);

        protected override IEnumerable<object> GetEqualityComponents()
        {            
            yield return Name;
            yield return IsStarter;
            yield return Pick;
            foreach (var position in PossiblePositions)
                yield return position;         
        }
    }
}