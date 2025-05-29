using Telegram.Bot;
using Telegram.Bot.Types;

using TelegramBot.Bot.Lib.Methods;

namespace TelegramBot.Bot.Handlers.CallbackHandlers;

public class ContentHandler : ICallbackHandler
{
    public bool CanHandle(string data) => new[] { "MC", "AC", "PC" }.Contains(data);

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery query, Dictionary<long, string> userMoods, CancellationToken cancellationToken)
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
            await BotMethod.GenerateContent(bot, query.Message.Chat.Id, contentType, userMoods, cancellationToken);
            await BotMethod.AskNextAsync(bot, query.Message.Chat.Id, cancellationToken);
        }
    }
}

