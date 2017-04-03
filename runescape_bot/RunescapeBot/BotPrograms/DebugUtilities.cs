using RunescapeBot.ImageTools;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RunescapeBot.BotPrograms
{
    public static class DebugUtilities
    {
        /// <summary>
        /// Saves a Bitmap object to file as an image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        public static void SaveImageToFile(Bitmap bitmap, string filePath)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            Image img = Image.FromHbitmap(hBitmap);
            img.Save(filePath, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Saves an RGB array to file as an image
        /// </summary>
        /// <param name="rgbArray"></param>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        public static void SaveImageToFile(Color[,] rgbArray, string filePath)
        {
            Bitmap bitmap = new Bitmap(rgbArray.GetLength(0), rgbArray.GetLength(1));
            for (int x = 0; x < rgbArray.GetLength(0); x++)
            {
                for (int y = 0; y < rgbArray.GetLength(1); y++)
                {
                    bitmap.SetPixel(x, y, rgbArray[x, y]);
                }
            }
            SaveImageToFile(bitmap, filePath);
        }

        /// <summary>
        /// Colors the pixels where the given feature was found in Bitmap.
        /// Does not modify Bitmap. Creates a copy and returns the copy with masking applied.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="colorArray"></param>
        /// <param name="bodyPart"></param>
        /// <param name="directory">e.x. "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\"</param>
        /// <param name="saveName"></param>
        /// <param name="mask"></param>
        public static void TestMask(Bitmap bitmap, Color[,] colorArray, ColorRange bodyPart, bool[,] mask, string directory, string saveName)
        {
            Bitmap redBitmap = (Bitmap)bitmap.Clone();
            Bitmap greenBitmap = (Bitmap)bitmap.Clone();
            Bitmap blueBitmap = (Bitmap)bitmap.Clone();
            Bitmap hueBitmap = (Bitmap)bitmap.Clone();
            Bitmap saturationBitmap = (Bitmap)bitmap.Clone();
            Bitmap brightnessBitmap = (Bitmap)bitmap.Clone();
            Bitmap combinedBitmap = (Bitmap)bitmap.Clone();
            Color pixel;

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    pixel = colorArray[x, y];

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
                        combinedBitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            SaveImageToFile(redBitmap, directory + saveName + "_ColorRedMaskTest.jpg");
            SaveImageToFile(greenBitmap, directory + saveName + "_ColorGreenMaskTest.jpg");
            SaveImageToFile(blueBitmap, directory + saveName + "_ColorBlueMaskTest.jpg");
            SaveImageToFile(hueBitmap, directory + saveName + "_HSBHueMaskTest.jpg");
            SaveImageToFile(saturationBitmap, directory + saveName + "_HSBSaturationMaskTest.jpg");
            SaveImageToFile(brightnessBitmap, directory + saveName + "_HSBBrightnessMaskTest.jpg");
            SaveImageToFile(combinedBitmap, directory + saveName + "_TotalMaskTest.jpg");
            SaveImageToFile(bitmap, directory + "Original.jpg");
        }
    }
}
