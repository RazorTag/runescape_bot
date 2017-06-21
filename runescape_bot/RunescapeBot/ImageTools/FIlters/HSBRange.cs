using System.Drawing;

namespace RunescapeBot.ImageTools
{
    public class HSBRange
    {
        /// <summary>
        /// The minimum hue value
        /// </summary>
        public float MinimumHue { get; set; }

        /// <summary>
        /// The maximum hue value
        /// </summary>
        public float MaximumHue { get; set; }

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
        /// The hue range is assumed to run clockwise.
        /// A hue range with a minimum of 355 and a maximum of 20 covers an arc of 25 degrees.
        /// A hue range with a minimum of 20 and a maximum of 355 covers an arc of 335 degrees.
        /// </summary>
        /// <param name="minimumHue"></param>
        /// <param name="maximumHue"></param>
        /// <param name="minimumSaturation"></param>
        /// <param name="maximumSaturation"></param>
        /// <param name="minimumBrightness"></param>
        /// <param name="maximumBrightness"></param>
        public HSBRange(float minimumHue, float maximumHue, float minimumSaturation, float maximumSaturation, float minimumBrightness, float maximumBrightness)
        {
            this.MinimumHue = minimumHue;
            if (maximumHue < minimumHue)
            {
                this.MaximumHue = maximumHue + 360;
            }
            else
            {
                this.MaximumHue = maximumHue;
            }
            this.MinimumSaturation = minimumSaturation;
            this.MaximumSaturation = maximumSaturation;
            this.MinimumBrightness = minimumBrightness;
            this.MaximumBrightness = maximumBrightness;
        }

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
            float hue = color.GetHue();

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
