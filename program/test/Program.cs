
using System;
using lib;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

namespace test
{
    class Program
    {
        private static string Token { get; set; } = "7685257153:AAE77imIaHX-T5EyBlCKd8G_H71QI9hAKLA";
        private static TelegramBotClient? botClient;
        private static object? contentKey;
        private static Dictionary<long, string> userMoods = new();


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

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { Text: not null } message)
            {
                if (message.Text == "/start")
                {
                    await bot.SendMessage(message.Chat.Id, "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
                }
            }
            else if (update.CallbackQuery is { Message: not null } callbackQuery)
            {
                long chatId = callbackQuery.Message.Chat.Id;

                switch (callbackQuery.Data)
                {
                    case "A":

                        try
                        {
                            BotMethod.ViewStatistics();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання… Я ще вчуся, але скоро зможу це зробити!", cancellationToken: cancellationToken);

                            await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
                        }

                    break;

                    case "B":

                        var keyboard = new InlineKeyboardMarkup(
                        [
                            [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути збір статистики", "BA")],
                            [InlineKeyboardButton.WithCallbackData("Ввімкнути/вимкнути історію", "BB")],
                        ]);

                        await bot.SendMessage(chatId, "Налаштування відкрито! Обирай, що змінити:", replyMarkup: keyboard, cancellationToken: cancellationToken);
                        
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
                            await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…  Я ще вчуся, але скоро зможу це зробити!", cancellationToken: cancellationToken);

                            await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
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
                            await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…  Я ще вчуся, але скоро зможу це зробити!", cancellationToken: cancellationToken);

                            await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
                        }

                    break;

                    case "C":

                        var moodKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[] { InlineKeyboardButton.WithCallbackData("Веселий", "HO") },
                            new[] { InlineKeyboardButton.WithCallbackData("Сумний", "SO") },
                            new[] { InlineKeyboardButton.WithCallbackData("Злий", "AO") },
                            new[] { InlineKeyboardButton.WithCallbackData("Виснажений", "TO") },
                            new[] { InlineKeyboardButton.WithCallbackData("Спокійний", "CO") }
                        });

                        await bot.SendMessage(chatId, "Обери свій кото-настрій на сьогодні!:", replyMarkup: moodKeyboard, cancellationToken: cancellationToken);
                    
                    break;

                    case "HO":
                    case "SO":
                    case "AO":
                    case "TO":
                    case "CO":

                        userMoods[chatId] = callbackQuery.Data;
                        var contentKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[] { InlineKeyboardButton.WithCallbackData("Фільми", "MC") },
                            new[] { InlineKeyboardButton.WithCallbackData("Аніме", "AC") },
                            new[] { InlineKeyboardButton.WithCallbackData("Фото", "PC") }
                        });

                        await bot.SendMessage(chatId, $"Ваш настрій зафіксовано! Що бажаєте переглянути?", replyMarkup: contentKeyboard, cancellationToken: cancellationToken);

                    break;

                    case "MC":

                        await BotMethod.GenerateContent(bot, chatId, "movies", userMoods, cancellationToken);
                        await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);

                    break;

                    case "AC":

                        await BotMethod.GenerateContent(bot, chatId, "anime", userMoods, cancellationToken);
                        await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);

                    break;

                    case "PC":

                        await BotMethod.GenerateContent(bot, chatId, "photos", userMoods, cancellationToken);
                        await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                    
                    break;

                    case "E":

                        try
                        {
                            BotMethod.SendContent();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…  Я ще вчуся, але скоро зможу це зробити!", cancellationToken: cancellationToken);

                            await BotMethod.AskNextAsync(bot, chatId, cancellationToken: cancellationToken);
                        }

                    break;

                    case "F":

                        await bot.SendMessage(chatId, "Виберіть опцію:", replyMarkup: Keyboard.menu, cancellationToken: cancellationToken);
                    
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
                            await bot.SendMessage(chatId, "Ой, ця функція ще в процесі навчання…  Я ще вчуся, але скоро зможу це зробити!", cancellationToken: cancellationToken);
                        }

                    break;

                    default:

                        await bot.SendMessage(chatId, "Ой-Ой! Щось пішло не так...", cancellationToken: cancellationToken);

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
