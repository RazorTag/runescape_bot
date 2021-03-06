﻿using RunecapeBot.UITools.Spline;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using static RunescapeBot.UITools.User32;
using RunescapeBot.BotPrograms;

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
        private const double MOUSE_MOVE_RATE = 125.0;

        /// <summary>
        /// Standard minimum time in milliseconds to hover at a point before clicking
        /// </summary>
        public const int HOVER_DELAY = 150;

        /// <summary>
        /// RuneScape client process
        /// </summary>
        public static RSClient RSClient;

        /// <summary>
        /// Gets the cursor's current location in operating system coordinates
        /// </summary>
        private static Point SystemLocation
        {
            get
            {
                POINT location;
                GetCursorPos(out location);
                return location;
            }
        }

        /// <summary>
        /// Gets the cursor's current location in game screen coordinates
        /// </summary>
        public static Point Location
        {
            get
            {
                POINT location;
                GetCursorPos(out location);
                int x = location.X, y = location.Y;
                ScreenScraper.WindowToGameScreen(ref x, ref y);
                return new Point(x, y);
            }
        }

        /// <summary>
        /// Gets the cursor's current X coordinate
        /// </summary>
        public static int X { get { return Location.X; } }

        /// <summary>
        /// Gets the cursor's current Y coordiinate
        /// </summary>
        public static int Y { get { return Location.Y; } }
        #endregion

        #region click handlers
        /// <summary>
        /// Execute a left mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static Point LeftClick(int x, int y, int randomize = 0, int hoverDelay = HOVER_DELAY)
        {
            if (ScreenScraper.ProcessExists(RSClient))
            {
                return Click(x, y, MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP, hoverDelay, randomize);
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Execute a right mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static Point RightClick(int x, int y, int randomize = 0, int hoverDelay = HOVER_DELAY)
        {
            if (ScreenScraper.ProcessExists(RSClient))
            {
                return Click(x, y, MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_RIGHTUP, hoverDelay, randomize);
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Click down and release a mouse button
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="clickTypeDown"></param>
        /// <param name="clickTypeUp"></param>
        private static Point Click(int x, int y, int clickTypeDown, int clickTypeUp, int hoverDelay, int randomize)
        {
            Random rng = new Random();
            Point clickPoint = Probability.GaussianCircle(new Point(x, y), 0.35 * randomize, 0, 360, randomize);
            x = clickPoint.X;
            y = clickPoint.Y;

            ScreenScraper.BringToForeGround();
            ScreenScraper.GameScreenToWindow(ref x, ref y);
            if (BotProgram.StopFlag) { return new Point(0, 0); }
            NaturalMove(x, y);

            BotProgram.SafeWaitPlus(hoverDelay, 0.25 * hoverDelay);  //wait for RS client to recognize the cursor hover
            if (!BotProgram.StopFlag)
            {
                mouse_event(clickTypeDown, x, y, 0, 0);
                mouse_event(clickTypeUp, x, y, 0, 0);
            }

            return clickPoint;
        }

        /// <summary>
        /// Clicks at the specified point on the monitor without translating to game screen coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void RawClick(int x, int y)
        {
            NaturalMove(x, y);
            if (!BotProgram.StopFlag)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
        }

        /// <summary>
        /// Moves a mouse across a screen in a straight line
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void ForceMove(int x, int y)
        {
            int discreteMovements, sleepTime;
            double xDistance, yDistance, totalDistance, xMoveDistance, yMoveDistance, moveDistance, currentX, currentY;
            currentX = SystemLocation.X;
            currentY = SystemLocation.Y;
            xDistance = x - currentX;
            yDistance = y - currentY;
            totalDistance = Math.Sqrt(Math.Pow(xDistance, 2.0) + Math.Pow(yDistance, 2.0));
            moveDistance = MOUSE_MOVE_SPEED / MOUSE_MOVE_RATE;
            discreteMovements = (int) (totalDistance / moveDistance);
            xMoveDistance = xDistance / discreteMovements;
            yMoveDistance = yDistance / discreteMovements;
            sleepTime = (int) (1000.0 / MOUSE_MOVE_RATE);   //milliseconds per mouse movement

            for (int i = 0; i < discreteMovements; i++)
            {
                if (BotProgram.StopFlag) { return; }

                currentX += xMoveDistance;
                currentY += yMoveDistance;
                SetCursorPos((int) currentX, (int) currentY);
                Thread.Sleep(sleepTime);
            }
            SetCursorPos(x, y);
        }

        /// <summary>
        /// Move the mouse relative to its current position
        /// </summary>
        /// <param name="x">x distance to move</param>
        /// <param name="y">y distance to move</param>
        /// <param name="randomize">maximum distance by which to randomize the mouse offset</param>
        public static void Offset(int x, int y, int randomize = 100)
        {
            Random rng = new Random();
            x += SystemLocation.X + rng.Next(-randomize, randomize + 1);
            y += SystemLocation.Y + rng.Next(-randomize, randomize + 1);
            NaturalMove(x, y);
        }

        /// <summary>
        /// Moves the mouse to a random position within the specified arc and distance from the current mouse position.
        /// The arc angle starts at a heading of (1, 0)/right and goes counterclockwise.
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <param name="arcStart">Degrees (NOT radians). The possible arc goes counterclockwise from arcStart to arcEnd.</param>
        /// <param name="arcEnd">Degrees (NOT radians). The possible arc goes counterclockwise from arcStart to arcEnd.</param>
        public static void RadialOffset(double minRadius, double maxRadius, double arcStart, double arcEnd)
        {
            Random rng = new Random();
            double radius = minRadius + (rng.NextDouble() * (maxRadius - minRadius));

            arcStart = arcStart % 360;
            arcEnd = arcEnd % 360;
            if (arcStart >= arcEnd)
            {
                arcEnd += 360;
            }
            double angle = arcStart + (rng.NextDouble() * (arcEnd - arcStart));
            angle = (angle % 360) * ((2 * Math.PI) / 360.0);

            int x = SystemLocation.X + ((int) Math.Round(Math.Cos(angle) * radius));
            int y = SystemLocation.Y - ((int) Math.Round(Math.Sin(angle) * radius));
            NaturalMove(x, y);
        }

        /// <summary>
        /// Moves a mouse across a screen like a human would
        /// </summary>
        /// <param name="x">x-coordinate to move to</param>
        /// <param name="y">y-coordinate to move to</param>
        private static void NaturalMove(int x, int y)
        {
            Stopwatch watch = new Stopwatch();
            Point startingPosition = SystemLocation;
            double currentX = startingPosition.X;
            double currentY = startingPosition.Y;
            float slope = (float)((y - currentY) / (x - currentX));

            int sleepTime = (int)(1000.0 / MOUSE_MOVE_RATE);   //milliseconds per mouse movement
            double mouseMoveSpeed = Probability.BoundedGaussian(MOUSE_MOVE_SPEED, 0.25 * MOUSE_MOVE_SPEED, 0.1 * MOUSE_MOVE_SPEED, double.MaxValue);
            double incrementDistance = mouseMoveSpeed / MOUSE_MOVE_RATE;

            double xDistance = x - currentX;
            double yDistance = y - currentY;
            double totalDistance = Math.Sqrt(Math.Pow(xDistance, 2.0) + Math.Pow(yDistance, 2.0));
            double slowMoveDistance = Math.Min(totalDistance, Probability.BoundedGaussian(102, 10, 0, double.MaxValue));
            double moveDistance = totalDistance - slowMoveDistance;
            int discreteMovements = (int)(moveDistance / incrementDistance);
            moveDistance = discreteMovements * incrementDistance;
            slowMoveDistance = totalDistance - moveDistance;

            CubicSpline xSpline, ySpline;
            CreateParameterizedSplines(startingPosition, new Point(x, y), out xSpline, out ySpline);

            //move at normal mouse speed when far away from the target
            double completion = 0.0;
            for (int i = 1; i <= discreteMovements; i++)
            {
                if (BotProgram.StopFlag) { return; }

                watch.Restart();
                completion = (i * incrementDistance) / totalDistance;
                currentX = xSpline.Eval((float) completion);
                currentY = ySpline.Eval((float) completion);
                SetCursorPos((int)currentX, (int)currentY);
                Thread.Sleep(Math.Max(0, sleepTime - (int)watch.ElapsedMilliseconds));
            }

            //move the mouse slowly when close to the target
            double completed = completion;
            double slowIncrement = Probability.BoundedGaussian(0.5 * incrementDistance, 0.15 * incrementDistance, 0.2 * incrementDistance, 0.75 * incrementDistance);
            double slowMovements = (int)(slowMoveDistance / slowIncrement);
            for (int i = 1; i <= slowMovements; i++)
            {
                if (BotProgram.StopFlag) { return; }

                watch.Restart();
                completion = completed + ((i * slowIncrement) / totalDistance);
                currentX = xSpline.Eval((float)completion);
                currentY = ySpline.Eval((float)completion);
                SetCursorPos((int)currentX, (int)currentY);
                Thread.Sleep(Math.Max(0, sleepTime - (int)watch.ElapsedMilliseconds));
            }
            ForceMove(x, y);
        }

        /// <summary>
        /// Creates a spline for each of x and y parameterized with the fraction of progress toward the end point
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="xSpline"></param>
        /// <param name="ySpline"></param>
        public static void CreateParameterizedSplines(Point start, Point end, out CubicSpline xSpline, out CubicSpline ySpline)
        {
            const double randomization = 0.05;
            const int maxRandomAllowed = 50;
            const double newMidPointDistance = 1000.0;
            int xRandomization = Math.Min(maxRandomAllowed, Math.Abs((int)(randomization * (end.Y - start.Y))));
            int yRandomization = Math.Min(maxRandomAllowed, Math.Abs((int)(randomization * (end.X - start.X))));
            double moveDistance = Geometry.DistanceBetweenPoints(start, end);
            int midPoints = 1 + (int)(moveDistance / newMidPointDistance);

            xSpline = new CubicSpline(new Point(0, start.X), new Point(1, end.X), 0, xRandomization, midPoints);
            ySpline = new CubicSpline(new Point(0, start.Y), new Point(1, end.Y), 0, yRandomization, midPoints);
        }

        /// <summary>
        /// Moves a mouse across a screen like a human would
        /// </summary>
        /// <param name="x">x-coordinate within the game screen</param>
        /// <param name="y">y-coordinate within the game screen</param>
        public static void Move(Point? location)
        {
            if (location == null) { return; }
            Move(location.Value.X, location.Value.Y);
        }

        /// <summary>
        /// Moves a mouse across a screen like a human would
        /// </summary>
        /// <param name="x">x-coordinate within the game screen</param>
        /// <param name="y">y-coordinate within the game screen</param>
        public static void Move(int x, int y)
        {
            if (ScreenScraper.ProcessExists(RSClient))
            {
                ScreenScraper.GameScreenToWindow(ref x, ref y);
                NaturalMove(x, y);
            }
        }

        /// <summary>
        /// Fire and forget version of MoveMouse.
        /// Only use if you don't need the mouse for several seconds afterward
        /// </summary>
        /// <param name="x">x-coordinate within the game screen</param>
        /// <param name="y">y-coordinate within the game screen</param>
        public static void MoveMouseAsynchronous(int x, int y)
        {
            Thread moveMouse = new Thread(unused => Move(x, y));
            moveMouse.Start();
        }
        #endregion
    }
}
