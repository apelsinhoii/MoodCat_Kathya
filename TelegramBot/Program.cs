using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Bot.Lib.Methods;
using TelegramBot.Bot.Lib.Keyboards;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Бот працює через polling!"); // Railway пінгуватиме цей endpoint

string? token = Environment.GetEnvironmentVariable("TOKEN");
if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("[ERROR] Не вдалося отримати токен із змінної середовища 'TOKEN'.");
    return;
}

var botClient = new TelegramBotClient(token);
var cts = new CancellationTokenSource();

var userMoods = new Dictionary<long, string>();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>(),
    DropPendingUpdates = true
};

botClient.StartReceiving(
    async (bot, update, cancellationToken) =>
    {
        if (update.Message is { Text: not null } message)
        {
            if (message.Text == "/start")
            {
                await bot.SendMessage(message.Chat.Id, "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
            }
        }
        else if (update.CallbackQuery is { Message: not null } callbackQuery)
        {
            long chatId = callbackQuery.Message.Chat.Id;

            switch (callbackQuery.Data)
            {
                case "A":
                    try { BotMethod.ViewStatistics(); }
                    catch { }
                    finally
                    {
                        await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…", cancellationToken: cancellationToken);
                        await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
                    }
                    break;

                case "B":
                    var keyboard = new InlineKeyboardMarkup(
                        [
                            [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути збір статистики", "BA")],
                            [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути історію", "BB")],
                        ]);
                    await bot.SendMessage(chatId, "Налаштування відкрито! Обирай, що змінити:", replyMarkup: keyboard, cancellationToken: cancellationToken);
                    break;

                case "BA":
                    try { BotMethod.SwitchStatistics(); }
                    catch { }
                    finally
                    {
                        await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…", cancellationToken: cancellationToken);
                        await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
                    }
                    break;

                case "BB":
                    try { BotMethod.SwitchHistory(); }
                    catch { }
                    finally
                    {
                        await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…", cancellationToken: cancellationToken);
                        await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
                    }
                    break;

                case "C":
                    var moodKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("Веселий", "HO") },
                        new[] { InlineKeyboardButton.WithCallbackData("Сумний", "SO") },
                        new[] { InlineKeyboardButton.WithCallbackData("Злий", "AO") },
                        new[] { InlineKeyboardButton.WithCallbackData("Виснажений", "TO") },
                        new[] { InlineKeyboardButton.WithCallbackData("Спокійний", "CO") }
                    });
                    await bot.SendMessage(chatId, "Обери свій кото-настрій на сьогодні!:", replyMarkup: moodKeyboard, cancellationToken: cancellationToken);
                    break;

                case "HO":
                case "SO":
                case "AO":
                case "TO":
                case "CO":
                    userMoods[chatId] = callbackQuery.Data;
                    var contentKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("Фільми", "MC") },
                        new[] { InlineKeyboardButton.WithCallbackData("Аніме", "AC") },
                        new[] { InlineKeyboardButton.WithCallbackData("Фото", "PC") }
                    });
                    await bot.SendMessage(chatId, "Ваш настрій зафіксовано! Що бажаєте переглянути?", replyMarkup: contentKeyboard, cancellationToken: cancellationToken);
                    break;

                case "MC":
                    await BotMethod.GenerateContent(bot, chatId, "movies", userMoods, cancellationToken);
                    await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                    break;

                case "AC":
                    await BotMethod.GenerateContent(bot, chatId, "anime", userMoods, cancellationToken);
                    await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                    break;

                case "PC":
                    await BotMethod.GenerateContent(bot, chatId, "photos", userMoods, cancellationToken);
                    await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                    break;

                case "E":
                    try { BotMethod.SendContent(); }
                    catch { }
                    finally
                    {
                        await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…", cancellationToken: cancellationToken);
                        await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                    }
                    break;

                case "F":
                    await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
                    break;

                case "G":
                    try { BotMethod.EndSession(); }
                    catch { }
                    finally
                    {
                        await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…", cancellationToken: cancellationToken);
                    }
                    break;

                default:
                    await bot.SendMessage(chatId, "Ой-Ой! Щось пішло не так...", cancellationToken: cancellationToken);
                    break;
            }
        }
    },
    (bot, ex, ct) =>
    {
        Console.WriteLine($"[ERR] {ex.Message}");
        return Task.CompletedTask;
    },
    receiverOptions,
    cts.Token
);

Console.WriteLine("[INFO] Бот запущено на polling!");
await app.RunAsync();
