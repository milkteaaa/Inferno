using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Inferno
{
    internal class network
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();



        // Download file
        public static void downloadFile(string url, string foutput = "")
        {
            if (string.IsNullOrEmpty(foutput))
            {
                foutput = url.Split('/')[url.Split('/').Length - 1];
            }
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, foutput);

            output.filename = foutput;
            core.Exit("File downloaded", output);
        }

        // Upload file to Anonfile.com
        public static void uploadFile(string filename)
        {
            // Check
            if (!File.Exists(filename))
            {
                output.error = true;
                core.Exit("File " + filename + " not found!", output, 1);
            }
            // POST request
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            byte[] responseBytes = client.UploadFile(@"https://api.anonfile.com/upload", "POST", filename);
            string response = System.Text.Encoding.ASCII.GetString(responseBytes);
            client.Dispose();
            // Parse json
            dynamic json = JObject.Parse(response);
            // Check upload status
            bool status = json["status"];
            if (!status)
            {
                output.error = true;
                output.msg = json.error.message;
                output.type = json.error.type;
                output.code = json.error.code;
                core.Exit("Failed upload file", output);
            } else
            {
                output.url = json.data.file.url.full;
                core.Exit("File uploaded successfully!", output);
            }
        }

        // Whois
        public static void Whois(string ip = "")
        {
            // Url
            string url = @"http://ip-api.com/json/" + ip;
            // GET request
            WebClient client = new WebClient();
            string response = client.DownloadString(url);
            // Parse json
            dynamic json = JObject.Parse(response);
            output.whois = json;
            core.Exit("Whois data received!", output);
        }

        // Geoplugin
        public static void Geoplugin(string ip = "")
        {
            // Url
            string url = @"http://www.geoplugin.net/json.gp?ip=" + ip;
            // GET request
            WebClient client = new WebClient();
            string response = client.DownloadString(url);
            // Parse json
            dynamic json = JObject.Parse(response);
            output.whois = json;
            core.Exit("Geo data received!", output);
        }

        // VirusTotal detection
        public static void VirusTotal(string filename)
        {
            // Url
            string vt_api = "https://www.virustotal.com/ui/search?query=";
            // Check file
            if (!File.Exists(filename))
            {
                output.error = true;
                core.Exit("Failed to check VirusTotal, " + filename + " not found!", output, 1);
            }
            else
            {
                // Get file md5 hash
                var md5 = System.Security.Cryptography.MD5.Create();
                var stream = File.OpenRead(filename);
                byte[] checksum = md5.ComputeHash(stream);
                var hash = BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
                // GET request
                WebClient client = new WebClient();
                string response = client.DownloadString(vt_api + hash);
                // Parse json
                dynamic json = JObject.Parse(response);
                try
                {
                    dynamic result = new System.Dynamic.ExpandoObject();
                    result.virustotal = json["data"][0]["attributes"]["last_analysis_stats"];
                    output.malicious = result.virustotal["malicious"];
                    output.suspicious = result.virustotal["suspicious"];
                    output.harmless = result.virustotal["harmless"];
                    output.undetected = result.virustotal["undetected"];

                } catch (ArgumentOutOfRangeException) {
                    output.error = true;
                    core.Exit("This file has never been uploaded to VirusTotal", output);
                }
                output.hash = hash;
                output.report = "https://www.virustotal.com/gui/file/" + hash + "/detection";
                core.Exit("VirusTotal data received!", output);
            }
        }

        // BSSID get info
        public static void BssidInfo(string bssid)
        {
            // Url
            string url = @"https://api.mylnikov.org/geolocation/wifi?bssid=" + bssid;
            // GET request
            WebClient client = new WebClient();
            string response = client.DownloadString(url);
            // Parse json
            dynamic json = JObject.Parse(response);
            // Check results
            if (json.result == 200)
            {
                output.bssid = json.data;
                core.Exit("BSSID info received!", output);
            } else {
                output.error = true;
                core.Exit(json.desc.ToString(), output);
            }
        }

        // Get router BSSID
        public static void BssidGet()
        {
            
            // Get BSSID
            IPAddress dst = GetDefaultGateway();
            output.router_ip = dst.ToString();
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            if (SendARP(BitConverter.ToInt32(dst.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
            {
                output.error = true;
                core.Exit("Send ARP failed", output, 3);
            }

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");
            string bssid = string.Join(":", str);

            output.bssid = bssid;
            core.Exit("Router BSSID received!", output);
                
        }

        // Get default gateway
        private static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }

        // Check target port
        private static bool portIsOpen(string target, int port)
        {
            TcpClient tcpClient = new TcpClient();
            try {
                tcpClient.Connect(target, port);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        // Check target port
        public static void PortIsOpen(string target, string port)
        {
            if (portIsOpen(target, Int32.Parse(port)))
            {
                output.portIsOpen = true;
                core.Exit("Port " + port + " is open!", output);
            } else {
                output.portIsOpen = false;
                core.Exit("Port " + port + " is closed!", output);
            }
        }

        // Scan wlan
        public static void WlanScanner(int to)
        {
            string gateway="";
            try { gateway = GetDefaultGateway().ToString(); }
            catch (NullReferenceException)
            {
                output.error = true;
                core.Exit("Not connected to WIFI network.", output, 3);
            }
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            string ip, host, mac;
            string[] s = gateway.Split('.');
            string target = s[0] + "." + s[1] + "." + s[2] + ".";
            List<Dictionary<string, string>> scanResult = new List<Dictionary<string, string>>();
            for (int i = 1; i < to; i++)
            {
                
                ip = target + i.ToString();
                Ping ping = new Ping();
                PingReply reply = ping.Send(ip, 10);

                if (reply.Status == IPStatus.Success)
                {
                    IPAddress addr = IPAddress.Parse(ip);
                    // Get hostname
                    try { host = Dns.GetHostEntry(addr).HostName;
                    } catch { host = "unknown"; }
                    // Get mac
                    if (SendARP(BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0) {
                        mac = "unknown";
                    } else {
                        string[] v = new string[(int)macAddrLen];
                        for (int j = 0; j < macAddrLen; j++)
                            v[j] = macAddr[j].ToString("x2");
                        mac = string.Join(":", v);
                    }
                    // Add to dictonary
                    Dictionary<string, string> result = new Dictionary<string, string>
                    {
                        ["addr"] = ip,
                        ["host"] = host,
                        ["mac"]  = mac
                    };
                    scanResult.Add(result);
                }
            }

            output.scanResult = scanResult;
            core.Exit("Local network scanned from 1 to " + to, output);
        }
       
        // Agents
        private static string[] user_agents = new string[]
        {
            "Mozilla/5.0 (Windows NT 10.0; ) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4086.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; rv:76.0) Gecko/20100101 Firefox/76.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/82.0.4062.3 Safari/537.36 OPR/69.0.3623.0 (Edition developer)",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/82.0.4080.0 Safari/537.36 Edg/82.0.453.0",
            "Mozilla/5.0 (Linux; Android 8.1.0; LM-Q710(FGN)) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.93 Mobile Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Safari/605.1.15",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.5 Mobile/15E148 Snapchat/10.77.0.54 (like Safari/604.1)"
        };
        // Random user-agent
        public static string randomAgent()
        {
            Random random = new Random();
            int rand = random.Next(0, user_agents.Length);
            return user_agents[rand];
        }


    }
}
