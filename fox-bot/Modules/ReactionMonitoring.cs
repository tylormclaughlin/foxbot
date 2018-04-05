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
        [Summary("This command designates the previous message for reaction monitoring. The reaction that triggers action is currently :100:.")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        public async Task MonitorMessageAsync([Summary("Optional message ID to monitor")] int id = 0)
        {
            var messages = await Context.Channel.GetMessagesAsync(Context.Message.Id, Direction.Before, 1).Flatten();

            RestUserMessage msgToMonitor = (RestUserMessage)messages.FirstOrDefault();

            await ReplyAsync(MonitorList.AddMonitoredMessage(msgToMonitor));
        }

        [Command("unmonitor")]
        [Summary("This command will stop watching the message monitored through !monitor and unassign the role to all who were reacted to it.")]
        [RequireUserPermission(Discord.GuildPermission.Administrator, Group = "Allowed")]
        [Remarks("If the monitored message was accidentally deleted before using this command, this command must still be executed before another message can be monitored. " +
                 "Currently, if the message was deleted before !unmonitor was used, the assigned role must be unassigned manually.")]
        public async Task StopMonitoringMessageAsync([Summary("Optional message ID to stop monitoring")] int id = 0)
        {
            if (MonitorList.GetMonitoredMessage() == null)
            {
                await ReplyAsync("There is no message currently being monitored.");
                return;
            }

            string emoji = ReactionUtilities.emojitoCheck;
            var msg = await Context.Channel.GetMessageAsync(MonitorList.GetMonitoredMessage().Id) as RestUserMessage;

            if (msg == null)
            {
                MonitorList.DeleteMonitoredMessage();
                return;
            }
            var users = await msg.GetReactionUsersAsync(emoji);

            if (!users.Any())
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
