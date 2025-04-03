using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Test
{
    class Program
    {
        private static string Token {get; set;} = "7685257153:AAE77imIaHX-T5EyBlCKd8G_H71QI9hAKLA"; 
        private static TelegramBotClient botClient;

        static async Task Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            botClient = new TelegramBotClient(Token);
            using var cts = new CancellationTokenSource();

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"@{me.Username} Запущений... Натисніть Enter щоб зупинити.");

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
        if (update.Message is { } message)
        {
            
            if (message.Text == "/start")
            {
                var keyboard = new InlineKeyboardMarkup(new[]
                {
                       new[]  {InlineKeyboardButton.WithCallbackData("Аctivity", "A"),},
                       new[]  {InlineKeyboardButton.WithCallbackData("Settings", "B")},
                       new[]  {InlineKeyboardButton.WithCallbackData("ChooseMood", "C")}
                });

                await bot.SendTextMessageAsync(message.Chat.Id, "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:", replyMarkup: keyboard, cancellationToken: cancellationToken);
            }
        }
        
        else if (update.CallbackQuery is { } callbackQuery)
        {
            switch (update.CallbackQuery.Data)
            {
                  case "A":
                 var keyboardA = new InlineKeyboardMarkup(new[]
                 {
                    new[]  {InlineKeyboardButton.WithCallbackData("Activity", "AA"),}, 
                 });
                
                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Переглянути статистику", replyMarkup: keyboardA, cancellationToken: cancellationToken);
                
                break;
 
                case "AA":
                
                await bot.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Статистика настрою користувача за тиждень.", cancellationToken: cancellationToken);
                
                break;

                default:              
                
                await bot.SendMessage(callbackQuery.Message.Chat.Id, "Щось пішло не так...", cancellationToken: cancellationToken);
                
                break;

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
