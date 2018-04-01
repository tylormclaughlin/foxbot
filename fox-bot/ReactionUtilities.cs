using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot
{
    public class ReactionUtilities
    {
        public static string roleName = "raid-run";
        public static string emojitoCheck = "💯";

        public static async Task<SocketGuildUser> GetUserAsync(ISocketMessageChannel channel, SocketReaction reaction)
        {
            SocketGuildUser user;

            if (reaction.User.IsSpecified)
            {
                user = (SocketGuildUser)reaction.User;
            }
            else
            {
                user = await channel.GetUserAsync(reaction.UserId) as SocketGuildUser;
            }

            if (user.IsBot)
            {
                return null;
            }

            return user;
        }

        public static async Task AddRoleToUserAsync(ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name != emojitoCheck) return;

            SocketGuildUser user = await GetUserAsync(channel, reaction);

            if (user != null)
            {
                //Print something to confirm this works
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                await user.AddRoleAsync(role);

                await channel.SendMessageAsync($"{user.Mention} was assigned {role.Mention}");
            }
        }

        public static async Task RemoveRoleFromUserAsync(ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name != emojitoCheck) return;

            SocketGuildUser user = await GetUserAsync(channel, reaction);

            if (user != null)
            {
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                await user.RemoveRoleAsync(role);

                await channel.SendMessageAsync($"{user.Mention} was unassigned {role.Mention}");
            }
        }

        public static async Task RemoveRoleFromUserAsync(RestUser user, SocketGuild guild)
        {
            if (user != null)
            {
                
                var role = guild.Roles.FirstOrDefault(x => x.Name == roleName);
                var u = guild.GetUser(user.Id);
                await u.RemoveRoleAsync(role);
            }
        }
    }
}
