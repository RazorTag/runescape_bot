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
        private static ColorRange RuneMedHelm;
        private static ColorRange MithrilArmor;

        /// <summary>
        /// Count of the number of consecutive prior frames where no demon has been found
        /// </summary>
        private int MissedDemons;

        /// <summary>
        /// The minimum required proportion of screen for a lesser demon
        /// </summary>
        private double MinDemonSize;

        /// <summary>
        /// The last location where a demon was found. Set to (0, 0) is no demon has been found yet.
        /// </summary>
        private Point LastDemonLocation;


        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            MinDemonSize = 0.001;
        }

        protected override void Run()
        {
            //test code to save mask pictures
            ReadWindow();
            LastDemonLocation = new Point(958, 500);
            CheckDrops();
            //bool[,] helmPixels = ColorFilter(RuneMedHelm);
            //EraseClientUIFromMask(ref helmPixels);
            //TestMask(RuneMedHelm, "helm", helmPixels);
            //bool[,] mithPixels = ColorFilter(MithrilArmor);
            //EraseClientUIFromMask(ref mithPixels);
            //TestMask(MithrilArmor, "helm", mithPixels);
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

            Point demonCenter = demon.Center;
            double cloveRange = 2 * Math.Sqrt(demon.Size);
            if (MinimumSizeMet(demon) && ClovesWithinRange(demonCenter, cloveRange))
            {
                maxOffset = (int) (0.05 * cloveRange);
                xOffset = RNG.Next(-maxOffset, maxOffset + 1);
                yOffset = RNG.Next(-maxOffset, maxOffset + 1);
                LeftClick(demonCenter.X, demonCenter.Y);
                MissedDemons = 0;
                MinDemonSize = ArtifactSize(demon) / 2.0;
                LastDemonLocation = demonCenter;
            }
            else
            {
                MissedDemons++;
            }

            // during the first frame that the bot program cant find a demon, look for a rune med helm drop
            if (MissedDemons == 1)
            {
                CheckDrops();
            }

            if (MissedDemons * RunParams.FrameTime > maxDemonSpawnTime)
            {
                MinDemonSize /= 2.0;
                MissedDemons = 0;
            }

            return true;
        }

        /// <summary>
        /// Telegrabs a rune med helm if one is found on the ground
        /// </summary>
        private void CheckDrops()
        {
            int dropRange = 250;
            int dropRangeLeft = LastDemonLocation.X - dropRange;
            int dropRangeRight = LastDemonLocation.X + dropRange;
            int dropRangeTop = LastDemonLocation.Y - dropRange;
            int dropRangeBottom = LastDemonLocation.Y + dropRange;
            Point trimOffset;
            Color[,] screenDropArea = ScreenPiece(dropRangeLeft, dropRangeRight, dropRangeTop, dropRangeBottom, out trimOffset);

            FindAndAlch(screenDropArea, trimOffset, RuneMedHelm, 70);
            FindAndAlch(screenDropArea, trimOffset, MithrilArmor, 320);
        }

        /// <summary>
        /// Looks for, picks up, and alchs a drop that matches a ColorRange
        /// </summary>
        /// <param name="screenDropArea"></param>
        /// <param name="referenceColor"></param>
        /// <param name="minimumSize">minimum number of pixels needed to </param>
        private void FindAndAlch(Color[,] screenDropArea, Point offset, ColorRange referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = ColorFilter(screenDropArea, referenceColor);
            EraseClientUIFromMask(ref matchedPixels);
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size > minimumSize)
            {
                Point blobCenter = biggestBlob.Center;
                Inventory.Telegrab(ColorArray, blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                //only start alching when the inventory fills up
                Inventory.Alch(ColorArray, 3, 6);
                return;
            }
        }

        /// <summary>
        /// Determines if a demon blob meets the minimum size requirement
        /// </summary>
        /// <param name="demon"></param>
        /// <returns></returns>
        private bool MinimumSizeMet(Blob demon)
        {
            double demonScreenSize = ArtifactSize(demon);
            if (demonScreenSize > MinDemonSize)
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

            //string directory = "C:\\Projects\\RunescapeBot\\test_pictures\\mask_tests\\";
            string directory = "D:\\SourceTree\\runescape_bot\\test_pictures\\mithrilarmor\\";
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
            RuneMedHelm = ColorFilters.RuneMedHelm();
            MithrilArmor = ColorFilters.MithrilArmor();
        }
    }
}
