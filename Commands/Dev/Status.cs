using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sprig.Commands.Dev
{
    public class DevStatusCommand : BaseCommandModule
    {
        [Command("status")]
        [Description("Displays bot and system status information.")]
        public async Task StatusCommand(CommandContext ctx)
        {
            var botUptime = BotState.BotUptime.Elapsed;
            var botUptimeStr = $"{botUptime.Days}d {botUptime.Hours}h {botUptime.Minutes}m {botUptime.Seconds}s";

            string systemUptime = "Unknown";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && File.Exists("/proc/uptime"))
            {
                var uptimeSeconds = double.Parse(File.ReadAllText("/proc/uptime").Split(' ')[0]);
                var ts = TimeSpan.FromSeconds(uptimeSeconds);
                systemUptime = $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
                    systemUptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
                }
                catch { }
            }

            var proc = Process.GetCurrentProcess();
            var memUsedMB = proc.WorkingSet64 / 1024 / 1024;

            double cpuUsagePercent = 0;
            try
            {
                var startTime = DateTime.UtcNow;
                var startCpu = proc.TotalProcessorTime;
                await Task.Delay(500);
                var endTime = DateTime.UtcNow;
                var endCpu = proc.TotalProcessorTime;
                var cpuUsedMs = (endCpu - startCpu).TotalMilliseconds;
                var totalMs = (endTime - startTime).TotalMilliseconds * Environment.ProcessorCount;
                cpuUsagePercent = (cpuUsedMs / totalMs) * 100;
            }
            catch { }

            var partitions = DriveInfo.GetDrives()
                .Where(d => d.IsReady &&
                           (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                            !new[] { "/proc", "/sys", "/dev", "/run", "/tmp", "/boot/efi" }
                                .Any(p => d.Name.StartsWith(p))))
                .ToArray();

            string driveInfo;
            if (partitions.Length == 0)
                driveInfo = "No usable drives found.";
            else
            {
                var driveInfoLines = partitions
                    .Select(d => $"{d.Name}: {d.TotalFreeSpace / 1024 / 1024 / 1024}GB free of {d.TotalSize / 1024 / 1024 / 1024}GB")
                    .ToArray();
                driveInfo = string.Join("\n", driveInfoLines);
            }

            var websocketPing = ctx.Client.Ping;

            var embed = new DiscordEmbedBuilder
            {
                Title = "System & Bot Status",
                Color = DiscordColor.Blurple, // use blurple for now.
                Timestamp = DateTimeOffset.Now
            };

            embed.AddField("Bot Uptime", botUptimeStr, true);
            embed.AddField("System Uptime", systemUptime, true);
            embed.AddField("Websocket Ping", $"{websocketPing} ms", true);
            embed.AddField("CPU Usage", $"{cpuUsagePercent:F2}%", true);
            embed.AddField("Memory Used", $"{memUsedMB} MB", true);
            embed.AddField("Disk Usage", $"```{driveInfo}```");

            await ctx.RespondAsync(embed: embed);
        }
    }
}
