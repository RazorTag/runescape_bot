﻿using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.FixedUIComponents
{
    public class Minimap
    {
        Color[,] Screen;
        private Process RSClient;
        private Keyboard Keyboard;

        private int ScreenWidth
        {
            get
            {
                if (Screen != null)
                {
                    return Screen.GetLength(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        public Minimap(Process rsClient, Keyboard keyboard)
        {
            RSClient = rsClient;
            Keyboard = keyboard;
        }

        /// <summary>
        /// Called by BotProgram to update with the most current screenshot
        /// </summary>
        /// <param name="colorArray">the newest screenshot</param>
        public void SetScreen(Color[,] colorArray)
        {
            Screen = colorArray;
        }

        /// <summary>
        /// Determines if the player has very green hitpoints
        /// </summary>
        /// <returns></returns>
        public bool HighHitpoints()
        {
            RectangleBounds hitpoints = HitpointsDigitsArea();
            double greenHitpointMatch = ImageProcessing.FractionalMatchPiece(Screen, RGBHSBRangeFactory.HitpointsGreen(), hitpoints.Left, hitpoints.Right, hitpoints.Top, hitpoints.Bottom);
            return greenHitpointMatch > 0.05;
        }

        /// <summary>
        /// Gets the bounds of the hitpoints digits area
        /// </summary>
        /// <param name="screenSize">dimensions of the game screen</param>
        /// <returns>left, right, top, and bottom bounds</returns>
        public RectangleBounds HitpointsDigitsArea()
        {
            int left = ScreenWidth - HITPOINTS_DIGITS_OFFSET_LEFT;
            int right = left + HITPOINTS_DIGITS_WIDTH;
            int top = HITPOINTS_DIGITS_OFFSET_TOP;
            int bottom = top + HITPOINTS_DIGITS_HEIGHT;
            return new RectangleBounds(left, right, top, bottom);
        }

        /// <summary>
        /// Determines the center of the minimap viewing area
        /// </summary>
        /// <returns></returns>
        public Point MinimapCenter()
        {
            int x = ScreenWidth - OFFSET_LEFT + (WIDTH / 2);
            int y = OFFSET_TOP + (HEIGHT / 2);
            return new Point(x, y);
        }

        #region constants

        public const int HITPOINTS_DIGITS_OFFSET_LEFT = 207; //pixels from left of hitpoints digits area to right of screen (using screen width as x index for right of screen)
        public const int HITPOINTS_DIGITS_OFFSET_TOP = 61; //pixels from top of screen to top of hitpoints digits area (using 0 as y index for top of screen)
        public const int HITPOINTS_DIGITS_WIDTH = 21;  //pixels from left to right of hitpoints digits area (1 less than horizontal number of pixels)
        public const int HITPOINTS_DIGITS_HEIGHT = 13;  //pixels from top to bottom of hitpoints digits area (1 less than vertical number of pixels)

        public const int WIDTH = 150;
        public const int HEIGHT = 152;
        public const int OFFSET_LEFT = 156; //pixels from the left side of the minimap to the right edge of the game screen
        public const int OFFSET_TOP = 8;    //pixels from the top of the game screen to the top of the minimap

        #endregion
    }
}
