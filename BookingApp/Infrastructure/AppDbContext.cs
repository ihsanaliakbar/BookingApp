using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    //readonly,get, representation of tables in the database
    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<AvailabilityRule> AvailabilityRules => Set<AvailabilityRule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    //configurations 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Service>()
            .Property(u => u.Name)
            .HasMaxLength(200);

        modelBuilder.Entity<AvailabilityRule>()
            .HasIndex(u => u.DayOfWeek);
        
        modelBuilder.Entity<Appointment>()
            .HasIndex(u => u.StartAtUtc);
        
        //prevent duplicate exact slots (still need overlap check in code)
        modelBuilder.Entity<Appointment>()
            .HasIndex(x => new { x.StartAtUtc, x.EndAtUtc, x.Status });
        
        //run default efcore configurations
        base.OnModelCreating(modelBuilder);
    }
}