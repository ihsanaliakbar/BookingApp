namespace BookingApp.Domain.Entities;

public enum UserRole
{
    User= 0, 
    Admin=1
}

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}