using System;

namespace TelegramBot.Bot.Services;

public class CurrentMoodManager
{
// Словник для зберігання настрою кожного користувача за TelegramId
        private static readonly Dictionary<long, string> UserMoods = new();

        /// <summary>
        /// Зберігає або оновлює настрій користувача
        /// </summary>
        public static void SetMood(long userId, string mood)
        {
            UserMoods[userId] = mood;
        }

        /// <summary>
        /// Повертає настрій користувача, якщо такий є
        /// </summary>
        public static string? GetMood(long userId)
        {
            return UserMoods.TryGetValue(userId, out var mood) ? mood : null;
        }

        /// <summary>
        /// Видаляє настрій (наприклад, після завершення сесії)
        /// </summary>
        public static void ClearMood(long userId)
        {
            UserMoods.Remove(userId);
        }
}
