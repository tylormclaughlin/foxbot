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
        public static string emojiToCheck = "💯";

        /// <summary>
        /// This method returns a SocketGuildUser object of the user that placed a reaction. 
        /// </summary>
        /// <param name="channel">The channel of the mesasge being reacted to</param>
        /// <param name="reaction">The reaction placed on the message</param>
        /// <returns>The user as a SocketGuildUser, or null if the reaction was placed by a bot.</returns>
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

        /// <summary>
        /// This method assigns the roleName role to a user if they reacted with the correct reaction.
        /// </summary>
        /// <param name="channel">The channel of the message being reacted to</param>
        /// <param name="reaction">The reaction being placed on the message</param>
        /// <returns></returns>
        public static async Task AddRoleToUserAsync(ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name != emojiToCheck) return;

            SocketGuildUser user = await GetUserAsync(channel, reaction);

            if (user != null)
            {
                //Print something to confirm this works
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                await user.AddRoleAsync(role);

                await channel.SendMessageAsync($"{user.Mention} was assigned {role.Mention}");
            }
        }

        /// <summary>
        /// This method removes the roleName role from a user if they unreacted with the correct reaction.
        /// </summary>
        /// <param name="channel">The channel of the message being unreacted to</param>
        /// <param name="reaction">The reaction being removed from the message</param>
        /// <returns></returns>
        public static async Task RemoveRoleFromUserAsync(ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Emote.Name != emojiToCheck) return;

            SocketGuildUser user = await GetUserAsync(channel, reaction);

            if (user != null)
            {
                var role = user.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                await user.RemoveRoleAsync(role);

                await channel.SendMessageAsync($"{user.Mention} was unassigned {role.Mention}");
            }
        }

        /// <summary>
        /// This method removes the roleName role from a user.
        /// </summary>
        /// <param name="user">The user having the role removed</param>
        /// <param name="guild">The Guild the user belongs to</param>
        /// <returns></returns>
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
