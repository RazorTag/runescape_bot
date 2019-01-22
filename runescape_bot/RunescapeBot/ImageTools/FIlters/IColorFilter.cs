using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public interface IColorFilter
    {
        /// <summary>
        /// Determines if the given color's RGB and HSB falls within this color range
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true if the given color falls within this color range</returns>
        bool ColorInRange(Color color);

        /// <summary>
        /// Determines if a color falls within the red, green, and blue ranges
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if color meets all thre red, green, and blue color ranges</returns>
        bool RGBInRange(Color color);

        /// <summary>
        /// Determines if the red component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the red component is inside of this color range</returns>
        bool RedInRange(Color color);

        /// <summary>
        /// Determines if the green component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the green component is inside of this color range</returns>
        bool GreenInRange(Color color);

        /// <summary>
        /// Determines if the blue component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the blue component is inside of this color range</returns>
        bool BlueInRange(Color color);

        /// <summary>
        /// Determines if a color falls within the hue range
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if the color is in hue range or if the hue is unspecified</returns>
        bool HSBInRange(Color color);

        /// <summary>
        /// Determines if a color is in the acceptable hue range
        /// </summary>
        bool HueInRange(Color color);

        /// <summary>
        /// Determines if a color is in the acceptable saturation range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        bool SaturationInRange(Color color);

        /// <summary>
        /// Determines if a color is in the acceptable brightness range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        bool BrightnessInRange(Color color);
    }


    /// <summary>
    /// Does not pass any pixels ever.
    /// </summary>
    public class RejectFilter : IColorFilter
    {
        public bool BlueInRange(Color color)
        {
            return false;
        }

        public bool BrightnessInRange(Color color)
        {
            return false;
        }

        public bool ColorInRange(Color color)
        {
            return false;
        }

        public bool GreenInRange(Color color)
        {
            return false;
        }

        public bool HSBInRange(Color color)
        {
            return false;
        }

        public bool HueInRange(Color color)
        {
            return false;
        }

        public bool RedInRange(Color color)
        {
            return false;
        }

        public bool RGBInRange(Color color)
        {
            return false;
        }

        public bool SaturationInRange(Color color)
        {
            return false;
        }
    }
}
