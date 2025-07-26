using Drivia.Data;
using Drivia.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drivia.Repositories;

public class UserRefreshTokenRepository(AppDbContext context)
{
    public async Task AddAsync(UserRefreshToken token)
    {
        context.UserRefreshTokens.Add(token);
        await context.SaveChangesAsync();
    }

    public async Task<UserRefreshToken?> GetByTokenAsync(string token)
    {
        return await context.UserRefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked);
    }

    public async Task RevokeAsync(UserRefreshToken token)
    {
        token.IsRevoked = true;
        await context.SaveChangesAsync();
    }
}