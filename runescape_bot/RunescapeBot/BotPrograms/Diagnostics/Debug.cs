using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms.Debug
{
    public class BotProgramActions
    {
        public BotProgramActions()
        {
            LeftClicks = new List<Point>();
            RightClicks = new List<Point>();
        }

        /// <summary>
        /// List of left clicks executed for a bot program
        /// </summary>
        public List<Point> LeftClicks { get; }

        /// <summary>
        /// List of right clicks executed for a bot program
        /// </summary>
        public List<Point> RightClicks { get; }

        /// <summary>
        /// Adds a single click to the list of clicks
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SaveClick(int x, int y, bool rightClick = false)
        {
            if (rightClick)
            {
                RightClicks.Add(new Point(x, y));
            }
            else
            {
                LeftClicks.Add(new Point(x, y));
            }
        }
    }
}
