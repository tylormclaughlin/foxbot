using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Purge : ModuleBase<SocketCommandContext>
    {
        [Command("purge")]
        [Summary("Clears the channel of all messages.")]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        [RequireBotPermission(Discord.ChannelPermission.ManageMessages)]
        public async Task purgeAsync()
        {
            //Note for later, try adding messges to new list and then calling DeleteMessagesAsync.
            var messages = await Context.Channel.GetMessagesAsync().Flatten();
            foreach (var msg in messages)
            {
                try
                {
                    //Don't attempt to delete pinned messages.
                    if (msg.IsPinned)
                    {
                        continue;
                    }
                    else
                    {
                        await msg.DeleteAsync();
                    }
                }
                //If a message can't be deleted for some reason, send the reason back.
                catch(Exception ex)
                {
                    await Context.Channel.SendMessageAsync(ex.Message);
                }
            }


        }
    }
}
