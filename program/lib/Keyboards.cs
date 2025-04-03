using Telegram.Bot.Types.ReplyMarkups;

namespace lib;
public static class Keyboard
{
public static InlineKeyboardMarkup menu = new InlineKeyboardMarkup(
                [

                      [InlineKeyboardButton.WithCallbackData("А", "A"),],
                      [InlineKeyboardButton.WithCallbackData("Налаштування", "B")],
                      [InlineKeyboardButton.WithCallbackData("В", "C")]
                    
                ]);
}