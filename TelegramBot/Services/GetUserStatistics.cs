using Microsoft.EntityFrameworkCore;
using TelegramBot.Data;


namespace TelegramBot.Services;

   public class MoodAnalysisResult
{
    public string MostFrequentMood { get; set; } = "";
    public Dictionary<string, int> MoodCounters { get; set; } = new();
    public Dictionary<string, double> MoodRatios { get; set; } = new();
}

public class StatisticsService
{
    private readonly AppDbContext _db;

    public StatisticsService(AppDbContext db) => _db = db;

    public async Task<MoodAnalysisResult?> AnalyzeUserMoodAsync(long tgId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.TelegramId == tgId);
        if (user is null) return null;

        // Збираємо дані
        var counters = new Dictionary<string, int>
        {
            ["Щасливий"] = user.HappyCounter,
            ["Сумний"] = user.SadCounter,
            ["Злий"] = user.AngryCounter,
            ["Втомлений"] = user.TiredCounter,
            ["Спокійний"] = user.CalmCounter
        };

        // Визначаємо найчастіший настрій
        string mostFrequent = counters
            .OrderByDescending(c => c.Value)
            .First().Key;

        // Обчислюємо загальну кількість виборів
        int total = counters.Values.Sum();
        var ratios = counters.ToDictionary(
            pair => pair.Key,
            pair => total == 0 ? 0 : Math.Round((double)pair.Value / total * 100, 2)
        );

        return new MoodAnalysisResult
        {
            MostFrequentMood = mostFrequent,
            MoodCounters = counters,
            MoodRatios = ratios
        };
    }
}

