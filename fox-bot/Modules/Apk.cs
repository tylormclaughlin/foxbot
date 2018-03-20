using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace foxbot.Modules
{
    public class Apk : ModuleBase<SocketCommandContext>
    {
        [Command("apk")]
        public async Task ApkAsync(string versionNumber = "")
        {
            var feed = FeedReader.Read("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/");

            if (versionNumber == "")
            {
                foreach(var item in feed.Items)
                {
                    Console.WriteLine(item.Title + " - " + item.Link);
                }
            }


            //versionNumber = versionNumber.Replace(".", "-");

            //await ReplyAsync($"https://www.apkmirror.com/apk/niantic-inc/pokemon-go/pokemon-go-{versionNumber}-android-apk-download/download/");
        }
    }
}
