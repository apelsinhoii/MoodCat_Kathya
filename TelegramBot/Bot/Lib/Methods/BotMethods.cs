using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Data;
using TelegramBot.Services;

namespace TelegramBot.Bot.Lib.Methods;

public static class BotMethod
{
    public static void SwitchHistory()///
    {
        throw new NotImplementedException();
    }

    public static void SwitchStatistics()
    {
        throw new NotImplementedException();
    }

    public static async Task ViewStatistics(long tgId, AppDbContext context, ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        var analizer = new StatisticsService(context);
        MoodAnalysisResult analysis = await analizer.AnalyzeUserMoodAsync(tgId);

        string message;
        if (analysis is not null)
        {
            message = $"Найчастіший настрій: {analysis.MostFrequentMood}\n";
            foreach (var mood in analysis.MoodCounters)
            {
                message = message + $"\n{mood.Key}: {mood.Value} ({analysis.MoodRatios[mood.Key]}%)";
            }
            await bot.SendTextMessageAsync(
                     chatId,
                     message,
                     cancellationToken: cancellationToken
                 );
        }
        else
        {
            await bot.SendTextMessageAsync(
                     chatId,
                     "Nothing happened :(",
                     cancellationToken: cancellationToken
                 );
        }
    }

    public static void EndSession()
    {
        throw new NotImplementedException();///
    }

    public static async Task AskNextAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        var nextOptions = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Ще контенту!", "E") },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("\U0001F504 настрій", "C"),
                InlineKeyboardButton.WithCallbackData("\U0001F504тип контенту", "HO")
            },
            new[] { InlineKeyboardButton.WithCallbackData("До головного меню", "F") },
            new[] { InlineKeyboardButton.WithCallbackData("Закрити сесію", "G") }
        });

        await bot.SendTextMessageAsync(chatId, "Що далі?", replyMarkup: nextOptions, cancellationToken: cancellationToken);
    }

    public static void SendContent()
    {
        throw new NotImplementedException();
    }

    public static async Task GenerateContent(ITelegramBotClient bot, long chatId, string contentType, string currUserMood, CancellationToken cancellationToken)
    {
        var animeRecommendations = new Dictionary<string, List<string>>
        {
            ["HO"] = new List<string> {
            "Несолодке життя псионіка Сайкі Кусуо — https://nekoteka.com/anime/nesolodke-zhyttya-psyonika-sayki-kusuo",
            "Шпигун та сім'я — https://nekoteka.com/anime/shpyhun-ta-simya",
            "Скейт: Нескінченність — https://nekoteka.com/anime/skeyt-neskinchennist"
        },
            ["SO"] = new List<string> {
            "Атака Титанів: Фінальний сезон — https://nekoteka.com/anime/ataka-tytaniv-finalnyy-sezon-2-chastyna",
            "Євангеліон 3.0+1.0 — https://nekoteka.com/anime/yevanhelion-3010-odnoho-razu",
            "Рибка-бананка — https://nekoteka.com/anime/rybka-bananka"
        },
            ["AO"] = new List<string> {
            "Сталевий алхімік: Братерство — https://nekoteka.com/anime/stalevyy-alkhimik-braterstvo",
            "Мисливець х Мисливець — https://nekoteka.com/anime/myslyvets-kh-myslyvets-2011",
            "Клинок, який знищує демонів — https://nekoteka.com/anime/klynok-yakyy-znyshchuye-demoniv-kvartal-rozvah"
        },
            ["TO"] = new List<string> {
            "Форма голосу — https://nekoteka.com/anime/forma-holosu",
            "Вайолет Еверґарден — https://nekoteka.com/anime/vayolet-evergarden",
            "Жозе, тигр і риба — https://nekoteka.com/anime/zhoze-tyhr-i-ryba"
        },
            ["CO"] = new List<string> {
            "K-ON!: The Movie — https://nekoteka.com/anime/k-movie",
            "Ґівен — https://nekoteka.com/anime/given",
            "Doukyuusei -Classmates- — https://nekoteka.com/anime/doukyuusei-classmates"
        }
        };

        var filmRecommendations = new Dictionary<string, List<string>>
        {
            ["HO"] = new List<string> {
            "Пес із нами / Суперпес — https://uakino.me/filmy/genre-action/19541-pes-z-nami-superpes.html",
            "К-9: Приватні детективи — https://uakino.me/filmy/genre-action/6574-sobacha-robota-3.html",
            "Minecraft: Фільм — https://uakino.me/filmy/genre-action/27135-minecraft-film.html"
        },
            ["SO"] = new List<string> {
            "Світ здригнеться — https://uakino.me/filmy/genre_drama/27796-svit-zdrygnetsia.html",
            "Королі літа — https://uakino.me/filmy/genre_comedy/27666-koroli-lita.html",
            "Зла не існує — https://uakino.me/filmy/genre_drama/24922-zla-ne-isnuie.html"
        },
            ["AO"] = new List<string> {
            "Я воїн — https://uakino.me/filmy/genre-action/27799-ia-voin.html",
            "Ідеальний хижак — https://uakino.me/filmy/genre-action/27291-idealnyi-khyzhak.html",
            "Палка пристрасть — https://uakino.me/filmy/genre-action/27075-palka-prystrast-zhyvy-na-povnu.html"
        },
            ["TO"] = new List<string> {
            "Все про мого собаку — https://uakino.me/filmy/genre_drama/27591-vse-pro-mogo-sobaku.html",
            "Звільніть Віллі — https://uakino.me/filmy/genre_adventure/3483-zvlnt-vll.html",
            "Хачіко / Історія Хачіко — https://uakino.me/filmy/genre_drama/26552-khachiko-istoriia-khachiko.html"
        },
            ["CO"] = new List<string> {
            "Незакінчене життя — https://uakino.me/filmy/genre_drama/27769-nezakinchene-zhyttia.html",
            "Операція Літаючий Слон — https://uakino.me/filmy/genre-action/8107-operacya-ltayuchiy-slon.html",
            "Друга книга Джунглів — https://uakino.me/filmy/genre_comedy/23932-druga-knyga-dzhungliv-maugli-i-balu.html"
        }
        };

        var rand = new Random();
        string recommendation;

        if (contentType == "anime" && animeRecommendations.ContainsKey(currUserMood))
        {
            var list = animeRecommendations[currUserMood];
            recommendation = list[rand.Next(list.Count)];
        }
        else if (contentType == "movies" && filmRecommendations.ContainsKey(currUserMood))
        {
            var list = filmRecommendations[currUserMood];
            recommendation = list[rand.Next(list.Count)];
        }
        else
        {
            recommendation = "Упс, щось пішло не так... Можливо, тип контенту або настрій не підтримується.";
        }

        await bot.SendTextMessageAsync(chatId, recommendation, cancellationToken: cancellationToken);
    }

}
