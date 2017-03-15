using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public class HSBRange
    {
        public HSBRange(int minimumHue, int maximumHue, float minimumSaturation, float maximumSaturation, float minimumBrightness, float maximumBrightness)
        {
            this.MinimumHue = minimumHue;
            if (maximumHue < minimumHue)
            {
                MaximumHue = maximumHue + 360;
            }
            this.MinimumSaturation = minimumSaturation;
            this.MaximumSaturation = maximumSaturation;
            this.MinimumBrightness = minimumBrightness;
            this.MaximumBrightness = maximumBrightness;
        }

        /// <summary>
        /// The minimum hue value
        /// </summary>
        public int MinimumHue { get; set; }

        /// <summary>
        /// The maximum hue value
        /// </summary>
        public int MaximumHue { get; set; }

        /// <summary>
        /// The minimum hue value
        /// </summary>
        public float MinimumSaturation { get; set; }

        /// <summary>
        /// The maximum hue value
        /// </summary>
        public float MaximumSaturation { get; set; }

        /// <summary>
        /// The minimum hue value
        /// </summary>
        public float MinimumBrightness { get; set; }

        /// <summary>
        /// The maximum hue value
        /// </summary>
        public float MaximumBrightness { get; set; }

        /// <summary>
        /// Determines if a color passes all three HSB ranges
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool ColorInRange(Color color)
        {
            return HueInRange(color) && SaturationInRange(color) && BrightnessInRange(color);
        }

        /// <summary>
        /// Determines if a color is in the acceptable hue range
        /// </summary>
        public bool HueInRange(Color color)
        {
            int hue = (int) color.GetHue();

            if (hue < MinimumHue)
            {
                hue += 360;
            }
            
            return (hue >= MinimumHue) && (hue <= MaximumHue);
        }

        /// <summary>
        /// Determines if a color is in the acceptable saturation range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool SaturationInRange(Color color)
        {
            float saturation = color.GetSaturation();
            return (saturation >= MinimumSaturation) && (saturation <= MaximumSaturation);
        }

        /// <summary>
        /// Determines if a color is in the acceptable brightness range
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool BrightnessInRange(Color color)
        {
            float brightness = color.GetBrightness();
            return (brightness >= MinimumBrightness) && (brightness <= MaximumBrightness);
        }
    }
}
