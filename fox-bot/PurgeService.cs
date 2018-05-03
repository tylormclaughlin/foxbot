using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxbot
{
    public class PurgeService
    {
        private readonly DiscordSocketClient _client;

        public PurgeService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task PurgeChannel(ulong channelID, int numbertoDelete = 1000)
        {
            var channel = _client.GetChannel(channelID) as ISocketMessageChannel;

            var messages = await channel.GetMessagesAsync(numbertoDelete).Flatten();
            var messagesToDelete = messages.Where(msg => !msg.IsPinned); //Don't delete pinned messages
            messagesToDelete = messagesToDelete.Where(msg => (DateTime.Today - msg.CreatedAt.DateTime).TotalDays < 14); //Don't try to delete anything older than 14 days

            if (messagesToDelete.Any())
            {
                await channel.DeleteMessagesAsync(messagesToDelete);
            }
            else
            {
                await channel.SendMessageAsync("I cannot delete pinned messages, or messages older than 14 days.");
            }
        }
    }
}
