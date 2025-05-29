using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Bot.Handlers.CallbackHandlers
{
    public interface ICallbackHandler
    {
        bool CanHandle(string data);
        Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, Dictionary<long, string> userMoods, CancellationToken cancellationToken);
    }

}
