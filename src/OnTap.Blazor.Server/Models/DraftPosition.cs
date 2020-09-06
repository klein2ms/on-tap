namespace OnTap.Blazor.Server.Models
{
    public class DraftPosition
    {
        public int Id { get; set; }
        public int DraftId { get; set; }
        public Draft Draft { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int Number { get; set; }
    }
}