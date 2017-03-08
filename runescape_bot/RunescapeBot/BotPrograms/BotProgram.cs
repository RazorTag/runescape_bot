using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        protected List<BotProgram> runningBots;
        protected string loadError;

        /// <summary>
        /// Process to which this bot program is attached
        /// </summary>
        public Process RSClient { get; set; }

        /// <summary>
        /// Thread in which the run method is executed
        /// </summary>
        public Thread RunThread { get; set; }

        /// <summary>
        /// Time when the bot program should cease execution
        /// </summary>
        public DateTime RunUntil { get; set; }

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Rate at which to iterate in units of Hz
        /// </summary>
        public double FrameRate { get; set; }

        /// <summary>
        /// Time at which to end execution if it hasn't ended already
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Stores a bitmap of the client window
        /// </summary>
        protected Bitmap Bitmap { get; set; }

        /// <summary>
        /// Stores a Color array of the client window
        /// </summary>
        protected Color[,] ColorArray { get; set; }

        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        public BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(startParams, out loadError);
            RunUntil = startParams.RunUntil;
            Iterations = startParams.Iterations;
            FrameRate = startParams.FrameRate;
            EndTime = startParams.EndTime;
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        /// <param name="runningBots"></param>
        /// <param name="iterations"></param>
        public void Start(List<BotProgram> runningBots)
        {
            if (!String.IsNullOrEmpty(loadError))
            {
                MessageBox.Show(loadError);
                return;
            }

            this.runningBots = runningBots;
            if (ProcessExists(RSClient)) {
                MessageBox.Show("A bot is already running for the selected process.");
                return;
            }
            runningBots.Add(this);

            RunThread = new Thread(Process);
            RunThread.Start();
        }

        /// <summary>
        /// Handles the sequential calling of the methods used to do bot work
        /// </summary>
        private void Process()
        {
            Run();
            Iterate();
            Done();
        }

        /// <summary>
        /// Contains the logic that determines what an implemented bot program does
        /// Executes before beginning iteration
        /// <param name="timeout">length of time after which the bot program should quit</param>
        /// </summary>
        protected virtual void Run()
        {
            return;
        }

        /// <summary>
        /// Begins iterating after Run is called. Called for the number of iterations specified by the user.
        /// Is only called if both Iterations and FrameRate are specified.
        /// </summary>
        private void Iterate()
        {
            int iterationTime = (int)(1000 * (1 / FrameRate));   //iteration refresh time in milliseconds
            if (Iterations == 0)
            {
                Iterations = int.MaxValue;
            }

            for (int i = 0; i < Iterations; i++)
            {
                if (DateTime.Now > EndTime)
                {
                    break; //quit if we have gone over our time limit
                }

                Stopwatch watch = Stopwatch.StartNew();
                if (!Execute())
                {
                    break;
                }
                watch.Stop();
                if (watch.ElapsedMilliseconds < iterationTime)
                {
                    Thread.Sleep(iterationTime - (int)watch.ElapsedMilliseconds);
                }
            }

            return;
        }

        /// <summary>
        /// A single iteration. Return false to stop execution.
        /// </summary>
        protected virtual bool Execute()
        {
            return false;
        }

        /// <summary>
        /// Removes this bot program from the list of running programs
        /// </summary>
        private void Done()
        {
            runningBots.Remove(this);
        }

        /// <summary>
        /// Forcefully stops execution of a bot program
        /// </summary>
        public void Stop()
        {
            RunThread.Abort();
            Done();
        }

        /// <summary>
        /// Creates a boolean array to represent that match 
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        protected bool[,] ColorFilter(ColorRange artifactColor)
        {
            return ImageProcessing.ColorFilter(ColorArray, artifactColor);
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
                if (bot.RSClient.MainWindowHandle == newProcess.MainWindowHandle)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        protected bool ReadWindow()
        {
            Bitmap = ScreenScraper.CaptureWindow(RSClient);
            ColorArray = ScreenScraper.GetRGB(Bitmap);

            return Bitmap != null;
        }

        /// <summary>
        /// Retrieve the color of a single pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Color GetPixel(int x, int y)
        {
            return ColorArray[x, y];
        }

        /// <summary>
        /// Wrapper for ScreenScraper.LeftMouseCLick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void LeftClick(int x, int y)
        {
            ScreenScraper.LeftMouseClick(x, y, RSClient);
        }

        /// <summary>
        /// Wrapper for ScreenScraper.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y)
        {
            ScreenScraper.RightMouseClick(x, y, RSClient);
        }
    }
}
