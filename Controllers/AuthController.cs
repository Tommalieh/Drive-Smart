using DriveSmart.Application.Services;
using DriveSmart.Shared.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DriveSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("API is alive!");
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var success = await _authService.RegisterAsync(dto);
        if (!success) return BadRequest("User already exists.");
        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var response = await _authService.LoginWithRefreshAsync(dto);
        if (response == null) return Unauthorized("Unauthorized credentials.");
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var response = await _authService.RefreshTokenAsync(dto.RefreshToken);
        if (response == null) return Unauthorized("Invalid or expired refresh token.");
        return Ok(response);
    }
}