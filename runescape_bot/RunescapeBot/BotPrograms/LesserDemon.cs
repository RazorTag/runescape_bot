using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1.BotPrograms
{
    /// <summary>
    /// Targets the lesser demon trapped in the Wizards' Tower
    /// </summary>
    public class LesserDemon : BotProgram
    {
        public LesserDemon(StartParams startParams) : base(startParams) { }

        /// <summary>
        /// Called once before execute
        /// </summary>
        protected override void Run()
        {
            
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override void Execute()
        {
            //use this to test visually
            //ScreenScraper.CaptureWindowToFile(RSClient, "c:\\Projects\\RunescapeBot\\test_pictures\\test.jpg", ImageFormat.Jpeg);

            Bitmap bitmap;

            //Make sure the read is successful before using the bitmap values
            if (ReadWindow(out bitmap))
            {
                ScreenScraper.LeftMouseClick(800, 500, RSClient);
            }

            Done();
        }
    }
}
