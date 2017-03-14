using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Targets the lesser demon trapped in the Wizards' Tower
    /// </summary>
    public class LesserDemon : BotProgram
    {
        private const int maxDemonSpawnTime = 28000;    //max possible lesser demon spawn time in milliseconds
        private static ColorRange LesserDemonSkin;
        private static ColorRange LesserDemonHorn;

        /// <summary>
        /// Count of the number of consecutive prior frames where no demon has been found
        /// </summary>
        private int missedDemons;

        /// <summary>
        /// Theminimum required screen size for a lesser demon
        /// </summary>
        private double minDemonSize;


        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            minDemonSize = 0.0005;
        }

        protected override void Run()
        {
            ////test code to save mask pictures
            //ReadWindow();
            //bool[,] skinPixels = ColorFilter(LesserDemonSkin);
            //EraseClientUIFromMask(ref skinPixels);
            //TestMask(LesserDemonSkin, "Skin", skinPixels);
            //bool[,] hornPixels = ColorFilter(LesserDemonHorn);
            //EraseClientUIFromMask(ref hornPixels);
            //TestMask(LesserDemonHorn, "horn", hornPixels);
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override bool Execute()
        {
            ReadWindow();   //Read the game window color values into Bitmap and ColorArray

            if (Bitmap != null)     //Make sure the read is successful before using the bitmap values
            {
                int xOffset, yOffset, maxOffset;
                bool[,] skinPixels = ColorFilter(LesserDemonSkin);
                EraseClientUIFromMask(ref skinPixels);
                Blob demon = ImageProcessing.BiggestBlob(skinPixels);
                if (demon == null) { return true; }

                Point demonCenter = demon.Center;
                double cloveRange = 2 * Math.Sqrt(demon.Size);

                if (MinimumSizeMet(demon) && ClovesWithinRange(demonCenter, cloveRange))
                {
                    maxOffset = (int) (0.05 * cloveRange);
                    xOffset = RNG.Next(-maxOffset, maxOffset + 1);
                    yOffset = RNG.Next(-maxOffset, maxOffset + 1);
                    LeftClick(demonCenter.X, demonCenter.Y);
                    missedDemons = 0;
                    minDemonSize = ArtifactSize(demon) / 2.0;
                }
                else
                {
                    missedDemons++;
                }

                if (missedDemons * FrameTime > maxDemonSpawnTime)
                {
                    minDemonSize /= 2.0;
                    missedDemons = 0;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if a demon blob meets the minimum size requirement
        /// </summary>
        /// <param name="demon"></param>
        /// <returns></returns>
        private bool MinimumSizeMet(Blob demon)
        {
            double demonScreenSize = ArtifactSize(demon);
            if (demonScreenSize > minDemonSize)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if there are enough cloves close enough to the demon
        /// </summary>
        /// <param name="demonCenter"></param>
        /// <param name="cloveRange"></param>
        /// <returns></returns>
        private bool ClovesWithinRange(Point demonCenter, double cloveRange)
        {
            int requiredCloves = 3;
            int clovesToCheck = 8;
            int clovesFound = 0;
            bool[,] hornPixels = ColorFilter(LesserDemonHorn);
            EraseClientUIFromMask(ref hornPixels);
            List<Blob> demonCloves = Blob.SortBlobs(ImageProcessing.FindBlobs(hornPixels));
            clovesToCheck = Math.Min(demonCloves.Count, 8);

            for (int i = 0; i < clovesToCheck; i++)
            {
                if (demonCloves[i].DistanceTo(demonCenter) < cloveRange)
                {
                    clovesFound++;
                }

                if (clovesFound >= requiredCloves)
                {
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Colors the pixels where the given feature was found in Bitmap.
        /// Does not modify Bitmap. Creates a copy and returns the copy with masking applied.
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="testColor"></param>
        /// <returns></returns>
        private void TestMask(ColorRange bodyPart, string saveName, bool[,] mask)
        {
            Bitmap redBitmap = (Bitmap) Bitmap.Clone();
            Bitmap greenBitmap = (Bitmap) Bitmap.Clone();
            Bitmap blueBitmap = (Bitmap) Bitmap.Clone();
            Bitmap bitmap = (Bitmap) Bitmap.Clone();
            Color pixel;

            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    pixel = Bitmap.GetPixel(x, y);

                    //make red bitmap
                    if (pixel.R < bodyPart.DarkestColor.R)
                    {
                        redBitmap.SetPixel(x, y, Color.Black);
                    }
                    else if (pixel.R > bodyPart.LightestColor.R)
                    {
                        redBitmap.SetPixel(x, y, Color.White);
                    }

                    //make green bitmap
                    if (pixel.G < bodyPart.DarkestColor.G)
                    {
                        greenBitmap.SetPixel(x, y, Color.Black);
                    }
                    else if (pixel.G > bodyPart.LightestColor.G)
                    {
                        greenBitmap.SetPixel(x, y, Color.White);
                    }

                    //make blue bitmap
                    if (pixel.B < bodyPart.DarkestColor.B)
                    {
                        blueBitmap.SetPixel(x, y, Color.Black);
                    }
                    else if (pixel.B > bodyPart.LightestColor.B)
                    {
                        blueBitmap.SetPixel(x, y, Color.White);
                    }

                    //make combined bitmap
                    if (!mask[x, y])
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            string directory = "C:\\Projects\\RunescapeBot\\test_pictures\\mask_tests\\";
            ScreenScraper.SaveImageToFile(redBitmap, directory + saveName + "_RedMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(greenBitmap, directory + saveName + "_GreenMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(blueBitmap, directory + saveName + "_BlueMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(bitmap, directory + saveName + "_TotalMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(Bitmap, directory + "Original.jpg", ImageFormat.Jpeg);
        }

        /// <summary>
        /// Sets the reference colors for the lesser demon's parts if they haven't been set already
        /// </summary>
        private static void GetReferenceColors()
        {
            if (LesserDemonSkin == null) { GetSkinColor(); }
            if (LesserDemonHorn == null) { GetHornColor(); }
        }

        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        private static void GetSkinColor()
        {
            Color dark = Color.FromArgb(30, 2, 0);
            Color light = Color.FromArgb(99, 39, 28);
            LesserDemonSkin = new ColorRange(dark, light);
        }

        /// <summary>
        /// Creates the color range to represent a lesser demon's horns, hoofs, and tail spike
        /// </summary>
        /// <returns></returns>
        private static void GetHornColor()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(12, 12, 12);
            LesserDemonHorn = new ColorRange(dark, light);
        }
    }
}
