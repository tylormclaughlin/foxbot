using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public Commands(CommandService commands)
        {
            _commandService = commands;
        }

        [Command("commands")]
        [Summary("Lists all recognized commands and a brief decription of their function.")]
        public async Task CommandHelpAsync(string commandToDescribe = "")
        {
            var commandList = _commandService.Commands;

            if (commandToDescribe == "")
            {
                foreach (var command in commandList)
                {
                    EmbedBuilder eb = new EmbedBuilder();

                    eb.AddField("Command: ", command.Name);
                    eb.AddField("Summary: ", command.Summary);

                    await ReplyAsync("", false, eb);
                }
            }
        }
    }
}
