using System.Drawing;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Represents a range of RGB colors used to identify artifacts of a known color
    /// </summary>
    class ColorRange
    {
        /// <summary>
        /// The color with the values closest to black that occur in an artifact
        /// </summary>
        public Color darkestColor { get; set; }

        /// <summary>
        /// The color with the values closest to white that occur in an artifact
        /// </summary>
        public Color lightestColor { get; set; }

        /// <summary>
        /// Sets the color range
        /// </summary>
        /// <param name="darkestColor">The color with the values closest to black that occur in an artifact</param>
        /// <param name="lightestColor">The color with the values closest to white that occur in an artifact</param>
        public ColorRange(Color darkestColor, Color lightestColor)
        {
            this.darkestColor = darkestColor;
            this.lightestColor = lightestColor;
        }

        /// <summary>
        /// Determines if the given color falls within this color range
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true if the given color falls within this color range</returns>
        public bool ColorInRange(Color color)
        {
            return RedInRange(color) && GreenInRange(color) && BlueInRange(color);
        }

        /// <summary>
        /// Determines if the red component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the red component is inside of this color range</returns>
        public bool RedInRange(Color color)
        {
            return (color.R >= darkestColor.R) && (color.R <= lightestColor.R);
        }

        /// <summary>
        /// Determines if the green component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the green component is inside of this color range</returns>
        public bool GreenInRange(Color color)
        {
            return (color.G >= darkestColor.G) && (color.G <= lightestColor.G);
        }

        /// <summary>
        /// Determines if the blue component of the given color falls within this color range
        /// </summary>
        /// <param name="color">color to check</param>
        /// <returns>true if the blue component is inside of this color range</returns>
        public bool BlueInRange(Color color)
        {
            return (color.B >= darkestColor.B) && (color.B <= lightestColor.B);
        }
    }
}
