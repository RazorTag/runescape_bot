using System.Drawing;

namespace RunescapeBot.ImageTools
{
    /// <summary>
    /// Represents a range of RGB colors used to identify artifacts of a known color
    /// </summary>
    public class RGBHSBRange : IColorFilter
    {
        /// <summary>
        /// The color with the values closest to black that occur in an artifact
        /// </summary>
        public Color DarkestColor { get; set; }

        /// <summary>
        /// The color with the values closest to white that occur in an artifact
        /// </summary>
        public Color LightestColor { get; set; }

        /// <summary>
        /// The acceptable range of hues
        /// </summary>
        public HSBRange HSBRange { get; set; }

        /// <summary>
        /// Sets the color range
        /// </summary>
        /// <param name="darkestColor">The color with the values closest to black that occur in an artifact</param>
        /// <param name="lightestColor">The color with the values closest to white that occur in an artifact</param>
        public RGBHSBRange(Color darkestColor, Color lightestColor)
        {
            this.DarkestColor = darkestColor;
            this.LightestColor = lightestColor;
            this.HSBRange = null;
        }

        /// <summary>
        /// Sets the color range
        /// </summary>
        /// <param name="darkestColor">The color with the values closest to black that occur in an artifact</param>
        /// <param name="lightestColor">The color with the values closest to white that occur in an artifact</param>
        public RGBHSBRange(HSBRange hsbRange)
        {
            this.DarkestColor = Color.FromArgb(0, 0, 0);
            this.LightestColor = Color.FromArgb(255, 255, 255);
            this.HSBRange = hsbRange;
        }

        /// <summary>
        /// Sets the color range
        /// </summary>
        /// <param name="darkestColor">The color with the values closest to black that occur in an artifact</param>
        /// <param name="lightestColor">The color with the values closest to white that occur in an artifact</param>
        public RGBHSBRange(Color darkestColor, Color lightestColor, HSBRange hsbRange)
        {
            this.DarkestColor = darkestColor;
            this.LightestColor = lightestColor;
            this.HSBRange = hsbRange;
        }

        /// <summary>
        /// Determines if the given color's RGB and HSB falls within this color range
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true if the given color falls within this color range</returns>
        public bool ColorInRange(Color color)
        {
            return  RGBInRange(color) && HSBInRange(color);
        }

        /// <summary>
        /// Determines if a color falls within the red, green, and blue ranges
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if color meets all thre red, green, and blue color ranges</returns>
        public bool RGBInRange(Color color)
        {
            if ((DarkestColor == null) || (LightestColor == null))
            {
                return true;
            }

            else
            {
                return RedInRange(color) && GreenInRange(color) && BlueInRange(color);
            }
        }

        /// <summary>
        /// Determines if the red component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the red component is inside of this color range</returns>
        public bool RedInRange(Color color)
        {
            return (color.R >= DarkestColor.R) && (color.R <= LightestColor.R);
        }

        /// <summary>
        /// Determines if the green component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the green component is inside of this color range</returns>
        public bool GreenInRange(Color color)
        {
            return (color.G >= DarkestColor.G) && (color.G <= LightestColor.G);
        }

        /// <summary>
        /// Determines if the blue component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the blue component is inside of this color range</returns>
        public bool BlueInRange(Color color)
        {
            return (color.B >= DarkestColor.B) && (color.B <= LightestColor.B);
        }

        /// <summary>
        /// Determines if a color falls within the hue range
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if the color is in hue range or if the hue is unspecified</returns>
        public bool HSBInRange(Color color)
        {
            if (HSBRange == null)
            {
                return true;
            }
            return HSBRange.ColorInRange(color);
        }

        /// <summary>
        /// Determines if a color is in the acceptable hue range
        /// </summary>
        public bool HueInRange(Color color)
        {
            return HSBRange.HueInRange(color);
        }

        /// <summary>
        /// Determines if a color is in the acceptable saturation range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool SaturationInRange(Color color)
        {
            return HSBRange.SaturationInRange(color);
        }

        /// <summary>
        /// Determines if a color is in the acceptable brightness range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool BrightnessInRange(Color color)
        {
            return HSBRange.BrightnessInRange(color);
        }
    }
}
