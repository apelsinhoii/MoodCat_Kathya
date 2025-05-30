using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Bot.Lib.Keyboards;
using TelegramBot.Services;
using TelegramBot.Data;
using TelegramBot.Bot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.MapGet("/", () => "MoodCat працює через polling!");

string? token = Environment.GetEnvironmentVariable("TOKEN");

if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("[ERROR] Не вдалося отримати токен із змінної середовища 'TOKEN'.");
    return;
}

var botClient = new TelegramBotClient(token);
var commandRouter = new CommandRouter();
var cts = new CancellationTokenSource();
var userMoods = new Dictionary<long, string>();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>(),
    DropPendingUpdates = true
};

var genContext = new AppDbContextFactory();
var dbContext = genContext.CreateDbContext(args);
await dbContext.Database.EnsureCreatedAsync();

botClient.StartReceiving(
    async (bot, update, cancellationToken) =>
    {
        if (update.Message is { Text: not null } message)
        {
            if (message.Text == "/start")
            {
                var user = message.From;

                UserService service = new(dbContext);
                await service.RegisterUserAsync(user!.Id, user!.FirstName);
                await bot.SendMessage(message.Chat.Id, "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
            }
        }
        else if (update.CallbackQuery is { Message: not null } callbackQuery)
        {
            var handler = commandRouter.Route(callbackQuery.Data!);

            if (handler != null)
            {
                userMoods[callbackQuery.Message.Chat.Id] = callbackQuery.Data!;
                await handler.HandleAsync(bot, callbackQuery, callbackQuery.Data!, dbContext, cancellationToken);
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Ой-ой! Я не знаю, як це обробити.",
                    cancellationToken: cancellationToken
                );
            }
        }
    },
    (bot, ex, ct) =>
    {
        Console.WriteLine($"[ERROR] {ex.Message}");
        return Task.CompletedTask;
    },
    receiverOptions,
    cts.Token
);

await app.RunAsync();




