using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure;

public static class DbSeeder
{
    //runs db initialization
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync(); //ensure db schema is up to date (dotnet ef database update)
        await SeedAdmin(db);
        await SeedServices(db);
    }
    
    public static async Task SeedServices(AppDbContext db)
    {
        if (! await db.Services.AnyAsync()) //only seed if no service
        {
            db.Services.AddRange(
                new Service
                {
                    Name = "Cleaning", 
                    DurationMinutes = 60, 
                    PriceCents = 1000, 
                    IsActive = true
                },
                new Service
                {
                    Name = "Laundry", 
                    DurationMinutes = 60, 
                    PriceCents = 1500, 
                    IsActive = true
                },
                new Service
                {
                    Name = "Gardening", 
                    DurationMinutes = 60, 
                    PriceCents = 1200, 
                    IsActive = true
                }
            );
            
            await db.SaveChangesAsync();
        }
    }

    public static async Task SeedAdmin(AppDbContext db)
    {
        var email = "admin@admin.com";
        
        var exists = await db.Users.AnyAsync(u => u.Email == email);
        if (exists) return;

        var admin = new User
        {
            Email = email,
            PasswordHash = PasswordHasher.Hash("admin123!"),
            Role = UserRole.Admin
        };
        
        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}