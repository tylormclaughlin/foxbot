using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace foxbot
{
    public class MonitorList
    {
        private ConcurrentHashSet<SocketUserMessage> monitorList = new ConcurrentHashSet<SocketUserMessage>();
        private object _sync = new object();

        //Getter
        public ConcurrentHashSet<SocketUserMessage> GetMonitoredMessages()
        {
            return new ConcurrentHashSet<SocketUserMessage>(monitorList);
        }

        //Setter
        public void AddMonitoredMessage(SocketUserMessage msg)
        {
            
        }

        public void DeleteMonitoredMessage(SocketUserMessage msg)
        {
                    
        }
    }
}
