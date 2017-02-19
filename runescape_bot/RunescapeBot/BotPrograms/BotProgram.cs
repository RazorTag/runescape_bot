using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class BotProgram
    {
        protected ArrayList runningBots;
        protected Process process;
        protected Thread runThread;
        protected string loadError;


        public BotProgram(StartParams startParams)
        {
            process = ScreenScraper.GetOSBuddy(startParams, ref loadError);
        }
       
        public void Start(ArrayList runningBots, int iterations = 1)
        {
            if (!String.IsNullOrEmpty(loadError))
            {
                MessageBox.Show(loadError);
                return;
            }

            this.runningBots = runningBots;
            if (ProcessExists(process)) {
                MessageBox.Show("A bot is already running for the selected process.");
                return;
            }
            runningBots.Add(this);

            runThread = new Thread(Run);
            Run();
        }

        protected virtual void Run()
        {
            return;
        }

        protected void Done()
        {
            runningBots.Remove(this);
        }

        public void Stop()
        {
            runThread.Abort();
            Done();
        }

        private bool ProcessExists(Process newProcess)
        {
            foreach (BotProgram bot in runningBots)
            {
                if (bot.process.MainWindowHandle == newProcess.MainWindowHandle)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
