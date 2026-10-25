using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rasket.Api.Data;
using Rasket.Api.DTOs;
using Rasket.Api.Models;
using Rasket.Api.Services;

namespace Rasket.Api.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email or password is required" });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);
        if (user == null)
            return BadRequest(new { message = "Email or password is incorrect" });

        var token = _tokenService.GenerateToken(user);

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Name = $"{user.FirstName} {user.LastName}",
            Avatar = user.Avatar ?? "assets/images/users/user-1.jpg",
            Location = user.Location ?? "California, USA",
            Title = user.Title ?? "Administrator",
            Token = token
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequestDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new { message = "User Already Exists" });

        var names = request.Name.Split(' ');
        var firstName = names.Length > 0 ? names[0] : request.Name;
        var lastName = names.Length > 1 ? names[1] : "";

        var newUser = new User
        {
            Username = firstName,
            Email = request.Email,
            PasswordHash = request.Password,
            FirstName = firstName,
            LastName = lastName,
            Avatar = "assets/images/users/avatar-5.jpg",
            Location = "California, USA",
            Title = "Member"
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }
}
