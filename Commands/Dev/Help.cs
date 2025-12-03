using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprig.Commands.Dev
{
    public class HelpCommand : BaseCommandModule
    {
        [Command("help")] // If you intend on doing text-based non-dev commands ever, please change this to "devhelp"
        [Description("Lists all dev-only commands.")]
        public async Task Help(CommandContext ctx)
        {
            var commands = ctx.CommandsNext.RegisteredCommands.Values
                .Where(c => c.Module.ModuleType.Namespace?.Contains("Dev") ?? false)
                .OrderBy(c => c.Name)
                .ToList();

            if (!commands.Any())
            {
                await ctx.RespondAsync("No dev commands found.");
                return;
            }

            var sb = new StringBuilder();
            foreach (var cmd in commands)
            {
                sb.AppendLine($"`{ctx.Prefix}{cmd.Name}` - {cmd.Description}");
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = "Dev Commands",
                Description = sb.ToString(),
                Color = DiscordColor.Blurple // use blurple for now.
            };

            await ctx.RespondAsync(embed: embed);
        }
    }
}
