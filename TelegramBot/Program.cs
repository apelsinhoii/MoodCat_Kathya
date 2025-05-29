using System.Text;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using TelegramBot.Bot.Lib.Keyboards;
using TelegramBot.Bot.Services;




namespace TelegramBot
{

    class Program
    {
        private static string Token { get; set; } = "7685257153:AAE77imIaHX-T5EyBlCKd8G_H71QI9hAKLA";
        private static TelegramBotClient? botClient;
        private static CommandRouter? commandRouter;
        private static Dictionary<long, string> userMoods = new();

        static async Task Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            botClient = new TelegramBotClient(Token);
            commandRouter = new CommandRouter();

            using var cts = new CancellationTokenSource();

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"@{me.Username} запущений... Натисніть Enter, щоб зупинити.");

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                DropPendingUpdates = true
            };

            botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { Text: not null } message)
            {
                if (message.Text == "/start")
                {
                    await bot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:",
                        replyMarkup: Keyboard.MainMenu,
                        cancellationToken: cancellationToken
                    );
                }
            }
            else if (update.CallbackQuery is { Message: not null } callbackQuery)
            {
                var handler = commandRouter?.Route(callbackQuery.Data!);

                if (handler != null)
                {
                    await handler.HandleAsync(bot, callbackQuery, userMoods, cancellationToken);
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
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Помилка: {exception.Message}");
            return Task.CompletedTask;
        }
    }

}