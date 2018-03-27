using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class AddCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public AddCommand(CommandService commands)
        {
            _commandService = commands;
        }

        [Command("addcom")]
        [RequireOwner(Group = "Allowed")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task AddCommandAsync(string cmdName, [Remainder]string cmdContent)
        {
            string result = await CommandUtilities.AddCommandAsync(cmdName, cmdContent, _commandService);

            if (result == "success")
            {
                await ReplyAsync($"Created command !{cmdName}");
            }
            else
            {
                await ReplyAsync(result);
            }
        }

    }
}
