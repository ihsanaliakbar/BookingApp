using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure;

public static class DbSeeder
{
    //runs db initialization
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync(); //ensure db schema is up to date (dotnet ef database update)

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
}