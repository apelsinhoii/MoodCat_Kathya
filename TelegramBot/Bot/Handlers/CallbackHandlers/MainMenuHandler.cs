using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Handlers.CallbackHandlers;
using TelegramBot.Bot.Lib.Keyboards;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class MainMenuHandler : ICallbackHandler
{
    public bool CanHandle(string data) => data == "F";

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, Dictionary<long, string> userMoods, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Ти у головному меню:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
    }
}
