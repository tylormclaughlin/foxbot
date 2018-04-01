using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace foxbot
{
    public class MonitorList
    {
        private static RestUserMessage message = null;

        //Getter
        public static RestUserMessage GetMonitoredMessage()
        {
            return message;
        }

        //Setter
        public static string AddMonitoredMessage(RestUserMessage msg)
        {
            string result = "success";

            if (message != null)
            {
                result = "Sorry, another message is already being monitored. Consider using !unmonitor and trying again, or consult this bot's administrator.";
            }
            else
            {
                try
                {
                    message = msg;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = e.Message;
                }
            }

            return result;
        }

        public static void DeleteMonitoredMessage()
        {
            message = null;
        }
    }
}
