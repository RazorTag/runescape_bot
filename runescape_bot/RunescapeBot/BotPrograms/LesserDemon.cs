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
            Bitmap bitmap;

            bitmap = ReadWindow();
            if (bitmap != null)     //Make sure the read is successful before using the bitmap values
            {
                //LeftClick(1000, 500);
                //ScreenScraper.WriteBitmapToFile(bitmap, "C:\\Projects\\RunescapeBot\\test_pictures\\saveBitmap.jpg", ImageFormat.Jpeg);
            }

            Done();
        }
    }
}
