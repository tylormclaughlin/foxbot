using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    [Group("keyword")]
    public class Keyword : ModuleBase<SocketCommandContext>
    {
        [Command]
        [RequireOwner]
        public async Task KeywordAsync(string keyword)
        {

        }
    }
}
