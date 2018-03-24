using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using System.Linq;
using Discord;

namespace foxbot.Modules
{
    public class Apk : ModuleBase<SocketCommandContext>
    {
        [Command("apk")]
        [Summary("This command provides a direct link to Pokemon Go APK Mirror downloads.")]
        public async Task DefaultApkAsync(string versionNumber = "")
        {
            var feed = FeedReader.Read("https://www.apkmirror.com/apk/niantic-inc/pokemon-go/feed/").Items;
            bool validVersion = false;
            string[] splits = versionNumber.Split(".");

            //Return a list of all APK versions in the feed.
            if (versionNumber == "list")
            {
                EmbedBuilder eb = new EmbedBuilder();

                foreach (FeedItem item in feed)
                {
                    eb.AddField(item.Title, item.Link.Replace("-release/", "-android-apk-download/download/"));
                }

                await ReplyAsync("", false, eb);
            }
            else
            {
                //Try to make sure given version number was in a valid format (X.XX)
                if (versionNumber != "" && (splits.Length < 2 || (splits.Length < 3 && splits[1] == "")))
                {
                    await ReplyAsync("Sorry, I don't think that's a valid version number");
                }
                else
                {
                    //Need to do this because the FeedReader.Items collection doesn't have an indexer defined, making accessing specific items impossible without putting them in a list first.
                    foreach (var item in feed)
                    {
                        if (versionNumber != "")
                        {
                            if (item.Title.Contains(versionNumber))
                            {
                                validVersion = true;
                                await ReplyAsync(item.Title + " - " + item.Link.Replace("-release/", "-android-apk-download/download/")); //Change the link to point at the direct download
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
    }
}
