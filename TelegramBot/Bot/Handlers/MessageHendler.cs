using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Bot.Lib.Keyboards;

namespace TelegramBot.TelegramBot.Bot.Handlers;

public class MessageHandler
{
    [Obsolete]
    public async Task HandleAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
    {
        if (message.Type != MessageType.Text || string.IsNullOrEmpty(message.Text))
            return;

        try
        {
            switch (message.Text)
            {
                case "/start":
                    await bot.SendTextMessageAsync(
                        message.Chat.Id, //Цей виклик надсилає текстове повідомлення в той самий чат, звідки прийшло /start, з певним текстом і, якщо вказано, кнопками.
                        "Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:",
                        replyMarkup: Keyboard.MainMenu,
                        cancellationToken: cancellationToken
                    );
                    break;

                default:
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Невідома команда. Спробуйте /start.",
                        cancellationToken: cancellationToken
                    );
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при обробці повідомлення: {ex.Message}");
        }
    }
}