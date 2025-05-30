using Microsoft.EntityFrameworkCore;
namespace TelegramBot.Data;

using Models;


public class AppDbContext : DbContext
{
    public DbSet<Person> Users => Set<Person>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>()
            .HasIndex(u => u.TelegramId)
            .IsUnique();
    }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=UserInfo.db");
        }
    }
}

