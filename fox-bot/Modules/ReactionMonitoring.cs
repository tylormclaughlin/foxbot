using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Rest;

namespace foxbot.Modules
{
    public class ReactionMonitoring : ModuleBase<SocketCommandContext>
    {
        [Command("monitor")]
        public async Task MonitorMessageAsync(int id = 0)
        {
            var messages = await Context.Channel.GetMessagesAsync(Context.Message.Id, Direction.Before, 1).Flatten();

            RestUserMessage msgToMonitor = (RestUserMessage)messages.FirstOrDefault();

            await ReplyAsync(MonitorList.AddMonitoredMessage(msgToMonitor));
        }
    }
}
