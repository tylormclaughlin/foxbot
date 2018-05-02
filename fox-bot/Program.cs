using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using foxbot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace fox_bot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private string maps_api_key = File.ReadAllText("map_api_key.txt"); //get google maps API token from file

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandUtilities>()
                .AddSingleton<MonitorList>()
                .AddSingleton<TimerService>()
                .BuildServiceProvider();

            _services.GetRequiredService<TimerService>();

            //Get bot token from file
            string botToken = File.ReadAllText("bot_token.txt");

            //Event subscriptions
            _client.Log += Log;
            _client.ReactionAdded += ReactionAdded;
            _client.ReactionRemoved += ReactionRemoved;

            //Register command modules
            await RegisterCommandsAsync();

            //Log in the bot
            await _client.LoginAsync(TokenType.Bot, botToken);

            //Start the client
            await _client.StartAsync();

            //Keep client from closing
            await Task.Delay(-1);
        }

        private async Task ReactionRemoved(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            RestUserMessage msg = MonitorList.GetMonitoredMessage();

            if (msg != null)
            {
                try
                {
                    await ReactionUtilities.RemoveRoleFromUserAsync(channel, reaction);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    
                }
            }
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            RestUserMessage msg = MonitorList.GetMonitoredMessage();

            if (msg != null)
            {

                try
                {
                    await ReactionUtilities.AddRoleToUserAsync(channel, reaction);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await channel.SendMessageAsync("Something bad happened. Consult this bot's administrator.");
                }
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            //Load pre-existing custom commands if possible
            if (DataStorage.LoadCommandsFromFile())
            {
                foreach (CustomCommand cmd in DataStorage.customCommands)
                {
                    await CommandUtilities.CreatCommandAsync(cmd.commandName, cmd.commandContent, _commands);
                }
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            

            if (message is null || message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            int argPos = 0;

            //Handle messages containing commands, which are prefixed with !
            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                //If ExecuteAsync fails, log message to the console.
                if (!result.IsSuccess)
                {
                    await message.Channel.SendMessageAsync(result.ErrorReason);
                    Console.WriteLine(result.ErrorReason + message.Content);
                }
            }

            //if (message.Channel.Name == "sightings")
            //{
            //    if (message.Attachments.Any())
            //    {
            //        var attachment = message.Attachments.FirstOrDefault();

            //        if (attachment != null)
            //        {
            //            //Check to see if the attachment is probably an image
            //            string[] extensions = { ".png", "jpg", ".bmp" };
            //            var result = extensions.Any(x => attachment.Filename.EndsWith(x));

            //            if ((result == true) && (attachment.Height != null))
            //            {
            //                try
            //                {
            //                    //EmbedBuilder eb = new EmbedBuilder();

            //                    //eb.WithDescription(message.Content);
            //                    //eb.WithImageUrl(attachment.Url);
            //                    //eb.WithColor(Color.DarkGreen);

            //                    //await context.Channel.SendMessageAsync("", false, eb);
            //                }
            //                catch (Exception e)
            //                {
            //                    Console.WriteLine(e.Message);
            //                }
            //            }
            //        }
            //    }
            //}
            //Create google maps link for locations pasted into the channel
            if(message.ToString().Contains("Location: "))
            {
                
                //Message format indicates lat/long coordinates should be after this prefix
                const string locationPrefix = "Location: ";
                const string mapsLink = "http://maps.google.com/maps?q=";

                string location = "";

                //Split the message into lines to look for the location coordinates
                string[] lines = message.ToString().Split("\n");

                foreach (string line in lines)
                {
                    if (line.Contains("Location: "))
                    {
                        int start = line.LastIndexOf(locationPrefix) + locationPrefix.Length;
                        int length = line.Length - start;

                        location = line.Substring(start, length);
                        location = location.Replace(" ", string.Empty);

                        //Create map URL with google's static map API
                        string mapView = CreateMapURL(location);

                        EmbedBuilder eb = new EmbedBuilder();

                        eb.WithTitle("Map Link");
                        eb.WithDescription(mapsLink + location);

                        eb.WithImageUrl(mapView);

                        await context.Channel.SendMessageAsync("", false, eb);
                    }
                }
            }
        }

        private string CreateMapURL(string center)
        {
            string mapURL = "https://maps.googleapis.com/maps/api/staticmap?center=" + center +
                            "&size=350x250" +
                            "&zoom=17" +
                            "&maptype=roadmap" +
                            "&markers=color:red%7Clabel:P%7C" + center +
                            "&key=" + maps_api_key;

            return mapURL;
        }
    }
}
