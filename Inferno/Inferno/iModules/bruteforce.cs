using System;
using System.Net;
using MinimalisticTelnet;
using System.IO;

namespace Inferno
{
    internal class bruteforce
    {
        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        // Brute FTP
        public static void FTP(string host, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://" + host + "/");
            request.Credentials = new NetworkCredential(username, password);
            try
            {
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.GetResponse();

                core.Exit("[FTP] Logged in", output);
            }
            catch
            {
                output.error = true;
                core.Exit("[FTP] Failed to login", output);
            }
        }

        // Brute SSH
        public static void SSH(string host, string username, string password)
        {
            var SSH = new Renci.SshNet.SshClient(host, username, password);
            try
            {
                SSH.Connect();
                SSH.Disconnect();
                core.Exit("[SSH] Logged in", output);
            }
            catch
            {
                output.error = true;
                core.Exit("[SSH] Failed to login", output);
            }
            finally { SSH.Dispose(); }
        }

        // Brute Telnet
        public static void Telnet(string host, string username, string password, int port = 23)
        {
            string s, prompt = "";
            // host:port
            if (host.Contains(":")) {
                port = Int32.Parse(host.Split(':')[1]);
                host = host.Split(':')[0];
            }
            try
            {
                TelnetConnection tc = new TelnetConnection(host, port);
                s = tc.Login(username, password, 250);
                prompt = s.TrimEnd();
                prompt = s.Substring(prompt.Length - 1, 1);
            } catch
            {
                output.error = true;
                core.Exit("[TELNET] Failed to connect", output);
                
            }
            // Check output
            if (prompt != "$" && prompt != ">" && prompt != "#")
            {
                output.error = true;
                core.Exit("[TELNET] Failed to login", output);
            }
            else
            {
                core.Exit("[TELNET] Logged in", output);
            }
        }



    }
}
