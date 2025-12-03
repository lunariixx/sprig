using DSharpPlus;
using DSharpPlus.EventArgs;
using Sprig.Utils.Logging;
using System;
using System.Threading.Tasks;

namespace Sprig.Events
{
    public static class ReadyEvent
    {
        public static Task Ready(DiscordClient client, ReadyEventArgs e)
        {
            Logger.LogInfo(LoggerEvents.Startup, $"Bot connected as {client.CurrentUser.Username}#{client.CurrentUser.Discriminator}!");

            var _ = BotState.BotUptime;

            return Task.CompletedTask;
        }
    }
}
