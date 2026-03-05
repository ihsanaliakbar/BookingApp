using BookingApp.Domain.Entities;
using BookingApp.DTOs;
using BookingApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _db;
        
        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        // Public: show only active services, for booking UI
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ServiceResponse>>> GetActive()
        {
            var items = await _db.Services
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new ServiceResponse(x.Id, x.Name, x.DurationMinutes, x.PriceCents, x.IsActive))
                .ToListAsync();
            
            return Ok(items);
        }

        // Admin: show all services active and inactive, for admin UI
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll()
        {
            var items = await _db.Services
                .OrderBy(x => x.Name)
                .Select(x => new ServiceResponse(x.Id, x.Name, x.DurationMinutes, x.PriceCents, x.IsActive))
                .ToListAsync();
            
            return Ok(items);       
        }

        //admin create new service
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse>> Create(CreateServiceRequest req)
        {
            var name = req.Name.Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (req.DurationMinutes <= 0 || req.DurationMinutes > 480) return BadRequest("Duration must be 1 - 480 minutes");
            if (req.PriceCents <= 0) return BadRequest("Price must be positive");

            var entity = new Service
            {
                Name = name,
                DurationMinutes = req.DurationMinutes,
                PriceCents = req.PriceCents,
            };
            
            _db.Services.Add(entity);
            await _db.SaveChangesAsync();
            
            var res = new ServiceResponse(entity.Id, entity.Name, entity.DurationMinutes, entity.PriceCents, entity.IsActive);
            return CreatedAtAction(nameof(GetByIdAdmin), new {id = entity.Id}, res);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse>> GetByIdAdmin(Guid id)
        {
            var s = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (s is null) return NotFound();
            
            return Ok(new ServiceResponse(s.Id, s.Name, s.DurationMinutes, s.PriceCents, s.IsActive));       
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse>> Update(Guid id, UpdateServiceRequest req)
        {
            var s = await _db.Services.FirstOrDefault(x => x.Id == id);
            if (s is null) return NotFound();
            
            var name = req.Name.Trim();
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required.");
            if (req.DurationMinutes <= 0 || req.DurationMinutes > 480) return BadRequest("Duration must be 1 - 480 minutes.");
            if (req.PriceCents <= 0) return BadRequest("Price must be positive.");
            
            s.Name = name;
            s.DurationMinutes = req.DurationMinutes;
            s.PriceCents = req.PriceCents;
            s.IsActive = req.IsActive;

            _db.SaveChangesAsync();

            return Ok(new ServiceResponse(s.Id, s.Name, s.DurationMinutes, s.PriceCents, s.IsActive));

        }
    }
}
