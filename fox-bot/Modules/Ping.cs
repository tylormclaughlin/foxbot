using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace fox_bot.Modules
{
    [Group("ping")]
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command]
        [Name("ping")]
        [Summary("Crashes the !commands command when it doesn't have a Summary precondition.")]
        public async Task PingAsync()
        {
            await ReplyAsync("Hello world!");
        }

        [Command("user")]
        public async Task PingUserAsync(SocketGuildUser user)
        {
            await ReplyAsync($"pong, {user.Mention}");
        }
    }
}
