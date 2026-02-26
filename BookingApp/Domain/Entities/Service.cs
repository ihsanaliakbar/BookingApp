namespace BookingApp.Domain.Entities;

public class Service
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int Duration { get; set; }
    public int? PriceCents { get; set; }
    public bool IsActive { get; set; } = true;
}