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
        private static ConcurrentHashSet<RestUserMessage> messageList = new ConcurrentHashSet<RestUserMessage>();

        //Getter
        public static ConcurrentHashSet<RestUserMessage> GetMonitoredMessages()
        {
            return new ConcurrentHashSet<RestUserMessage>(messageList);
        }

        //Setter
        public static string AddMonitoredMessage(RestUserMessage msg)
        {
            string result = "success";
            try
            {
                messageList.TryAdd(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = e.Message;
            }

            return result;
        }

        public static void DeleteMonitoredMessage(RestUserMessage msg)
        {
                    
        }
    }
}
