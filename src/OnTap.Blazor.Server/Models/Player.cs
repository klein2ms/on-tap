using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public string TeamShortName { get; set; }
        public int ByeWeek { get; set; }
        public int Best { get; set; }
        public int Worst { get; set; }
        public decimal Adp { get; set; }
        public decimal? PassingAttempts { get; set; }
        public decimal? PassingCompletions { get; set; }
        public decimal? PassingYards { get; set; }
        public decimal? PassingTouchdowns { get; set; }
        public decimal? PassingInterceptions { get; set; }
        public decimal? RushingAttempts { get; set; }
        public decimal? RushingYards { get; set; }
        public decimal? RushingTouchdowns { get; set; }
        public decimal? Receptions { get; set; }
        public decimal? ReceivingYards { get; set; }
        public decimal? ReceivingTouchdowns { get; set; }
        public decimal? FumblesLost { get; set; }
        public decimal? FieldGoals { get; set; }
        public decimal? FieldGoalsAttempted { get; set; }
        public decimal? FieldGoalsMissed { get; set; }
        public decimal? ExtraPoints { get; set; }
        public decimal? Sacks { get; set; }
        public decimal? Interceptions { get; set; }
        public decimal? FumblesRecovered { get; set; }
        public decimal? ForcedFumbles { get; set; }
        public decimal? DefensiveTouchdowns { get; set; }
        public decimal? Safeties { get; set; }
        public decimal? PointsAgainst { get; set; }
        public decimal? YardsAgainst { get; set; }
    }
}