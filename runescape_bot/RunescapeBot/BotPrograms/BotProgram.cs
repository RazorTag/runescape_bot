using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Used by the bot to inform that is has completed its task
    /// </summary>
    public delegate void BotResponse();

    /// <summary>
    /// Base class for bot programs that handles starting and stopping of bot programs
    /// Implement Run in a derived class to tell a bot program what to do
    /// </summary>
    public class BotProgram
    {
        /// <summary>
        /// Error message to show the user for a start error
        /// </summary>
        private string loadError;

        /// <summary>
        /// Process to which this bot program is attached
        /// </summary>
        protected Process RSClient { get; set; }

        /// <summary>
        /// Thread in which the run method is executed
        /// </summary>
        protected Thread RunThread { get; set; }

        /// <summary>
        /// Specifies how the bot should be run
        /// </summary>
        public StartParams RunParams;

        /// <summary>
        /// Stores a bitmap of the client window
        /// </summary>
        protected Bitmap Bitmap { get; set; }

        /// <summary>
        /// Stores a Color array of the client window
        /// </summary>
        protected Color[,] ColorArray { get; set; }

        /// <summary>
        /// Stock random number generator
        /// </summary>
        protected Random RNG { get; set; }

        /// <summary>
        /// Tells anyone listening to stop at their convenience
        /// </summary>
        protected bool StopFlag { get; set; }


        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        public BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(startParams, out loadError);
            this.RunParams = startParams;
            RNG = new Random();
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        /// <param name="runningBots"></param>
        /// <param name="iterations"></param>
        public void Start()
        {
            if (!String.IsNullOrEmpty(loadError))
            {
                MessageBox.Show(loadError);
                return;
            }
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
            if (StopFlag) { return; }
        }

        /// <summary>
        /// Begins iterating after Run is called. Called for the number of iterations specified by the user.
        /// Is only called if both Iterations and FrameRate are specified.
        /// </summary>
        private void Iterate()
        {
            if (StopFlag) { return; }
            int randomFrameOffset, randomFrameTime;

            if (RunParams.Iterations == 0)
            {
                RunParams.Iterations = int.MaxValue;
            }

            if (RunParams.RandomizeFrames)
            {
                randomFrameOffset = (int) (0.6 * RunParams.FrameTime);
            }
            else
            {
                randomFrameOffset = 0;
            }

            for (int i = 0; i < RunParams.Iterations; i++)
            {
                if (DateTime.Now > RunParams.RunUntil)
                {
                    return; //quit if we have gone over our time limit
                }

                Stopwatch watch = Stopwatch.StartNew();
                ReadWindow();               //Read the game window color values into Bitmap and ColorArray
                if (StopFlag || !CheckLogIn()) { return; }   //quit immediately if the stop flag has been raised or we can't log back in

                if (Bitmap != null)     //Make sure the read is successful before using the bitmap values
                {
                    if (!Execute()) //quit by an override Execute method
                    {
                        return;
                    }
                    if (StopFlag) { return; }
                }

                randomFrameTime = RunParams.FrameTime + RNG.Next(-randomFrameOffset, randomFrameOffset + 1);
                randomFrameTime = Math.Max(0, randomFrameTime);
                watch.Stop();
                if (watch.ElapsedMilliseconds < randomFrameTime)
                {
                    Thread.Sleep(randomFrameTime - (int)watch.ElapsedMilliseconds);
                }
                if (StopFlag) { return; }
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
        /// Clean up
        /// </summary>
        private void Done()
        {
            Bitmap.Dispose();
            RunParams.TaskComplete();
        }

        /// <summary>
        /// Forcefully stops execution of a bot program
        /// </summary>
        public void Stop()
        {
            StopFlag = true;
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
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        protected bool ReadWindow()
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }
            if (StopFlag) { return false; }

            //Bitmap = ScreenScraper.CaptureWindowLegacy(RSClient);
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
        /// Wrapper for MouseActions.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void LeftClick(int x, int y)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                MouseActions.LeftMouseClick(x, y, RSClient);
            }
        }

        /// <summary>
        /// Wrapper for MouseActions.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                MouseActions.RightMouseClick(x, y, RSClient);
            }
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

            EraseFromMask(ref mask, 0, 519, height - 159, height);                  //erase chat box
            EraseFromMask(ref mask, width - 241, width, height - 336, height);      //erase inventory
            EraseFromMask(ref mask, width - 211, width, 0, 192);                    //erase minimap
        }

        /// <summary>
        /// Clears a rectangle from a boolean mask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="xMin">Inclusive</param>
        /// <param name="xMax">Exclusive</param>
        /// <param name="yMin">Inclusive</param>
        /// <param name="yMax">Exclusive</param>
        protected void EraseFromMask(ref bool[,] mask, int xMin, int xMax, int yMin, int yMax)
        {
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    mask[x, y] = false;
                }
            }
        }

        /// <summary>
        /// Determines if the user is logged out and logs him back in if he is.
        /// If the bot does not have valid login information, then it will quit.
        /// </summary>
        /// <returns>true if we are already logged in or we are able to log in, false if we can't log in</returns>
        private bool CheckLogIn()
        {
            int width = ColorArray.GetLength(0);
            Color color = ColorArray[width / 2, ScreenScraper.LOGIN_WINDOW_HEIGHT];
            if (ImageProcessing.ColorsAreEqual(color, Color.Black))
            {
                if (string.IsNullOrEmpty(RunParams.Login) || string.IsNullOrEmpty(RunParams.Password))
                {
                    return false;
                }
                else
                {
                    //log in
                }
            }

            return true;
        }
    }
}
