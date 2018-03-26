using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public Commands(CommandService commands, IServiceProvider service)
        {
            _commandService = commands;
        }

        [Command("commands")]
        [Summary("Lists all recognized commands and a brief decription of their function, or more detailed information about a specific command.")]
        public async Task CommandHelpAsync([Summary("Optional string containing the command to see detailed information about.")] string commandToDescribe = "")
        {
            var commandList = _commandService.Commands;

            if (commandToDescribe == "")
            {
                foreach (var command in commandList)
                {
                    //If user has permissions required to use the command, display the command
                    PreconditionResult pr = await command.CheckPreconditionsAsync(Context, _serviceProvider);
                    if (pr.IsSuccess)
                    {
                        EmbedBuilder eb = new EmbedBuilder();

                        eb.AddField("Command: ", command.Name ?? "");

                        if (command.Summary != null)
                        {
                            eb.AddField("Summary: ", command.Summary);
                        }

                        await ReplyAsync("", false, eb);
                    }
                }
            }
            else
            {
                var command = commandList.Where(cmd => cmd.Name == commandToDescribe);

                if (command.Any())
                {
                    CommandInfo cmdInfo = command.FirstOrDefault();

                    EmbedBuilder eb = new EmbedBuilder();

                    eb.AddField("Command", cmdInfo.Name);

                    if (cmdInfo.Summary != null)
                    {
                        eb.AddField("Summary", cmdInfo.Summary);
                    }

                    if (cmdInfo.Preconditions != null)
                    {
                        eb.AddField("Preconditions", string.Join('\n', cmdInfo.Preconditions));
                    }

                    if (cmdInfo.Parameters!= null)
                    {
                        eb.AddField("Parameters", string.Join('\n', cmdInfo.Parameters.Select(x => $"{x.Name} - {x.Summary}")));
                    }

                    eb.WithColor(Color.DarkGreen);

                    await ReplyAsync("", false, eb);
                }
                else
                {
                    await ReplyAsync("Sorry, I don't think that's a valid command. Use !commands to view a list of recognized commands.");
                }
            }
        }
    }
}
