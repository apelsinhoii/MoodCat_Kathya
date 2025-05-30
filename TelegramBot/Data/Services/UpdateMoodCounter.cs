using Microsoft.EntityFrameworkCore;
using TelegramBot.Data;

namespace TelegramBot.Services;

public class MoodService
{
    public AppDbContext Db { get; set; }

    public MoodService(AppDbContext db) => Db = db;

    public async Task UpdateMoodCounterAsync(long tgId, string currUserMood)
    {
        var user = await Db.Users.FirstOrDefaultAsync(u => u.TelegramId == tgId);

        if (user is not null)
        {
            switch (currUserMood)
            {
                case "Happy":
                    user.HappyCounter += 1;
                    break;

                case "Sad":
                    user.SadCounter += 1;
                    break;

                case "Angry":
                    user.AngryCounter += 1;
                    break;

                case "Tired":
                    user.TiredCounter += 1;
                    break;

                case "Calm":
                    user.CalmCounter += 1;
                    break;

            }

        }
        await Db.SaveChangesAsync();
        System.Console.WriteLine("Зміни збережено в бд.");
    }
}