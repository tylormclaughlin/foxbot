using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Purge : ModuleBase<SocketCommandContext>
    {
        private readonly PurgeService _purgeService;

        public Purge(PurgeService purgeService)
        {
            _purgeService = purgeService;
        }

        [Command("purge")]
        [Summary("Clears the channel of unpinned messages.")]
        [RequireRole("Team Leader+", "Admin", Group = "Allowed")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Allowed")]
        [RequireUserPermission(Discord.ChannelPermission.ManageMessages)]
        [RequireBotPermission(Discord.ChannelPermission.ManageMessages)]
        public async Task PurgeAsync([Summary("Number of messages to delete. Default = 1000")] int numberToDelete = 1000)
        {
            await _purgeService.PurgeChannel(Context.Message.Channel.Id);
        }
    }
}
