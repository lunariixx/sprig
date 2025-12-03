using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sprig.Commands.Utils
{
    public class PingSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("ping", "Shows bot websocket latency and response time.")]
        public async Task Ping(InteractionContext ctx)
        {
            var sw = Stopwatch.StartNew();
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            sw.Stop();

            var websocketPing = ctx.Client.Ping;
            var responseTime = sw.ElapsedMilliseconds;

            var embed = new DiscordEmbedBuilder
            {
                Title = "Bot Latency",
                Color = DiscordColor.Blurple,
                Timestamp = DateTimeOffset.Now,
            };

            embed.AddField("WebSocket Latency", $"**{websocketPing} ms**");
            embed.AddField("Message Round-Trip", $"**{responseTime} ms**");
            embed.WithFooter("Pong! 🏓");

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
