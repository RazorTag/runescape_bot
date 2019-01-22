using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.ImageTools
{
    public class RGBHSBRangeGroup : IColorFilter
    {
        protected List<RGBHSBRange> ColorRanges;

        public RGBHSBRangeGroup()
        {
            ColorRanges = new List<RGBHSBRange>();
        }

        public RGBHSBRangeGroup(List<RGBHSBRange> colorRanges)
        {
            ColorRanges = colorRanges;
        }

        /// <summary>
        /// Adds a new color filter range to the group
        /// </summary>
        /// <param name="colorRange">new color filter range</param>
        public void AddColorRange(RGBHSBRange colorRange)
        {
            ColorRanges.Add(colorRange);
        }

        /// <summary>
        /// Determines if the given color's RGB and HSB falls within any of the ranges in this color range group
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true if the given color falls within this color range</returns>
        public bool ColorInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.ColorInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a color falls within any of the red, green, and blue ranges in this color range group
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if color meets all thre red, green, and blue color ranges</returns>
        public bool RGBInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.RGBInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the red component of the given color falls within any of the ranges in this color range group
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the red component is inside of this color range</returns>
        public bool RedInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.RedInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the green component of the given color falls within any of the ranges in this color range group
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the green component is inside of this color range</returns>
        public bool GreenInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.GreenInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the blue component of the given color falls within any of the ranges in this color range group
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the blue component is inside of this color range</returns>
        public bool BlueInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.BlueInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a color falls within any of the HSB ranges in this color range group
        /// </summary>
        /// <param name="color"></param>
        /// <returns>true if the color is in hue range or if the hue is unspecified</returns>
        public bool HSBInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.HSBInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a color is in the acceptable hue range
        /// </summary>
        public bool HueInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.HueInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a color is in the acceptable saturation range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool SaturationInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.SaturationInRange(color))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a color is in the acceptable brightness range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool BrightnessInRange(Color color)
        {
            foreach (RGBHSBRange colorRange in ColorRanges)
            {
                if (colorRange.BrightnessInRange(color))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
