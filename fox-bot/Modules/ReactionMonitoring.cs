using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class ReactionMonitoring : ModuleBase<SocketCommandContext>
    {
        [Command("monitor")]
        public async Task MonitorMessageAsync(int id = 0)
        {
            var messages = await Context.Channel.GetMessagesAsync(Context.Message.Id, Direction.Before, 1).Flatten();

            IMessage msgToMonitor = messages.FirstOrDefault();

            await ReplyAsync(msgToMonitor.Id.ToString());
        }
    }
}
