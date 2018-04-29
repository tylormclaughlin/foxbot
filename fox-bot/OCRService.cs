using Discord.WebSocket;
using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace foxbot
{
    public class OCRService
    {
        public OCRService(DiscordSocketClient client)
        {
            //stuff
        }

        public void GetFile(string url, string name)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, name);
            }
        }

        public void DeleteFile(string name)
        {
            if (File.Exists(name))
            {
                File.Delete(name);
            }
        }

        public string ProcessImage(string url)
        {
            var ocr = new AutoOcr();
        }
    }
}
