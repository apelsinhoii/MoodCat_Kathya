using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Bot.Services;
using TelegramBot.Data;

namespace TelegramBot.Bot.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly CommandRouter router;

        public CallbackQueryHandler()
        {
            router = new CommandRouter();
        }

        public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, string currUserMood, AppDbContext context, CancellationToken cancellationToken)
        {
            var handler = router.Route(query.Data);

            if (handler != null)
            {
                await handler.HandleAsync(bot, query, currUserMood, context, cancellationToken);
            }
            else
            {
                await bot.SendTextMessageAsync(query.Message.Chat.Id, "Невідома команда.", cancellationToken: cancellationToken);
            }
        }
    }
}
