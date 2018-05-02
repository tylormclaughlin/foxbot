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
        [Command("purge")]
        [Summary("Clears the channel of unpinned messages.")]
        [RequireRole("Team Leader+", "Admin", Group = "Allowed")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Allowed")]
        [RequireUserPermission(Discord.ChannelPermission.ManageMessages)]
        [RequireBotPermission(Discord.ChannelPermission.ManageMessages)]
        public async Task PurgeAsync([Summary("Number of messages to delete. Default = 1000")] int numberToDelete = 1000)
        {
            var user = (SocketGuildUser)Context.User;
            
            //Dramatically more effective implementation. Add error handling.
            var messages = await Context.Channel.GetMessagesAsync(numberToDelete).Flatten();
            var messagesToDelete = messages.Where(msg => !msg.IsPinned); //Don't delete pinned messages
            messagesToDelete = messagesToDelete.Where(msg => (DateTime.Today - msg.CreatedAt.DateTime).TotalDays < 14); //Don't try to delete anything older than 14 days

            if (messagesToDelete.Any())
            {
                await Context.Channel.DeleteMessagesAsync(messagesToDelete);
            }
            else
            {
                await ReplyAsync("I cannot delete pinned messages");
            }
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
