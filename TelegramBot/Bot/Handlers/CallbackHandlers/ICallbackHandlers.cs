using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Data;

namespace TelegramBot.Bot.Handlers.CallbackHandlers
{
    public interface ICallbackHandler
    {
        bool CanHandle(string data);
        Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, string currUserMood, AppDbContext contex, CancellationToken cancellationToken);
    }

}
