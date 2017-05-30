using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Threading;

namespace RunescapeBot.BotPrograms.Popups
{
    /// <summary>
    /// Choose Option popup that contains the options Make 1, Make 5, Make X, and Make All
    /// </summary>
    public class MakeAll : RightClick
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public MakeAll(int xClick, int yClick, Process rsClient) : base(xClick, yClick, rsClient)
        {
            
        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 95;
            TitleHeight = 15;
            Width = 154;
        }

        /// <summary>
        /// Click the Make-1 option in a Make-X pop-up
        /// </summary>
        public void MakeOne()
        {
            const int yOffset = 25;
            RandomClick(yOffset);
        }

        /// <summary>
        /// Click the Make-5 option in a Make-X pop-up
        /// </summary>
        public void MakeFive()
        {
            const int yOffset = 40;
            RandomClick(yOffset);
        }

        /// <summary>
        /// Click the Make-10 option in a Make-X pop-up
        /// </summary>
        public void MakeX(int itemsToMake)
        {
            const int yOffset = 55;
            RandomClick(yOffset);

            //Wait for the "Enter amount:" prompt to appear
            if (WaitForEnterAmount(5000))
            {
                Utilities.EnterAmount(RSClient, itemsToMake);
            }
        }

        /// <summary>
        /// Use the Make-X option in a Make-X pop-up
        /// </summary>
        public void MakeAllItems()
        {
            const int yOffset = 70;
            RandomClick(yOffset);
        }

        /// <summary>
        /// Click a random spot on a given row
        /// </summary>
        /// <param name="yOffset"></param>
        private void RandomClick(int yOffset)
        {
            const int padding = 2;
            Random rng = new Random();
            
            XClick += rng.Next(-(Width / 2) + padding, (Width / 2) - padding + 1);
            YClick += yOffset + rng.Next(-2, 3);
            Mouse.LeftClick(XClick, YClick, RSClient);
        }
    }
}
