using TelegramBot.Bot.Handlers.CallbackHandlers;

namespace TelegramBot.Bot.Services
{
    public class CommandRouter
    {
        private readonly List<ICallbackHandler> _handlers;

        public CommandRouter()
        {
            _handlers = new List<ICallbackHandler>
        {
            new MoodHandler(),
            new SettingsHandler(),
            new ContentHandler(),
            new MainMenuHandler(),
            new MoodStatistHandler()
        };
        }

        public ICallbackHandler? Route(string data)
        {
            return _handlers.FirstOrDefault(h => h.CanHandle(data));
        }
    }
}
