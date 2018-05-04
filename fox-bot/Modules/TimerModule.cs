using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class TimerModule : ModuleBase
    {
        private readonly TimerService _service;

        public TimerModule(TimerService service) // Make sure to configure your DI with your TimerService instance
        {
            _service = service;
        }

        // Example commands
        [Command("stoptimer")]
        public async Task StopCmd(string name)
        {
            _service.Stop(name);
            await ReplyAsync($"{name} timer stopped.");
        }

        [Command("starttimer")]
        public async Task RestartCmd(string name)
        {
            _service.Restart(name);
            await ReplyAsync($"{name} timer restarted.");
        }
    }
}
