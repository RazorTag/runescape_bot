using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Common
{
    public class RectangleBounds
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public RectangleBounds() { }

        public RectangleBounds(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public RectangleBounds(Rectangle rectangle)
        {
            Left = rectangle.Left;
            Right = Left + rectangle.Width;
            Top = rectangle.Top;
            Bottom = Top + rectangle.Height;
        }
    }
}
