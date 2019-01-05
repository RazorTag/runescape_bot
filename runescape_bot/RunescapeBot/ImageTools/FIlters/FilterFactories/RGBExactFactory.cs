using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools.Filters.FilterFactories
{
    public static class RGBExactFactory
    {
        /// <summary>
        /// Blue chat text the player's enter into the chat box in the bottom-left corner of the game screen.
        /// </summary>
        /// <returns></returns>
        public static readonly RGBExact PlayerChatText = new RGBExact(Color.Blue);

        /// <summary>
        /// Black name tag for players in the chat box in the bottom-left corner of the game screen.
        /// </summary>
        /// <returns></returns>
        public static readonly RGBExact PlayerChatName = new RGBExact(Color.Black);
    }
}
