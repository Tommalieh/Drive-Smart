using DriveSmart.Domain.Entities;
using DriveSmart.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace DriveSmart.Persistence.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    } 
}