using Microsoft.EntityFrameworkCore;
using DriveSmart.Domain.Entities;
using DriveSmart.Persistence.Data;
namespace DriveSmart.Persistence.Repositories;

public class UserRefreshTokenRepository
{
    private readonly AppDbContext _context;
    public UserRefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserRefreshToken token)
    {
        _context.UserRefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<UserRefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.UserRefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked);
    }

    public async Task RevokeAsync(UserRefreshToken token)
    {
        token.IsRevoked = true;
        await _context.SaveChangesAsync();
    }
}