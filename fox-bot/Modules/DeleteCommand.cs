using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class DeleteCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public DeleteCommand(CommandService commands)
        {
            _commandService = commands;
        }

        [Command("delcom")]
        [Summary("This command allows you to delete and existing text command.")]
        [RequireOwner(Group = "Allowed")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task DeleteCommandAsync([Summary("The name of the command to delete")] string cmdName)
        {
            string result = await CommandUtilities.DeleteCommandAsync(cmdName, _commandService);

            await ReplyAsync(result);
        }
    }
}
