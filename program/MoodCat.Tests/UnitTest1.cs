using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace test.Tests
{
    public class ProgramTests
    {
        private Mock<ITelegramBotClient> _mockBotClient;
        private CancellationToken _cancellationToken;

        public ProgramTests()
        {
            _mockBotClient = new Mock<ITelegramBotClient>();
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async Task UpdateHandler_StartCommand_SendsWelcomeMessage()
        {
            // Arrange
            var update = new Update
            {
                Message = new Message
                {
                    Chat = new Chat { Id = 123456 },
                    Text = "/start"
                }
            };

            // Act
            await Program.InvokeUpdateHandler(_mockBotClient.Object, update, _cancellationToken);

            // Assert
            _mockBotClient.Verify(bot => bot.SendMessage(
                It.IsAny<ChatId>(), 
                It.Is<string>(msg => msg.Contains("Привіт! Я MoodCat, твій пухнастий помічник у світі настроїв! Обери, що тобі потрібно:")), 
                ParseMode.Markdown, 
                null, 
                It.IsAny<ReplyMarkup>(), 
                null, 
                null, 
                null, 
                false, 
                false, 
                null, 
                null, 
                false, 
                _cancellationToken
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateHandler_UnknownCommand_SendsErrorMessage()
        {
            // Arrange
            var update = new Update
            {
                Message = new Message
                {
                    Chat = new Chat { Id = 123456 },
                    Text = "/unknown"
                }
            };

            // Act
            await Program.InvokeUpdateHandler(_mockBotClient.Object, update, _cancellationToken);

            // Assert
            _mockBotClient.Verify(bot => bot.SendMessage(
                It.IsAny<ChatId>(), 
                It.Is<string>(msg => msg.Contains("Ой, ця функція ще в процесі навчання… Я ще вчуся, але скоро зможу це зробити!")), 
                ParseMode.Markdown, 
                null, 
                It.IsAny<ReplyMarkup>(), 
                null, 
                null, 
                null, 
                false, 
                false, 
                null, 
                null, 
                false, 
                _cancellationToken
            ), Times.Once);
        }
    }
}