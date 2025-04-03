using System;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace test
{
    class Program
    {
        private static string Token { get; set; } = "7685257153:AAE77imIaHX-T5EyBlCKd8G_H71QI9hAKLA"; //private static readonly string Token = "7685257153:AAE77imIaHX-T5EyBlCKd8G_H71QI9hAKLA";
        private static TelegramBotClient? botClient;
        private static object? contentKey;

        static async Task Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            botClient = new TelegramBotClient(Token);
            using var cts = new CancellationTokenSource();

            var me = await botClient.GetMe();
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

        /* private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
         {
             if (update.Message is { } message)
             {
                 Console.WriteLine($"Отримано повідомлення: {message.Text}");
                 await botClient.SendTextMessageAsync(message.Chat.Id, "Привіт! Це тестовий бот.", cancellationToken: cancellationToken);
             }
         }

         private static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
         {
             Console.WriteLine($"Помилка: {exception.Message}");
             return Task.CompletedTask;
         }*/

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { Text: not null } message)
            {

                if (message.Text == "/start")
                {


                    await bot.SendMessage(message.Chat.Id, "Доброго дня! Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
                }
            }
            else if (update.CallbackQuery is { Message: not null } callbackQuery)
            {
                long chatId = callbackQuery.Message.Chat.Id;
                /* string response = callbackQuery.Data switch
                 {
                     "A" => "Ви вибрали А! Це чудовий вибір!",
                     "B" => "Варіант Б — це цікавий вибір!",
                     "C" => "Ви натиснули В! Дякую за вибір!",
                     _ => "Щось пішло не так..."
                 };

                 await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, response, cancellationToken: cancellationToken);*/
                switch (callbackQuery.Data)
                {

                    case "A":
                        break;

                    case "B":
                        var keyboard = new InlineKeyboardMarkup(
                        [

                            [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути збір статистики", "BA")],
                    [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути історію", "BB")],

                ]);

                        await bot.SendMessage(chatId, "Відкрито налаштування. Виберіть опцію:", replyMarkup: keyboard, cancellationToken: cancellationToken);
                        break;

                    case "BA":
                        try
                        {
                            BotMethod.SwitchStatistics();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Функція в розробці...", cancellationToken: cancellationToken);
                        await bot.SendMessage(callbackQuery.Message.Chat.Id, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);

                        }
                        break;

                    case "BB":
                        try
                        {
                            BotMethod.SwitchHistory();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Функція в розробці...", cancellationToken: cancellationToken);
                        await bot.SendMessage(callbackQuery.Message.Chat.Id, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);

                        }

                        break;

                    case "C":
                        var testD = new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("D", "D") });
                        await bot.SendMessage(chatId, "D", replyMarkup: testD, cancellationToken: cancellationToken);

                        break;

                    case "D":
                        var testE = new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("E", "E") });
                        await bot.SendMessage(chatId, "E", replyMarkup: testE, cancellationToken: cancellationToken);

                        break;

                    case "E":
                        try
                        {
                            BotMethod.GenerateContent(contentKey);

                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Функція в розробці...", cancellationToken: cancellationToken);
                            var nextOptions = new InlineKeyboardMarkup(
                       [
                           [InlineKeyboardButton.WithCallbackData("Ще контенту!", "E"),],
                    [InlineKeyboardButton.WithCallbackData("\U0001F504 настрій", "C"), InlineKeyboardButton.WithCallbackData("\U0001F504тип контенту", "D")],
                    [InlineKeyboardButton.WithCallbackData("До головного меню", "F"),],
                    [InlineKeyboardButton.WithCallbackData("Скінчити сесію", "G"),]

                       ]);
                            await bot.SendMessage(callbackQuery.Message.Chat.Id, "Що далі?", replyMarkup: nextOptions, cancellationToken: cancellationToken);
                        }

                        break;

                    case "F":
                        await bot.SendMessage(callbackQuery.Message.Chat.Id, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);

                        break;

                    case "G":
                        try
                        {
                            BotMethod.EndSession();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Функція в розробці...", cancellationToken: cancellationToken);
                        }
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
