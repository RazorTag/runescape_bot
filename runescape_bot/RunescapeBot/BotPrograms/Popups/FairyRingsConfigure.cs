using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Popups
{
    public class FairyRingsConfigure
    {
        #region properties

        /// <summary>
        /// Distance right or left of a dial to click in order to effect the spin direction o the dial.
        /// </summary>
        const int CLICK_DIRECTIONAL_OFFSET = 30;

        /// <summary>
        /// The location of the leftmost dial.
        /// </summary>
        protected Point LeftDial { get; set; }

        /// <summary>
        /// The location of the leftmost dial.
        /// </summary>
        protected Point CenterDial { get; set; }

        /// <summary>
        /// The location of the leftmost dial.
        /// </summary>
        protected Point RightDial { get; set; }

        /// <summary>
        /// The location of the x button to close the configuration window.
        /// </summary>
        protected Point CloseButton { get; set; }

        /// <summary>
        /// The location of the teleport button.
        /// </summary>
        protected Point TeleportButton { get; set; }

        /// <summary>
        /// Game client
        /// </summary>
        protected Process RSClient { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Determines the locations of each of the three dials
        /// </summary>
        /// <param name="screen"></param>
        public FairyRingsConfigure(Color[,] screen, Process rsClient)
        {
            int width = screen.GetLength(1);
            int height = screen.GetLength(0);
            RSClient = rsClient;
            DetermineDialLocations(width, height);
        }

        /// <summary>
        /// Determines the locations of each of the three dials
        /// </summary>
        /// <param name="width">width of the game screen in pixels</param>
        /// <param name="height">height of the game screen in pixels</param>
        public FairyRingsConfigure(int width, int height)
        {
            DetermineDialLocations(width, height);
        }

        /// <summary>
        /// Determines the locations of each of the three dials
        /// </summary>
        /// <param name="screen">image off the entire game screen</param>
        protected void DetermineDialLocations(int width, int height)
        {
            int dialY = (int)(216 + 0.5 * (width - 505));
            int leftX = (int)(98 + 0.5 * (height - 780));
            LeftDial = new Point(leftX, dialY);
            CenterDial = new Point(leftX + 168, dialY);
            RightDial = new Point(leftX + 336, dialY);
            CloseButton = new Point(leftX + 400, dialY - 192);
            TeleportButton = new Point(leftX + 168, dialY + 66);
        }

        /// <summary>
        /// Closes the fairy ring configuration popup.
        /// </summary>
        public void Close()
        {
            Mouse.LeftClick(CloseButton.X, CloseButton.Y, RSClient, 5);
        }

        #endregion

        #region configuration

        /// <summary>
        /// Configures the three faiiry ring dials.
        /// </summary>
        /// <param name="leftCode">Character to be set in left dial.</param>
        /// <param name="centerCode">Character to be set in center dial.</param>
        /// <param name="rightCode">Character to be set in right dial.</param>
        public void SetConfiguration(char leftCode, char centerCode, char rightCode)
        {
            SpinLeftDial(leftCode);
            SpinCenterDial(centerCode);
            SpinRightDial(rightCode);
            WaitForDialSpin(2);
        }

        /// <summary>
        /// Spins the left dial to the specified letter
        /// </summary>
        /// <param name="code"></param>
        public void SpinLeftDial(char code)
        {
            switch(code)
            {
                case 'a':
                    break;
                case 'b':
                    Mouse.LeftClick(LeftDial.X - CLICK_DIRECTIONAL_OFFSET, LeftDial.Y, RSClient);
                    break;
                case 'd':
                    Mouse.LeftClick(LeftDial.X + CLICK_DIRECTIONAL_OFFSET, LeftDial.Y, RSClient);
                    break;
                case 'c':
                    Mouse.LeftClick(LeftDial.X - CLICK_DIRECTIONAL_OFFSET, LeftDial.Y, RSClient);
                    Mouse.LeftClick(LeftDial.X - CLICK_DIRECTIONAL_OFFSET, LeftDial.Y, RSClient);
                    break;
            }
        }

        /// <summary>
        /// Spins the left dial to the specified letter
        /// </summary>
        /// <param name="code"></param>
        public void SpinCenterDial(char code)
        {
            switch (code)
            {
                case 'i':
                    break;
                case 'j':
                    Mouse.LeftClick(CenterDial.X - CLICK_DIRECTIONAL_OFFSET, CenterDial.Y, RSClient);
                    break;
                case 'l':
                    Mouse.LeftClick(CenterDial.X + CLICK_DIRECTIONAL_OFFSET, CenterDial.Y, RSClient);
                    break;
                case 'k':
                    Mouse.LeftClick(CenterDial.X - CLICK_DIRECTIONAL_OFFSET, CenterDial.Y, RSClient);
                    Mouse.LeftClick(CenterDial.X - CLICK_DIRECTIONAL_OFFSET, CenterDial.Y, RSClient);
                    break;
            }
        }

        /// <summary>
        /// Spins the left dial to the specified letter
        /// </summary>
        /// <param name="code"></param>
        public void SpinRightDial(char code)
        {
            switch (code)
            {
                case 'p':
                    break;
                case 'q':
                    Mouse.LeftClick(RightDial.X - CLICK_DIRECTIONAL_OFFSET, RightDial.Y, RSClient);
                    break;
                case 's':
                    Mouse.LeftClick(RightDial.X + CLICK_DIRECTIONAL_OFFSET, RightDial.Y, RSClient);
                    break;
                case 'r':
                    Mouse.LeftClick(RightDial.X - CLICK_DIRECTIONAL_OFFSET, RightDial.Y, RSClient);
                    Mouse.LeftClick(RightDial.X - CLICK_DIRECTIONAL_OFFSET, RightDial.Y, RSClient);
                    break;
            }
        }

        /// <summary>
        /// Waits for a dial to spin the specified number of quarter turns
        /// </summary>
        protected void WaitForDialSpin(int numberOfTurns)
        {
            const int quarterTurnTime = 1000;
            BotProgram.SafeWaitPlus(numberOfTurns * quarterTurnTime, 100);
        }

        /// <summary>
        /// Teleports the player to the configured location.
        /// </summary>
        public void Teleport(bool waitForTeleport)
        {
            Mouse.LeftClick(TeleportButton.X, TeleportButton.Y, RSClient, 8);
            if (waitForTeleport)
            {
                BotProgram.SafeWait(2000);
            }
        }

        #endregion
    }
}
