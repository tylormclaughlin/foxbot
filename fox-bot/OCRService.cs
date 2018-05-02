using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using Tesseract;
using System.Net.Http;
using System.Threading.Tasks;

namespace foxbot
{
    public class OCRService
    {
        public OCRService(DiscordSocketClient client)
        {
            //stuff
        }

        public async Task<Bitmap> GetImage(string url)
        {
            using (var client = new HttpClient())
            {
                return (Bitmap)Image.FromStream(await client.GetStreamAsync(url));
            }
        }

        public void DeleteImage(string name)
        {
            if (File.Exists(name))
            {
                File.Delete(name);
            }
        }

        //public string ProcessImage(string url)
        //{
        //    using (var ocr = new TesseractEngine(@"./tessdata", "eng"))
        //    {
        //        using (var img = PixConverter.ToPix(GetImage(url)))
        //        {
        //            //stuff
        //        }
        //    }
        //}
    }
}
