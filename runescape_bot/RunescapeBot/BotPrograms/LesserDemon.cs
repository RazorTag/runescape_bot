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
        private static ColorRange LesserDemonSkin;
        private static ColorRange LesserDemonHorn;

        public static Bitmap Bitmap;

        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
        }

        /// <summary>
        /// Called once before execute
        /// </summary>
        protected override void Run()
        {
            Bitmap = ReadWindow();
            if (Bitmap != null)     //Make sure the read is successful before using the bitmap values
            {
                bool[,] skinPixels;
                bool[,] hornPixels;
                FindDemonPixels(out skinPixels, out hornPixels);

                TestMask(LesserDemonSkin, "Skin", skinPixels);
                TestMask(LesserDemonHorn, "Horn", hornPixels);
                //LeftClick(1000, 500);
                //ScreenScraper.WriteBitmapToFile(skinBitmap, "C:\\Projects\\RunescapeBot\\test_pictures\\TestMaskSkin.jpg", ImageFormat.Jpeg);
                //ScreenScraper.WriteBitmapToFile(hornBitmap, "C:\\Projects\\RunescapeBot\\test_pictures\\TestMaskHorn.jpg", ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override void Execute()
        {
            
        }

        /// <summary>
        /// Locates the pixels in a Bitmap that match the parts of the lesser demon to search for
        /// </summary>
        /// <param name="skinPixels">Color range representing a lesser demon's skin</param>
        /// <param name="hornPixels">Color range representing a lesser demon's horn, hoofs, or tail spike</param>
        private void FindDemonPixels(out bool[,] skinPixels, out bool[,] hornPixels)
        {
            Color pixelColor;
            skinPixels = new bool[Bitmap.Width, Bitmap.Height];     //don't use the zero indices
            hornPixels = new bool[Bitmap.Width, Bitmap.Height];

            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    pixelColor = Bitmap.GetPixel(x, y);
                    if (LesserDemonSkin.ColorInRange(pixelColor))
                    {
                        skinPixels[x, y] = true;
                    }
                    if (LesserDemonHorn.ColorInRange(pixelColor))
                    {
                        hornPixels[x, y] = true;
                    }
                }
            }
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
            ScreenScraper.WriteBitmapToFile(redBitmap, directory + saveName + "_RedMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.WriteBitmapToFile(greenBitmap, directory + saveName + "_GreenMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.WriteBitmapToFile(blueBitmap, directory + saveName + "_BlueMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.WriteBitmapToFile(bitmap, directory + saveName + "_TotalMaskTest.jpg", ImageFormat.Jpeg);
            ScreenScraper.WriteBitmapToFile(Bitmap, directory + "Original.jpg", ImageFormat.Jpeg);
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
            Color dark = Color.FromArgb(50, 2, 0);
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
