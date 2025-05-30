using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Handlers.CallbackHandlers;
using TelegramBot.Data;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class SettingsHandler : ICallbackHandler
{
    public bool CanHandle(string data) => data == "B";

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, string currUserMood, AppDbContext context, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Налаштування ще в розробці :)", cancellationToken: cancellationToken);
    }
}