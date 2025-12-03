using DotNetEnv;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Sprig.Modules;
using Sprig.Utils.Logging;

Env.Load();

bool debugEnabled = Env.GetBool("DEBUG", false);

var discord = new DiscordClient(new DiscordConfiguration()
{
    Token = Env.GetString("DISCORD_TOKEN"),
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.All,
    MinimumLogLevel = debugEnabled ? LogLevel.Debug : LogLevel.Information,
    LogTimestampFormat = "yyyy.MM.dd - HH:mm:ss"
});

Logger.Init(discord);

var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
{
    StringPrefixes = new[] {"sprig."},
    EnableDefaultHelp = false
});

var slash = discord.UseSlashCommands();

CommandLoader.LoadText(commands);
CommandLoader.LoadSlash(slash);
EventLoader.Load(discord);

await discord.ConnectAsync();
await Task.Delay(-1);