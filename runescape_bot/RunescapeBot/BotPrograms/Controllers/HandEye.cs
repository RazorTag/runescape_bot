using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class HandEye
    {
        #region properties

        private Vision Vision;
        private GameScreen Screen;

        #endregion

        #region constructors

        public HandEye(Vision vision, GameScreen screen)
        {
            Vision = vision;
            Screen = screen;
        }

        #endregion

        #region methods

        /// <summary>
        /// Clicks on a stationary object
        /// </summary>
        /// <param name="stationaryObject">color filter for the stationary object</param>
        /// <param name="tolerance">maximum allowable distance between two subsequent checks to consider both objects the same object</param>
        /// <param name="afterClickWait">time to wait after clicking on the stationary object</param>
        /// <param name="maxWaitTime">maximum time to wait before giving up</param>
        /// <returns></returns>
        internal bool ClickStationaryObject(ColorFilter stationaryObject, double tolerance, int afterClickWait, int maxWaitTime, int minimumSize)
        {
            Blob foundObject;

            if (Vision.LocateStationaryObject(stationaryObject, out foundObject, tolerance, maxWaitTime, minimumSize))
            {
                Mouse.LeftClick(foundObject.Center.X, foundObject.Center.Y);
                BotProgram.SafeWaitPlus(afterClickWait, 0.2 * afterClickWait);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Mouses over an alleged stationary object. Left-clicks if the stationary object text appears.
        /// </summary>
        /// <param name="stationaryObject">alleged stationary object</param>
        /// <param name="click">set to false to skip clicking on the stationary object</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching stationary object is found and clicked on</returns>
        internal bool MouseOverStationaryObject(Blob stationaryObject, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(stationaryObject.Center, RGBHSBRangeFactory.MouseoverTextStationaryObject(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over an alleged NPC. Left-clicks if the NPC text appears.
        /// </summary>
        /// <param name="ObjectsToCheck">alleged NPC</param
        /// <param name="click">set to false to skip clicking on the NPC</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching NPC is found and clicked on</returns>
        internal bool MouseOverNPC(Blob npc, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(npc.Center, RGBHSBRangeFactory.MouseoverTextNPC(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over an alleged NPC. Left-clicks if the NPC text appears.
        /// </summary>
        /// <param name="ObjectsToCheck">alleged NPC</param
        /// <param name="click">set to false to skip clicking on the NPC</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching NPC is found and clicked on</returns>
        internal bool MouseOverDroppedItem(Blob item, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(item.Center, RGBHSBRangeFactory.MouseoverTextDroppedItem(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over each object in a list of blobs. Left-clicks the first object with mouseover text matching textColor.
        /// </summary>
        /// <param name="ObjectsToCheck">list of objects to mouse over</param>
        /// <param name="textColor">color of text expected to be in the top-left on mouseover</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching object is found and clicked on</returns>
        internal bool MouseOver(List<Blob> ObjectsToCheck, ColorFilter textColor, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            randomization = (int)((Screen.Height / 1000.0) * randomization);
            Point clickLocation;

            foreach (Blob objectCheck in ObjectsToCheck)
            {
                if (BotProgram.StopFlag) { return false; }

                clickLocation = objectCheck.Center;

                if (MouseOver(clickLocation, textColor, click, randomization, maxWait))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Mouses over a single point and left-clicks it if it matches the specified mouseover text color.
        /// </summary>
        /// <param name="mouseover">point to mouse over</param>
        /// <param name="textColor">color of text expected to be in the top-left on mouseover</param>
        /// <param name="click">set to false to hover and check without clicking</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <param name="maxWait">max time to wait before concluding that the mouse over failed</param>
        /// <param name="lagTime">time to wait before checking and clicking after mousing over</param>
        /// <returns>true if a matching object is found and clicked on</returns>
        internal bool MouseOver(Point mouseover, ColorFilter textColor, bool click = true, int randomization = 5, int maxWait = 1000, int lagTime = 250)
        {
            randomization = (int)((Screen.Height / 1000.0) * randomization);
            mouseover = Probability.GaussianCircle(mouseover, randomization);
            Mouse.Move(mouseover.X, mouseover.Y);
            if (BotProgram.SafeWait(lagTime)) { return false; }

            if (Vision.WaitForMouseOverText(textColor, maxWait))
            {
                if (click) { Mouse.LeftClick(mouseover.X, mouseover.Y, 0, 0); }
                return true;
            }
            return false;
        }

        #endregion
    }
}
