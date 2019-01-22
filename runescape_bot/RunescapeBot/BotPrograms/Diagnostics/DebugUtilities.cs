using RunescapeBot.ImageTools;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

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
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            IntPtr hBitmap = bitmap.GetHbitmap();
            Image img = Image.FromHbitmap(hBitmap);

            try
            {
                img.Save(filePath, ImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Unable to save test image to " + filePath);
            }
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
        /// Loads an image from disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>a color array of an image</returns>
        public static Color[,] LoadImageFromFile(string filePath)
        {
            Bitmap bitmap = (Bitmap)Image.FromFile(filePath);
            return BitmapToColorArray(bitmap);
        }

        /// <summary>
        /// Loads an image from a Stream.
        /// </summary>
        /// <param name="stream">Stream to load from</param>
        /// <returns>a color array of an image</returns>
        public static Color[,] LoadImageFromStream(Stream stream)
        {
            Bitmap bitmap = (Bitmap)Image.FromStream(stream);
            return BitmapToColorArray(bitmap);
        }

        /// <summary>
        /// Converts a bitmap to color array image.
        /// </summary>
        /// <param name="bitmap">bitmap to convert</param>
        /// <returns>a color array image</returns>
        public static Color[,] BitmapToColorArray(Bitmap bitmap)
        {
            Color[,] image = new Color[bitmap.Width, bitmap.Height];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    image[x, y] = bitmap.GetPixel(x, y);
                }
            }
            return image;
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
        public static void TestMask(Bitmap bitmap, Color[,] colorArray, IColorFilter bodyPart, bool[,] mask, string directory = "C:\\Projects\\Roboport\\debug_pictures\\mask_tests\\", string saveName = "test")
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

                    //make red bitmap
                    if (!bodyPart.RedInRange(pixel))
                    {
                        redBitmap.SetPixel(x, y, Color.White);
                    }

                    //make green bitmap
                    if (!bodyPart.GreenInRange(pixel))
                    {
                        greenBitmap.SetPixel(x, y, Color.White);
                    }

                    //make blue bitmap
                    if (!bodyPart.BlueInRange(pixel))
                    {
                        blueBitmap.SetPixel(x, y, Color.White);
                    }

                    //make hue bitmap
                    if (!bodyPart.HueInRange(pixel))
                    {
                        hueBitmap.SetPixel(x, y, Color.White);
                    }

                    //make saturation bitmap
                    if (!bodyPart.SaturationInRange(pixel))
                    {
                        saturationBitmap.SetPixel(x, y, Color.White);
                    }

                    //make brightness bitmap
                    if (!bodyPart.BrightnessInRange(pixel))
                    {
                        brightnessBitmap.SetPixel(x, y, Color.White);
                    }

                    //make combined bitmap
                    if (!mask[x, y])
                    {
                        combinedBitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            try
            {
                SaveImageToFile(redBitmap, directory + saveName + "_ColorRedMaskTest.png");
                SaveImageToFile(greenBitmap, directory + saveName + "_ColorGreenMaskTest.png");
                SaveImageToFile(blueBitmap, directory + saveName + "_ColorBlueMaskTest.png");
                SaveImageToFile(hueBitmap, directory + saveName + "_HSBHueMaskTest.png");
                SaveImageToFile(saturationBitmap, directory + saveName + "_HSBSaturationMaskTest.png");
                SaveImageToFile(brightnessBitmap, directory + saveName + "_HSBBrightnessMaskTest.png");
                SaveImageToFile(combinedBitmap, directory + saveName + "_TotalMaskTest.png");
                SaveImageToFile(bitmap, directory + "Original.png");
            }
            catch
            {
                MessageBox.Show("Unable to save " + saveName + " test images to directory " + directory);
            }
        }
    }
}
