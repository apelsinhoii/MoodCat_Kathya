using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
namespace test;
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

        public static void EndSession()
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
                    "HO" => "Ось список веселих фільмів для гарного настрою!",
                    "SO" => "Ось список драматичних фільмів для роздумів...",
                    "AO" => "Ось список екшн-фільмів, щоб випустити пар!",
                    "TO" => "Ось список легких комедій для відпочинку.",
                    "CO" => "Ось список спокійних фільмів для релаксу.",
                    _ => "Йой.."
                },
                "anime" => mood switch
                {
                    "HO" => "Ось список веселих аніме!",
                    "SO" => "Ось список глибоких і драматичних аніме...",
                    "AO" => "Ось список бойових аніме!",
                    "TO" => "Ось список аніме, що допоможуть розслабитися.",
                    "CO" => " Ось список аніме з приємною атмосферою.",
                    _ => "Йой.."
                },
                "photos" => mood switch
                {
                    "HO" => "Веселий котик /ᐠ. ᴗ.ᐟ\\",
                    "SO" => "Сумний котик /ᐠᵕ̩̩̥ ‸ᵕ̩̩̥ ᐟ\\ﾉ",
                    "AO" => "Злий смайлик (；⌣̀_⌣́)",
                    "TO" => "Виснажений смайлик (_　_|||)",
                    "CO" => "Спокійний смайлик (∪｡∪)｡｡｡zzZ",
                    _ => "Йой.."
                },
                _ => "Щось пішло не так..."
            };

            await bot.SendMessage(chatId, response, cancellationToken: cancellationToken);
        }}

       