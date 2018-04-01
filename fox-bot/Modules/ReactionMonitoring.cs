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
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task MonitorMessageAsync(int id = 0)
        {
            var messages = await Context.Channel.GetMessagesAsync(Context.Message.Id, Direction.Before, 1).Flatten();

            RestUserMessage msgToMonitor = (RestUserMessage)messages.FirstOrDefault();

            await ReplyAsync(MonitorList.AddMonitoredMessage(msgToMonitor));
        }

        [Command("unmonitor")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task StopMonitoringMessageAsync(int id = 0)
        {
            if (MonitorList.GetMonitoredMessage() == null)
            {
                await ReplyAsync("There is no message currently being monitored.");
                return;
            }

            string emoji = "💯";
            var msg = await Context.Channel.GetMessageAsync(MonitorList.GetMonitoredMessage().Id) as RestUserMessage;
            var users = await msg.GetReactionUsersAsync(emoji);

            if (users == null)
            {
                return;
            }

            foreach (RestUser user in users)
            {
                await ReactionUtilities.RemoveRoleFromUserAsync(user, Context.Guild);
            }

            MonitorList.DeleteMonitoredMessage();
            await ReplyAsync("Message is no longer monitored and all reacted users have had their role unassigned.");
        }
    }
}
