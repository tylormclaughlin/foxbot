using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foxbot.Modules
{
    public class Apk : ModuleBase<SocketCommandContext>
    {
        [Command("apk")]
        public async Task ApkAsync(string versionNumber)
        {
            versionNumber = versionNumber.Replace(".", "-");

            await ReplyAsync($"https://www.apkmirror.com/apk/niantic-inc/pokemon-go/pokemon-go-{versionNumber}-android-apk-download/download/");
        }
    }
}
