using System.Collections.Generic;

namespace RunescapeBot.ImageTools
{
    public class BlobVerticalComparer : IComparer<Blob>
    {
        /// <summary>
        /// Used to sort from top to bottom
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Compare(Blob a, Blob b)
        {
            if (a.Center.Y > b.Center.Y)
            {
                return 1;
            }
            if (a.Center.Y < b.Center.Y)
            {
                return -1;
            }

            return 0;   //y = x
        }
    }
}
