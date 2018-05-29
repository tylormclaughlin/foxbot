using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using foxbot;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "apk");

            await ReplyAsync(role.Mention + " this is a test");
        }

        [Command("user")]
        public async Task PingUserAsync(SocketGuildUser user)
        {
            await ReplyAsync($"pong, {user.Mention}");
        }
    }
}
