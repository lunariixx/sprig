using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace Sprig.Utils.Logging
{
    public static class Logger
    {
        private static DiscordClient? _client;

        public static void Init(DiscordClient client)
        {
            _client = client;
        }

        public static void LogTrace(EventId? id, string message)
            => _client?.Logger.LogTrace(id ?? default, message);

        public static void LogDebug(EventId? id, string message)
            => _client?.Logger.LogDebug(id ?? default, message);

        public static void LogInfo(EventId? id, string message)
            => _client?.Logger.LogInformation(id ?? default, message);

        public static void LogWarn(EventId? id, string message)
            => _client?.Logger.LogWarning(id ?? default, message);

        public static void LogError(EventId? id, string message)
            => _client?.Logger.LogError(id ?? default, message);

        public static void LogCrit(EventId? id, string message)
            => _client?.Logger.LogCritical(id ?? default, message);

        public static class EventIds
        {
            public static readonly EventId ModuleLoader = new EventId(200, "ModuleLoader");
        }
    }
}
