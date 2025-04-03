using Telegram.Bot.Types.ReplyMarkups;

namespace lib;
public static class Keyboard
{
public static InlineKeyboardMarkup menu = new InlineKeyboardMarkup(
                [

                      [InlineKeyboardButton.WithCallbackData("Обрати настрій", "C"),],
                      [InlineKeyboardButton.WithCallbackData("Налаштування", "B")],
                      [InlineKeyboardButton.WithCallbackData("Статистика настрою", "A")]
                    
                ]);
}