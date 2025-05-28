using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Bot.Lib.Keyboards;
public static class Keyboard
{
public static InlineKeyboardMarkup MainMenu = new InlineKeyboardMarkup(
                      [

                            [InlineKeyboardButton.WithCallbackData("Обрати настрій", "C"),],
                      [InlineKeyboardButton.WithCallbackData("Налаштування", "B")],
                      [InlineKeyboardButton.WithCallbackData("Статистика настрою", "A")]

                      ]);
}