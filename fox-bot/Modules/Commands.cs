using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace foxbot.Modules
{
    class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;

        public Commands(CommandService commands)
        {
            _commands = commands;
        }
    }
}
