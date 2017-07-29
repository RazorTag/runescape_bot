using RunescapeBot.Common;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms.Popups
{
    /// <summary>
    /// Choose Option popup that contains the options Make-1, Make-5, Make-10, and Make-X
    /// -crafting jewelry at a furnace
    /// </summary>
    public class MakeX : RightClick
    {
        /// <summary>
        /// Create a record of a standard Make X popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public MakeX(int xClick, int yClick, Process rsClient) : base(xClick, yClick, rsClient) { }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 95;
            TitleHeight = 16;
            Width = 154;
        }

        /// <summary>
        /// Click the Make-1 option in a Make-X pop-up
        /// </summary>
        public void MakeOne()
        {
            const int yOffset = 25;
            SelectOption(yOffset);
        }

        /// <summary>
        /// Click the Make-5 option in a Make-X pop-up
        /// </summary>
        public void MakeFive()
        {
            const int yOffset = 40;
            SelectOption(yOffset);
        }

        /// <summary>
        /// Click the Make-10 option in a Make-X pop-up
        /// </summary>
        public void MakeTen()
        {
            const int yOffset = 55;
            SelectOption(yOffset);
        }

        /// <summary>
        /// Use the Make-X option in a Make-X pop-up
        /// </summary>
        public void MakeXItems(int itemsToMake)
        {
            const int yOffset = 70;
            SelectOption(yOffset);

            //Wait for the "Enter amount:" prompt to appear
            if (WaitForEnterAmount(5000))
            {
                Thread.Sleep(500);
                BotUtilities.EnterAmount(RSClient, itemsToMake);
            }
        }
    }
}