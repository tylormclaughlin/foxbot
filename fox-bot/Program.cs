﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using foxbot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
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
                .BuildServiceProvider();

            //Get bot token from file
            string botToken = File.ReadAllText("bot_token.txt");

            //Event subscriptions
            _client.Log += Log;

            //Register command modules
            await RegisterCommandsAsync();

            //Log in the bot
            await _client.LoginAsync(TokenType.Bot, botToken);

            //Start the client
            await _client.StartAsync();

            //Keep client from closing
            await Task.Delay(-1);
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
                    Console.WriteLine(result.ErrorReason + message.Content);
                }
            }
            //Create google maps link for locations pasted into the channel
            else if(message.ToString().Contains("Location: "))
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
