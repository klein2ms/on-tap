using System;

namespace OnTap.Blazor.Shared
{
    public class DraftPickViewModel : ValueObject<DraftPickViewModel>
    {        
        public int Id { get; set; }
        public int DraftId { get; set; }
        public int Number { get; set; }
        public PlayerViewModel Player { get; set; } = new PlayerViewModel();
        public TeamViewModel Team { get; set; } = new TeamViewModel();       
    }

    public static class DraftPickViewModelExtensions
    {
        public static DraftPickViewModel DraftPlayer(
            this DraftPickViewModel draftPick, 
            PlayerViewModel player)
        {
            draftPick.Player = player ?? throw new ArgumentNullException(nameof(player));
            player.IsDrafted = true;
            draftPick.Team.Roster.FillRosterSpot(player);

            return draftPick;
        }
    }
}