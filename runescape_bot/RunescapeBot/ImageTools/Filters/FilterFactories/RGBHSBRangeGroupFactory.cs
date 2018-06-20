using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public static class RGBHSBRangeGroupFactory
    {
        /// <summary>
        /// Text colors that can appear in an NPC dialog
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRangeGroup DialogText()
        {
            RGBHSBRangeGroup filter = new RGBHSBRangeGroup();
            filter.AddColorRange(RGBHSBRangeFactory.Black());
            filter.AddColorRange(RGBHSBRangeFactory.DialogBoxBodyBlue());
            
            return filter;
        }
    }
}
