using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Services;

namespace TelegramBot.Bot.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly CommandRouter router;

        public CallbackQueryHandler()
        {
            router = new CommandRouter();
        }

        public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, Dictionary<long, string> userMoods, CancellationToken cancellationToken)
        {
            var handler = router.Route(query.Data);

            if (handler != null)
            {
                await handler.HandleAsync(bot, query, userMoods, cancellationToken);
            }
            else
            {
                await bot.SendTextMessageAsync(query.Message.Chat.Id, "Невідома команда.", cancellationToken: cancellationToken);
            }
        }
    }
}
