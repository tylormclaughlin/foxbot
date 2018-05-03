using System;
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
        private readonly Timer _timer;
        private readonly ConcurrentDictionary<string, Timer> _timerList = new ConcurrentDictionary<string, Timer>();

        private string currentVersion;

        public TimerService(DiscordSocketClient client, CommandService _commandService)
        {
            Feed feed = FeedReader.ReadAsync("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/").Result;

            currentVersion = feed.Items.FirstOrDefault().Title;

            //_timer = new Timer(async _ =>
            //{
            //    // 3) Any code you want to periodically run goes here, for example:
            //    var channels = client.Guilds.SelectMany(x => x.TextChannels).Where(x => x.Name == "timer-test");

            //    if (channels.Any())
            //    {
            //        foreach(var channel in channels)
            //        {
            //            //await channel.SendMessageAsync("This timer is working.");

            //            //Put RSS Feed reader code in separate method, create feed reader service, use feed reader method here.
                        
            //        }
            //    }
            //},
            //null,
            //TimeSpan.FromMinutes(1),  // 4) Time that message should fire after the timer is created
            //TimeSpan.FromMinutes(1)); // 5) Time after which message should repeat (use `Timeout.Infinite` for no repeat)

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
            TimeSpan.FromMinutes(1),  // 4) Time that message should fire after the timer is created
            TimeSpan.FromMinutes(1)));

            _timerList.TryAdd("sightings", new Timer(async _ =>
            {
                // 3) Any code you want to periodically run goes here, for example:
                var channels = client.Guilds.SelectMany(x => x.TextChannels).Where(x => x.Name == "timer-test");

                if (channels.Any())
                {
                    foreach (var channel in channels)
                    {
                        //await channel.SendMessageAsync("This timer is working.");

                        //Put RSS Feed reader code in separate method, create feed reader service, use feed reader method here.

                    }
                }
            },
            null,
            TimeSpan.FromMinutes(2),  // 4) Time that message should fire after the timer is created
            TimeSpan.FromMinutes(1)));
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
