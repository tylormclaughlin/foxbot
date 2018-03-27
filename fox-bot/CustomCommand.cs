using System;
using System.Collections.Generic;
using System.Text;

namespace foxbot
{
    class CustomCommand
    {
        public string commandName;
        public string commandContent;

        public CustomCommand(string cmdName, string cmdContent)
        {
            commandName = cmdName;
            commandContent = cmdContent;
        }
    }
}
