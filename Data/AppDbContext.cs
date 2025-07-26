using Drivia.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drivia.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<ChapterProgress> ChapterProgress => Set<ChapterProgress>();
    public DbSet<SectionProgress> SectionProgress => Set<SectionProgress>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuizSession> QuizSessions { get; set; }
    
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

}