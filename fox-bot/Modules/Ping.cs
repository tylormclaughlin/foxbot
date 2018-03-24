using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace fox_bot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Crashes the !commands command when it doesn't have a Summary precondition.")]
        public async Task PingAsync()
        {
            await ReplyAsync("Hello world!");
        }
    }
}
