using Telegram.Bot.Types.ReplyMarkups;

namespace test;
public static class Keyboard
{
public static InlineKeyboardMarkup menu = new InlineKeyboardMarkup(
                [

                      [InlineKeyboardButton.WithCallbackData("А", "A"),],
                      [InlineKeyboardButton.WithCallbackData("B", "B")],
                      [InlineKeyboardButton.WithCallbackData("Обрати настрій", "C")]
                    
                ]);
}