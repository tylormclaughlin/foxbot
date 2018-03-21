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
            //Dramatically more effective implementation. Add error handling.
            var messages = await Context.Channel.GetMessagesAsync().Flatten();
            var messagesToDelete = messages.Where(msg => !msg.IsPinned);

            await Context.Channel.DeleteMessagesAsync(messagesToDelete);

            //foreach (var msg in messages)
            //{
            //    try
            //    {
            //        //Don't attempt to delete pinned messages.
            //        if (msg.IsPinned)
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //            await msg.DeleteAsync();
            //        }
            //    }
            //    //If a message can't be deleted for some reason, send the reason back.
            //    catch(Exception ex)
            //    {
            //        await Context.Channel.SendMessageAsync(ex.Message);
            //    }
            //}


        }
    }
}
