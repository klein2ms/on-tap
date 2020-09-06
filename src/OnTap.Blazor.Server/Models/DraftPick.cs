namespace OnTap.Blazor.Server.Models
{
    public class DraftPick
    {
        public int Id { get; set; }
        public int DraftId { get; set; }        
        public Draft Draft { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int Number { get; set; }                                                      
    }
}