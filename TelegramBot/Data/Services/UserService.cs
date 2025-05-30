using Microsoft.EntityFrameworkCore;
using TelegramBot.Data;


namespace TelegramBot.Services;
using Data.Models;

public class UserService
{
    public AppDbContext Db { get; set; }

    public UserService(AppDbContext db) => Db = db;

    public async Task RegisterUserAsync(long tgId, string? username)
    {
        var user = await Db.Users.FirstOrDefaultAsync(u => u.TelegramId == tgId);
        if (user is null)
        {
            Db.Users.Add(new Person(){ TelegramId = tgId, Username = username ?? "unknown" });
            await Db.SaveChangesAsync();
        }
    }
}
