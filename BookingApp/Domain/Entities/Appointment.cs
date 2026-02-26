namespace BookingApp.Domain.Entities;

public enum AppointmentStatus
{
    Booked = 0,
    Cancelled = 1
}

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    
    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }
    
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}