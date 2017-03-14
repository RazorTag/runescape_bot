using RunescapeBot.BotPrograms.Debug;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
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
        protected Process RSClient { get; set; }

        /// <summary>
        /// Thread in which the run method is executed
        /// </summary>
        protected Thread RunThread { get; set; }

        /// <summary>
        /// Time when the bot program should cease execution
        /// </summary>
        protected DateTime RunUntil { get; set; }

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        protected int Iterations { get; set; }

        /// <summary>
        /// Time between iteration in milliseconds
        /// </summary>
        protected int FrameTime { get; set; }

        /// <summary>
        /// If set to true, slightly varies the wait time between executions
        /// </summary>
        protected bool RandomizeFrames { get; set; }

        /// <summary>
        /// Time at which to end execution if it hasn't ended already
        /// </summary>
        protected DateTime EndTime { get; set; }

        /// <summary>
        /// Stores a bitmap of the client window
        /// </summary>
        protected Bitmap Bitmap { get; set; }

        /// <summary>
        /// Stores a Color array of the client window
        /// </summary>
        protected Color[,] ColorArray { get; set; }

        /// <summary>
        /// Stores the simulated user actions that have been taken by this program (click, etc)
        /// </summary>
        protected BotProgramActions BotActions { get; set; }

        /// <summary>
        /// Stock random number generator
        /// </summary>
        protected Random RNG { get; set; }



        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        public BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(startParams, out loadError);
            RunUntil = startParams.RunUntil;
            Iterations = startParams.Iterations;
            FrameTime = startParams.FrameTime;
            RandomizeFrames = startParams.RandomizeFrames;
            EndTime = startParams.EndTime;
            BotActions = new BotProgramActions();
            RNG = new Random();
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
            
        }

        /// <summary>
        /// Begins iterating after Run is called. Called for the number of iterations specified by the user.
        /// Is only called if both Iterations and FrameRate are specified.
        /// </summary>
        private void Iterate()
        {
            int randomFrameOffset, randomFrameTime;

            if (Iterations == 0)
            {
                Iterations = int.MaxValue;
            }

            if (RandomizeFrames)
            {
                randomFrameOffset = (int) (0.1 * FrameTime);
            }
            else
            {
                randomFrameOffset = 0;
            }

            for (int i = 0; i < Iterations; i++)
            {
                if (DateTime.Now > EndTime)
                {
                    break; //quit if we have gone over our time limit
                }

                Stopwatch watch = Stopwatch.StartNew();
                if (!Execute()) //quit by an override Execute method
                {
                    break;
                }

                randomFrameTime = FrameTime + RNG.Next(-randomFrameOffset, randomFrameOffset + 1);
                randomFrameTime = Math.Max(0, randomFrameTime);
                watch.Stop();
                if (watch.ElapsedMilliseconds < randomFrameTime)
                {
                    Thread.Sleep(FrameTime - (int)watch.ElapsedMilliseconds);
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
            if (ColorArray == null)
            {
                return null;
            }

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
        /// Determines the portion of the screen taken up by a blob
        /// </summary>
        /// <param name="artifact"></param>
        /// <returns></returns>
        protected double ArtifactSize(Blob artifact)
        {
            double screenSize = Bitmap.Size.Width * Bitmap.Size.Height;
            return artifact.Size / screenSize;
        }

        /// <summary>
        /// Wrapper for ScreenScraper.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void LeftClick(int x, int y, bool preserveMousePosition = false)
        {
            MouseActions.LeftMouseClick(x, y, RSClient, preserveMousePosition);
            BotActions.SaveClick(x, y);
        }

        /// <summary>
        /// Wrapper for ScreenScraper.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y, bool preserveMousePosition = false)
        {
            MouseActions.RightMouseClick(x, y, RSClient, preserveMousePosition);
            BotActions.SaveClick(x, y, true);
        }

        /// <summary>
        /// Sets the pixels in client UI areas to false.
        /// This should only be used with untrimmed images.
        /// </summary>
        /// <param name="mask"></param>
        protected void EraseClientUIFromMask(ref bool[,] mask)
        {
            int width = mask.GetLength(0);
            int height = mask.GetLength(1);

            //erase chat box
            for (int x = 0; x < 519; x++)
            {
                for (int y = height - 159; y < height; y++)
                {
                    mask[x, y] = false;
                }
            }

            //erase inventory
            for (int x = width - 241; x < width; x++)
            {
                for (int y = height - 336; y < height; y++)
                {
                    mask[x, y] = false;
                }
            }

            //erase minimap
            for (int x = width - 211; x < width; x++)
            {
                for (int y = 0; y < 192; y++)
                {
                    mask[x, y] = false;
                }
            }
        }
    }
}
