using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class EditCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public EditCommand(CommandService commands)
        {
            _commandService = commands;
        }

        [Command("editcom")]
        [RequireOwner(Group = "Allowed")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task EditCommandAsync(string cmdName, [Remainder]string cmdContent)
        {
            string result = await CommandUtilities.ModifyCommandAsync(cmdName, cmdContent, _commandService);

            await ReplyAsync(result);
        }
    }
}
