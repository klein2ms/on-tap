namespace OnTap.Blazor.Shared
{
    public class DraftPositionViewModel : ValueObject<DraftPositionViewModel>
    {
        public int Id { get; set; }
        public int DraftId { get; set; }
        public TeamViewModel Team { get; set; }
        public int Number { get; set; }
    }
}