using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace lib;
public static class BotMethod
{
public static void SwitchHistory()
        {
            throw new NotImplementedException();
        }

        public static void SwitchStatistics()
        {
            throw new NotImplementedException();
        }

         public static void ViewStatistics()
        {
            throw new NotImplementedException();
        }

        public static void EndSession()
        {
            throw new NotImplementedException();
        }

        public static async Task AskNextAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    
        {        
                            var nextOptions = new InlineKeyboardMarkup(
                       [
                           [InlineKeyboardButton.WithCallbackData("Ще контенту!", "E"),],
                           [InlineKeyboardButton.WithCallbackData("\U0001F504 настрій", "C"), InlineKeyboardButton.WithCallbackData("\U0001F504тип контенту", "HO")],
                           [InlineKeyboardButton.WithCallbackData("До головного меню", "F"),],
                           [InlineKeyboardButton.WithCallbackData("Скінчити сесію", "G"),]
                       ]);

                         await bot.SendMessage(chatId, "Що далі?", replyMarkup: nextOptions, cancellationToken: cancellationToken);
        }

        public static void SendContent()
        {
            throw new NotImplementedException();
        }

        public static async Task GenerateContent(ITelegramBotClient bot, long chatId, string contentType, Dictionary<long, string> userMoods, CancellationToken cancellationToken)
        {
             if (!userMoods.TryGetValue(chatId, out string mood))
            {
                mood = "Neutral";
            }

            string response = contentType switch
            {
                "movies" => mood switch
                {
                    "HO" => "Мур-мур! Ось фільми, які подарують тобі багато радості та тепла, наче пухнастик, що весело ганяється за мотузочкою!",
                    "SO" => "Іноді хочеться посумувати, загорнувшись у ковдру, як котик у клубочок. Ось фільми, що допоможуть пережити ці моменти.",
                    "AO" => "Перемкни свою злість у пригоди! Ось список фільмів, де герої, як кіт, що вирішив підкорити вершину шафи, як би тяжко не було, ніколи не здається!!",
                    "TO" => "Втома буває у всіх, навіть у хвостатих мандрівників. Ось фільми, що допоможуть відпочити та зарядитися затишком.",
                    "CO" => "Ці фільми огорнуть тебе спокоєм, як тепле муркотіння поруч. Вдихни, видихни – і просто насолоджуйся.",
                    _ => "Йой.."
                },
                "anime" => mood switch
                {
                    "HO" => "Муркотливий світ аніме чекає! Ось історії, які подарують тобі сміх і радість, наче котик, що знайшов нову коробку!",
                    "SO" => "Якщо душа просить глибоких емоцій, ось аніме, що огорне тебе ніжними почуттями, як теплі лапки у холодний день.",
                    "AO" => "Пора на справжню пригоду! Ці аніме такі ж динамічні, як кіт, що женеться за лазерним променем по всій кімнаті!",
                    "TO" => "Іноді хочеться просто полежати і нічого не робити... Ось аніме, що допоможуть розслабитися та відпочити.",
                    "CO" => "Спокійне аніме, що подарує затишок, наче муркотіння улюбленого пухнастика під боком.",
                    _ => "Йой.."
                },
                "photos" => mood switch
                {
                    "HO" => "Ось для тебе наймиліші фото, сповнені тепла і радості! Нехай вони піднімуть настрій, як сонечко на підвіконні!",
                    "SO" => "Навіть у сумних моментах важливо знати, що тебе розуміють. Ось фото, які огорнуть тебе теплом, наче м’який хвостик.",  
                    "AO" => "Готовий до вибуху емоцій? Ось картинки, що запалять в тобі енергію, наче кіт, який вирішив побігати о третій ночі!",  
                    "TO" => "Час для відпочинку! Ці зображення такі ж затишні, як котик, що скрутився калачиком поруч із тобою.",  
                    "CO" => "Моменти спокою важливі для всіх – навіть для пухнастиків. Ось фотографії, що огорнуть тебе гармонією та теплом.",
                    _ => "Йой.."
                },
                _ => "Ой-ой! Щось пішло не так..."
            };
            
            await bot.SendMessage(chatId, response, cancellationToken: cancellationToken);
        }
}