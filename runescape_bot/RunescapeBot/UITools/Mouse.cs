using RunescapeBot.ImageTools;
using System;
using System.Diagnostics;
using System.Threading;
using static RunescapeBot.UITools.User32;

namespace RunescapeBot.UITools
{
    public static class Mouse
    {
        #region constants
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        /// <summary>
        /// Pixels per second to move the mouse
        /// </summary>
        private const double MOUSE_MOVE_SPEED = 3000.0;

        /// <summary>
        /// Mouse movements per second to execute when moving the mouse smoothly
        /// </summary>
        private const double MOUSE_MOVE_RATE = 60.0;
        #endregion

        #region click handlers
        /// <summary>
        /// Execute a left mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void LeftClick(int x, int y, Process rsClient, int hoverDelay = 100)
        {
            Click(x, y, rsClient, MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP, hoverDelay);
        }

        /// <summary>
        /// Execute a right mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void RightClick(int x, int y, Process rsClient, int hoverDelay = 100)
        {
            Click(x, y, rsClient, MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_RIGHTUP, hoverDelay);
        }

        /// <summary>
        /// Click down and release a mouse button
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rsClient"></param>
        /// <param name="clickTypeDown"></param>
        /// <param name="clickTypeUp"></param>
        private static void Click(int x, int y, Process rsClient, int clickTypeDown, int clickTypeUp, int hoverDelay)
        {
            int hWnd = rsClient.MainWindowHandle.ToInt32();
            ScreenScraper.BringToForeGround(rsClient);
            TranslateClick(ref x, ref y, rsClient);
            NaturalMove(x, y);
            Random rng = new Random();
            Thread.Sleep(rng.Next(hoverDelay, (int)(hoverDelay * 1.5)));  //wait for RS client to recognize the cursor hover
            mouse_event(clickTypeDown, x, y, 0, 0);
            mouse_event(clickTypeUp, x, y, 0, 0);
        }

        /// <summary>
        /// Translate a click location from a position within the diplay portion of OSBuddy to a position on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void TranslateClick(ref int x, ref int y, Process rsClient)
        {
            //adjust for the position of the OSBuddy window
            RECT windowRect = new RECT();
            GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
            x += windowRect.left;
            y += windowRect.top;

            //adjust for the borders and toolbar
            x += ScreenScraper.OSBUDDY_BORDER_WIDTH;
            y += ScreenScraper.OSBUDDY_TOOLBAR_WIDTH + ScreenScraper.OSBUDDY_BORDER_WIDTH;
        }

        /// <summary>
        /// Clicks at the specified point on the monitor without translating to game screen coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void RawClick(int x, int y)
        {
            NaturalMove(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        /// <summary>
        /// Moves a mouse across a screen in a straight line
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Move(int x, int y)
        {
            int discreteMovements, sleepTime;
            double xDistance, yDistance, totalDistance, xMoveDistance, yMoveDistance, moveDistance, currentX, currentY;
            POINT startingPosition;
            GetCursorPos(out startingPosition);
            currentX = startingPosition.X;
            currentY = startingPosition.Y;
            xDistance = x - currentX;
            yDistance = y - currentY;
            totalDistance = Math.Sqrt(Math.Pow(xDistance, 2.0) + Math.Pow(yDistance, 2.0));
            moveDistance = MOUSE_MOVE_SPEED / MOUSE_MOVE_RATE;
            discreteMovements = (int) (totalDistance / moveDistance);
            xMoveDistance = xDistance / discreteMovements;
            yMoveDistance = yDistance / discreteMovements;
            sleepTime = (int) (1000.0 / MOUSE_MOVE_RATE);   //milliseconds per mouse movement

            Spline spline = new Spline(startingPosition, new System.Drawing.Point(x, y));

            for (int i = 0; i < discreteMovements; i++)
            {
                currentX += xMoveDistance;
                currentY += yMoveDistance;
                User32.SetCursorPos((int) currentX, (int) currentY);
                Thread.Sleep(sleepTime);
            }
            User32.SetCursorPos(x, y);
        }

        /// <summary>
        /// Moves a mouse across a screen like a human would
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void NaturalMove(int x, int y)
        {
            int discreteMovements, sleepTime;
            double xDistance, yDistance, totalDistance, xMoveDistance, yMoveDistance, moveDistance, currentX, currentY, mouseMoveSpeed;
            POINT startingPosition;
            Stopwatch watch = new Stopwatch();
            Random rng = new Random();
            GetCursorPos(out startingPosition);
            currentX = startingPosition.X;
            currentY = startingPosition.Y;
            xDistance = x - currentX;
            yDistance = y - currentY;
            totalDistance = Math.Sqrt(Math.Pow(xDistance, 2.0) + Math.Pow(yDistance, 2.0));
            mouseMoveSpeed = Math.Max(1.0, rng.Next((int)(0.6 * MOUSE_MOVE_SPEED), (int)(1.6 * MOUSE_MOVE_SPEED)));
            moveDistance = mouseMoveSpeed / MOUSE_MOVE_RATE;
            discreteMovements = (int)(totalDistance / moveDistance);
            xMoveDistance = xDistance / discreteMovements;
            yMoveDistance = yDistance / discreteMovements;
            sleepTime = (int)(1000.0 / MOUSE_MOVE_RATE);   //milliseconds per mouse movement

            Spline spline = new Spline(startingPosition, new System.Drawing.Point(x, y));

            for (int i = 0; i < discreteMovements; i++)
            {
                watch.Restart();
                currentX += xMoveDistance;
                currentY = spline.GetValue(currentX);
                User32.SetCursorPos((int)currentX, (int)currentY);
                watch.Stop();
                Thread.Sleep(sleepTime - (int)watch.ElapsedMilliseconds);
            }
            Move(x, y);
        }
        #endregion
    }
}
