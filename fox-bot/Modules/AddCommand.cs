using Discord.Commands;
using System;
using System.Collections.Generic;
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
            await ReplyAsync($"Created command !{cmdName} - {cmdContent}");
        }

    }
}
