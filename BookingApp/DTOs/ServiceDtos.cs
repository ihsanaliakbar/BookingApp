namespace BookingApp.DTOs;

public record ServiceResponse(Guid Id, string Name, int DurationMinutes, int? PriceCents, bool IsActive);

public record CreateServiceRequest(string Name, int DurationMinutes, int? PriceCents);

public record UpdateServiceRequest(string Name, int DurationMinutes, int? PriceCents, bool IsActive);
