using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Nicknames : Discord.Commands.ModuleBase<SocketCommandContext>
    {
        [Command("nicknames")]
        public async Task FindUsersWithoutNicknames()
        {
            await Context.Guild.DownloadUsersAsync();

            List<string> usernames = new List<string>();
            
            foreach (var user in Context.Guild.Users)
            {
                if (user.Nickname == null)
                {
                    //await ReplyAsync(user.ToString());
                    usernames.Add(user.ToString());
                }
            }

            bool result = DataStorage.SaveUsernamesToFile(usernames);

            if (result)
            {
                await ReplyAsync("Finished finding all users with no nickname set.");
            }
            else
            {
                await ReplyAsync("Dunno how this failed, but it did. Contact this bot's administrator.");
            }
        }
    }
}
