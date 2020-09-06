namespace OnTap.Blazor.Shared
{
    public class OwnerViewModel : ValueObject<OwnerViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}