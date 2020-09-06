namespace OnTap.Blazor.Shared
{
    public class StatisticViewModel : ValueObject<StatisticViewModel>
    {
        public Statistic Stat { get; set; }
        public decimal Value { get; set; }
    }
}