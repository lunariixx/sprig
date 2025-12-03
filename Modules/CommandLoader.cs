using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using Sprig.Utils.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Sprig.Modules
{
    public static class CommandLoader
    {
        public static void LoadText(CommandsNextExtension commands)
        {
            var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
                                       .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseCommandModule)));

            foreach (var type in commandTypes)
            {
                try
                {
                    commands.RegisterCommands(type);
                    Logger.LogDebug(Logger.EventIds.ModuleLoader, $"Registered text command module: {type.FullName}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(Logger.EventIds.ModuleLoader, $"Failed to register text command {type.FullName}: {ex.Message}");
                }
            }
        }

        public static void LoadSlash(SlashCommandsExtension slash)
        {
            var slashTypes = Assembly.GetExecutingAssembly().GetTypes()
                                     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ApplicationCommandModule)));

            foreach (var type in slashTypes)
            {
                try
                {
                    slash.RegisterCommands(type);
                    Logger.LogDebug(Logger.EventIds.ModuleLoader, $"Registered slash command module: {type.FullName}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(Logger.EventIds.ModuleLoader, $"Failed to register slash command {type.FullName}: {ex.Message}");
                }
            }
        }
    }
}
