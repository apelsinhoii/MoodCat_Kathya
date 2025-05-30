using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TelegramBot.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // Use a valid SQLite connection string, e.g., a file-based database
        optionsBuilder.UseSqlite("Data Source=UserInfo.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}