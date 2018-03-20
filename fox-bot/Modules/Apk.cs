using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using System.Linq;

namespace foxbot.Modules
{
    public class Apk : ModuleBase<SocketCommandContext>
    {
        [Command("apk")]
        public async Task ApkAsync(string versionNumber = "")
        {
            var feed = FeedReader.Read("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/").Items;
            bool validVersion = false;

            //Need to do this because the FeedReader.Items collection doesn't have an indexer defined, making accessing specific items impossible without putting them in a list first.
            foreach(var item in feed)
            {
                if (versionNumber != "")
                {
                    if (item.Title.Contains(versionNumber))
                    {
                        validVersion = true;
                        await ReplyAsync(item.Title + " - " + item.Link.Replace("-release/", "-android-apk-download/download/"));
                        break;
                    }
                }
                else
                {
                    await ReplyAsync(item.Title + " - " + item.Link.Replace("-release/", "-android-apk-download/download/"));
                    break;
                }
            }

            if (!validVersion && versionNumber != "")
            {
                await ReplyAsync("Sorry, I could find that version.");
            }
        }
    }
}
