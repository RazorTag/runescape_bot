using RunescapeBot.Common;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
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
        public MakeAll(int xClick, int yClick, Process rsClient, Keyboard keyboard) : base(xClick, yClick, rsClient, keyboard)
        {
            
        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 110;
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
        /// Click the Make-X option in a Make-X pop-up
        /// </summary>
        public void MakeX(int itemsToMake)
        {
            const int yOffset = 70;
            SelectOption(yOffset);

            //Wait for the "Enter amount:" prompt to appear
            if (WaitForEnterAmount(5000))
            {
                BotProgram.SafeWaitPlus(200, 100);
                BotUtilities.EnterAmount(Keyboard, itemsToMake);
            }
        }

        /// <summary>
        /// Use the Make-All option in a Make-X pop-up
        /// </summary>
        public void MakeAllItems()
        {
            const int yOffset = 85;
            SelectOption(yOffset);
        }
    }
}
