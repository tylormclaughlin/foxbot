﻿using System;
using System.Collections.Generic;
using System.Text;

namespace foxbot
{
    using System;
    using System.Linq;
    using System.Threading; // 1) Add this namespace
    using System.Threading.Tasks;
    using CodeHollow.FeedReader;
    using Discord.Commands;
    using Discord.WebSocket;

    public class TimerService
    {
        private readonly Timer _timer; // 2) Add a field like this
                                       // This example only concerns a single timer.
                                       // If you would like to have multiple independant timers,
                                       // you could use a collection such as List<Timer>,
                                       // or even a Dictionary<string, Timer> to quickly get
                                       // a specific Timer instance by name.

        private string currentVersion;

        public TimerService(DiscordSocketClient client)
        {
            Feed feed = FeedReader.ReadAsync("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/").Result;

            currentVersion = feed.Items.FirstOrDefault().Title;

            _timer = new Timer(async _ =>
            {
                // 3) Any code you want to periodically run goes here, for example:
                var channels = client.Guilds.SelectMany(x => x.TextChannels).Where(x => x.Name == "timer-test");

                if (channels.Any())
                {
                    foreach(var channel in channels)
                    {
                        await channel.SendMessageAsync("This timer is working.");
                    }
                }
            },
            null,
            TimeSpan.FromMinutes(1),  // 4) Time that message should fire after the timer is created
            TimeSpan.FromMinutes(1)); // 5) Time after which message should repeat (use `Timeout.Infinite` for no repeat)
        }

        public void Stop() // 6) Example to make the timer stop running
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Restart() // 7) Example to restart the timer
        {
            _timer.Change(TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30));
        }
    }
}
