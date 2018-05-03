﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading; // 1) Add this namespace
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Concurrent;

namespace foxbot
{
    //Borrowed from here https://gist.github.com/Joe4evr/967949a477ed0c6c841407f0f25fa730
    public class TimerService
    {
        private readonly PurgeService _purgeService;
        private readonly ConcurrentDictionary<string, Timer> _timerList = new ConcurrentDictionary<string, Timer>();

        public TimerService(DiscordSocketClient client, PurgeService purgeService)
        {
            //Feed feed = FeedReader.ReadAsync("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/").Result;

            _purgeService = purgeService;

            _timerList.TryAdd("apk", new Timer(async _ =>
            {
                // 3) Any code you want to periodically run goes here, for example:
                var channels = client.Guilds.SelectMany(x => x.TextChannels).Where(x => x.Name == "timer-test");

                if (channels.Any())
                {
                    foreach (var channel in channels)
                    {
                        await channel.SendMessageAsync("This timer is working.");

                        //Put RSS Feed reader code in separate method, create feed reader service, use feed reader method here.

                    }
                }
            },
            null,
            TimeSpan.FromMinutes(30),  // 4) Time that message should fire after the timer is created
            TimeSpan.FromMinutes(60)));

            //Do some math to figure out when midnight is so we know when to fire the timer for the first time.
            DateTime now = DateTime.Now;
            DateTime midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);

            TimeSpan untilMidnight = midnight - now;

            _timerList.TryAdd("sightings", new Timer(async _ =>
            {
                var channels = client.Guilds.SelectMany(x => x.TextChannels).Where(x => x.Name == "timer-test");

                foreach (var channel in channels)
                {
                    await _purgeService.PurgeChannel(channel.Id);
                    await channel.SendMessageAsync(DateTime.Now.ToString());
                }
            },
            null,
            untilMidnight,
            TimeSpan.FromHours(24)));
        }

        public void Stop() // 6) Example to make the timer stop running
        {
            //_timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Restart() // 7) Example to restart the timer
        {
            //_timer.Change(TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30));
        }
    }
}
