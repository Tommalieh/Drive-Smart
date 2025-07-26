using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DriveSmart.Domain.Entities;
using DriveSmart.Persistence.Repositories;
using DriveSmart.Shared.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DriveSmart.Application.Services;

public class AuthService
{
    private readonly UserRepository _userRepo;
    private readonly UserRefreshTokenRepository _tokenRepo;
    private readonly IConfiguration _config;

    public AuthService(
        UserRepository userRepo,
        UserRefreshTokenRepository tokenRepo,
        IConfiguration config)
    {
        _userRepo = userRepo;
        _tokenRepo = tokenRepo;
        _config = config;
    }

    public async Task<AuthResponseDto?> LoginWithRefreshAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;
        
        var accessToken = GenerateJwt(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var userRefreshToken = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = expiresAt,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            User = user
        };
        await _tokenRepo.AddAsync(userRefreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepo.GetByEmailAsync(dto.Email) != null)
            return false;

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        await _userRepo.AddAsync(user);
        return true;
    }
    
    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _tokenRepo.GetByTokenAsync(refreshToken);

        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            return null;

        var user = storedToken.User;
        if (user == null) return null;

        // Revoke old token and generate new one (rotation)
        await _tokenRepo.RevokeAsync(storedToken);

        var newRefreshToken = GenerateRefreshToken();
        var newToken = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            User = user
        };
        await _tokenRepo.AddAsync(newToken);

        var accessToken = GenerateJwt(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),     // Used by your backend to identify the user
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),   // Optional: aligns with JWT standards
            new Claim(JwtRegisteredClaimNames.Email, user.Email),         // ✅ Optional: allows displaying or using email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Standard for unique token ID
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string GenerateRefreshToken()
    {
        var random = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(random);
        return Convert.ToBase64String(random);
    }

}
    