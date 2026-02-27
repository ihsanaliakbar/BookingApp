using BookingApp.Domain.Entities;
using BookingApp.DTOs;
using BookingApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwt;
        
        public AuthController(AppDbContext db, JwtTokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
        {
            //check email pw valid
            var email = req.Email.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
            {
                return BadRequest("Email and password are required");
            }

            if (req.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters long");
            }
            
            var exist = await _db.Users.AnyAsync(x => x.Email == email);
            if (exist)
            {
                return Conflict("Email already registered");
            }
            
            //create user
            var user = new User
            {
                Email = email,
                PasswordHash = PasswordHasher.Hash(req.Password),
                Role = UserRole.User
            };
            
            //save user to db
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _jwt.CreateToken(user);
            return Ok(new AuthResponse(token, user.Email, user.Role.ToString()));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
        {
            var email = req.Email.Trim().ToLowerInvariant();
            
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                return Unauthorized("Invalid credentials");
            }

            var ok = PasswordHasher.Verify(req.Password, user.PasswordHash);
            if (!ok)
            {
                return Unauthorized("Invalid credentials");
            }
            
            var token = _jwt.CreateToken(user);
            return Ok(new AuthResponse(token, user.Email, user.Role.ToString()));
        }
    }
}
