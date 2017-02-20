using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Base class for bot programs that handles starting and stopping of bot programs
    /// Implement Run in a derived class to tell a bot program what to do
    /// </summary>
    public class BotProgram
    {
        protected ArrayList runningBots;
        protected Thread runThread;
        protected string loadError;

        /// <summary>
        /// Process to which this bot program is attached
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Time when the bot program should cease execution
        /// </summary>
        public DateTime RunUntil { get; set; }

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        public BotProgram(StartParams startParams)
        {
            Process = ScreenScraper.GetOSBuddy(startParams, ref loadError);
            RunUntil = startParams.RunUntil;
            Iterations = startParams.Iterations;
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        /// <param name="runningBots"></param>
        /// <param name="iterations"></param>
        public void Start(ArrayList runningBots)
        {
            if (!String.IsNullOrEmpty(loadError))
            {
                MessageBox.Show(loadError);
                return;
            }

            this.runningBots = runningBots;
            if (ProcessExists(Process)) {
                MessageBox.Show("A bot is already running for the selected process.");
                return;
            }
            runningBots.Add(this);

            runThread = new Thread(Run);
            Run();
        }

        /// <summary>
        /// Contains the logic that determines what an implemented bot program does
        /// <param name="timeout">length of time after which the bot program should quit</param>
        /// </summary>
        protected virtual void Run()
        {
            return;
        }

        /// <summary>
        /// Removes this bot program from the list of running programs
        /// </summary>
        protected void Done()
        {
            runningBots.Remove(this);
        }

        /// <summary>
        /// Forcefully stops execution of a bot program
        /// </summary>
        public void Stop()
        {
            runThread.Abort();
            Done();
        }

        /// <summary>
        /// Checks the processes attached to running bot programs for one that matches the given process
        /// </summary>
        /// <param name="newProcess"></param>
        /// <returns></returns>
        private bool ProcessExists(Process newProcess)
        {
            foreach (BotProgram bot in runningBots)
            {
                if (bot.Process.MainWindowHandle == newProcess.MainWindowHandle)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
