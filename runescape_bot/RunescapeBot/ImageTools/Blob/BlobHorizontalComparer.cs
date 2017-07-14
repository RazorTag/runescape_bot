using System.Collections.Generic;

namespace RunescapeBot.ImageTools
{
    public class BlobHorizontalComparer : IComparer<Blob>
    {
        /// <summary>
        /// Used to sort from left to right
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Compare(Blob a, Blob b)
        {
            if (a.Center.X > b.Center.X)
            {
                return 1;
            }
            if (a.Center.X < b.Center.X)
            {
                return -1;
            }

            return 0;   //y = x
        }
    }
}
