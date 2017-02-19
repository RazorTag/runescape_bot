using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public static class ScreenScraper
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPosition(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPosition(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public static void RightMouseClick(int xpos, int ypos)
        {
            SetCursorPosition(xpos, ypos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
        }

        public static Process GetOSBuddy(StartParams startParams, ref string loadError)
        {
            string windowName = "OSBuddy";
            string username = startParams.username;
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.MainWindowTitle.Contains(windowName))
                    {
                        if (String.IsNullOrEmpty(username))
                        {
                            return process;
                        }
                        else    //verify that the username matches that specified by the user
                        {
                            if (process.MainWindowTitle.Contains(username))
                            {
                                return process;
                            }
                        }
                    }
                }
            }
            if (String.IsNullOrEmpty(username))
            {
                loadError = "No OSBuddy client found";
            }
            else
            {
                loadError = "No OSBuddy client found with username " + startParams.username;
            }
            
            return null;    //no suitable OSBuddy client found
        }
    }
}
