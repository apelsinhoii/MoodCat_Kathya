using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Handlers.CallbackHandlers;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class SettingsHandler : ICallbackHandler
{
    public bool CanHandle(string data) => data == "B";

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, Dictionary<long, string> userMoods, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Налаштування ще в розробці :)", cancellationToken: cancellationToken);
    }
}