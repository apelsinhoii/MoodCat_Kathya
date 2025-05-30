using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Lib.Keyboards;
using TelegramBot.Data;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class MainMenuHandler : ICallbackHandler
{
    public bool CanHandle(string data) => data == "F";

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, string currUserMood,AppDbContext context, CancellationToken cancellationToken)
    {
        await bot.SendTextMessageAsync(query.Message.Chat.Id, "Ти у головному меню:", replyMarkup: Keyboard.MainMenu, cancellationToken: cancellationToken);
    }
}
