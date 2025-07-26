using Drivia.Data;
using Drivia.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drivia.Repositories;

public class UserRepository(AppDbContext context)
{
    public Task<User?> GetByEmailAsync(string email) =>
        context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    } 
}