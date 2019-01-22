using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools.Filters
{
    public class RGBExact : IColorFilter
    {
        private Color ColorMatch;


        public RGBExact(Color color)
        {
            ColorMatch = color;
        }


        public bool RedInRange(Color color)
        {
            return ColorMatch.R == color.R;
        }

        public bool GreenInRange(Color color)
        {
            return ColorMatch.G == color.G;
        }

        public bool BlueInRange(Color color)
        {
            return ColorMatch.B == color.B;
        }

        public bool ColorInRange(Color color)
        {
            return RedInRange(color) && GreenInRange(color) && BlueInRange(color);
        }

        public bool BrightnessInRange(Color color)
        {
            return true;
        }

        public bool HSBInRange(Color color)
        {
            return true;
        }

        public bool HueInRange(Color color)
        {
            return true;
        }

        public bool RGBInRange(Color color)
        {
            return true;
        }

        public bool SaturationInRange(Color color)
        {
            return true;
        }
    }
}
