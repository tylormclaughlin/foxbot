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
            //Add new command info to commandPairs for persistence
            DataStorage.AddCustomCommand(cmdName, cmdContent);

            ModuleInfo module = await _commandService.CreateModuleAsync("", m =>
            {
                m.AddCommand(cmdName, async (ctx, _, provider, _1) =>
                {
                    await ReplyAsync(cmdContent);
                }, command => { });
            });

            await ReplyAsync($"Created command !{cmdName}");
        }

    }
}
