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
        /// The minimum required proportion of screen for a lesser demon
        /// </summary>
        private double minDemonSize;


        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            minDemonSize = 0.001;
        }

        protected override void Run()
        {
            //test code to save mask pictures
            //ReadWindow();
            //bool[,] skinPixels = ColorFilter(LesserDemonSkin);
            //EraseClientUIFromMask(ref skinPixels);
            //TestMask(LesserDemonSkin, "Skin", skinPixels);
            //bool[,] hornPixels = ColorFilter(LesserDemonHorn);
            //EraseClientUIFromMask(ref hornPixels);
            //TestMask(LesserDemonHorn, "horn", hornPixels);
            //TestSkinAndHorn(skinPixels, hornPixels);
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override bool Execute()
        {
            int xOffset, yOffset, maxOffset;
            bool[,] skinPixels = ColorFilter(LesserDemonSkin);
            if (StopFlag) { return false; }   //quit immediately if the stop flag has been raised
            EraseClientUIFromMask(ref skinPixels);
            Blob demon = ImageProcessing.BiggestBlob(skinPixels);
            if (StopFlag) { return false; }   //quit immediately if the stop flag has been raised
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

            if (missedDemons * RunParams.FrameTime > maxDemonSpawnTime)
            {
                minDemonSize /= 2.0;
                missedDemons = 0;
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
        /// Colors the pixels where either the skin of the horn were found in Bitmap.
        /// Does not modify Bitmap. Creates a copy and returns the copy with masking applied.
        /// </summary>
        /// <param name="skin"></param>
        /// <param name="horn"></param>
        private void TestSkinAndHorn(bool[,] skin, bool[,] horn)
        {
            Bitmap bitmap = (Bitmap) Bitmap.Clone();

            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    if (!skin[x, y] && !horn[x, y])
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }
            string directory = "C:\\Projects\\RunescapeBot\\test_pictures\\mask_tests\\";
            ScreenScraper.SaveImageToFile(bitmap, directory + "SkinAndHorn.jpg", ImageFormat.Jpeg);
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
            Bitmap hueBitmap = (Bitmap) Bitmap.Clone();
            Bitmap saturationBitmap = (Bitmap)Bitmap.Clone();
            Bitmap brightnessBitmap = (Bitmap)Bitmap.Clone();
            Bitmap bitmap = (Bitmap) Bitmap.Clone();
            Color pixel;

            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    pixel = ColorArray[x, y];

                    if ((bodyPart.DarkestColor != null) && (bodyPart.LightestColor != null))
                    {
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
                    }

                    if (bodyPart.HSBRange != null)
                    {
                        //make hue bitmap
                        if (!bodyPart.HSBRange.HueInRange(pixel))
                        {
                            hueBitmap.SetPixel(x, y, Color.White);
                        }

                        //make saturation bitmap
                        if (pixel.GetSaturation() < bodyPart.HSBRange.MinimumSaturation)
                        {
                            saturationBitmap.SetPixel(x, y, Color.Black);
                        }
                        else if (pixel.GetSaturation() > bodyPart.HSBRange.MaximumSaturation)
                        {
                            saturationBitmap.SetPixel(x, y, Color.White);
                        }

                        //make brightness bitmap
                        if (pixel.GetBrightness() < bodyPart.HSBRange.MinimumBrightness)
                        {
                            brightnessBitmap.SetPixel(x, y, Color.Black);
                        }
                        else if (pixel.GetBrightness() > bodyPart.HSBRange.MaximumBrightness)
                        {
                            brightnessBitmap.SetPixel(x, y, Color.White);
                        }
                    }

                    //make combined bitmap
                    if (!mask[x, y])
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            string directory = "C:\\Projects\\RunescapeBot\\test_pictures\\mask_tests\\";
            ScreenScraper.SaveImageToFile(redBitmap, directory + saveName + "_ColorRedMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(greenBitmap, directory + saveName + "_ColorGreenMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(blueBitmap, directory + saveName + "_ColorBlueMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(hueBitmap, directory + saveName + "_HSBHueMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(saturationBitmap, directory + saveName + "_HSBSaturationMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(brightnessBitmap, directory + saveName + "_HSBBrightnessMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(bitmap, directory + saveName + "_TotalMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.SaveImageToFile(Bitmap, directory + "Original.jpg", ImageFormat.Jpeg);
        }

        public static int SizeOfMatch(bool[,] mask)
        {
            int matches = 0;

            for (int x = 0; x < mask.GetLength(0); x++)
            {
                for (int y = 0; y < mask.GetLength(1); y++)
                {
                    if (mask[x, y])
                    {
                        matches++;
                    }
                }
            }
            return matches;
        }

        /// <summary>
        /// Sets the reference colors for the lesser demon's parts if they haven't been set already
        /// </summary>
        private static void GetReferenceColors()
        {
            LesserDemonSkin = ColorFilters.LesserDemonSkin();
            LesserDemonHorn = ColorFilters.LesserDemonHorn();
        }
    }
}
