using Telegram.Bot;
using Telegram.Bot.Types;

using TelegramBot.Bot.Lib.Methods;
using TelegramBot.Bot.Services;
using TelegramBot.Data;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class ContentHandler : ICallbackHandler
{
    public bool CanHandle(string data) => new[] { "MC", "AC", "PC" }.Contains(data);

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, string currUserMood, AppDbContext context, CancellationToken cancellationToken)
    {
        string contentType = query.Data switch
        {
            "MC" => "movies",
            "AC" => "anime",
            "PC" => "photos",
            _ => ""
        };

        if (!string.IsNullOrEmpty(contentType))
        {
            var userId = query.From.Id;
            currUserMood = CurrentMoodManager.GetMood(userId);
            await BotMethod.GenerateContent(bot, query.Message.Chat.Id, contentType, currUserMood, cancellationToken);
            await BotMethod.AskNextAsync(bot, query.Message.Chat.Id, cancellationToken);
        }
    }
}

