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
        [Summary("This command allows you to edit the contents of an existing text command.")]
        [RequireOwner(Group = "Allowed")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task EditCommandAsync([Summary("The name of the command you want to edit")] string cmdName, [Summary("The new contents of the command")] [Remainder]string cmdContent)
        {
            string result = await CommandUtilities.ModifyCommandAsync(cmdName, cmdContent, _commandService);

            await ReplyAsync(result);
        }
    }
}
