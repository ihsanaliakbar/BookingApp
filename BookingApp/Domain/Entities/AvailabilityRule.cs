namespace BookingApp.Domain.Entities;

public class AvailabilityRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int DayOfWeek { get; set; } //Sunday = 0, Monday = 1, ...
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}