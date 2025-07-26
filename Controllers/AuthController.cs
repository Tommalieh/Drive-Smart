using Drivia.Auth;
using Drivia.Services;
using Microsoft.AspNetCore.Mvc;

namespace Drivia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("API is alive!");
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var success = await authService.RegisterAsync(dto);
        if (!success) return BadRequest("User already exists.");
        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var response = await authService.LoginWithRefreshAsync(dto);
        if (response == null) return Unauthorized("Unauthorized credentials.");
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var response = await authService.RefreshTokenAsync(dto.RefreshToken);
        if (response == null) return Unauthorized("Invalid or expired refresh token.");
        return Ok(response);
    }
}