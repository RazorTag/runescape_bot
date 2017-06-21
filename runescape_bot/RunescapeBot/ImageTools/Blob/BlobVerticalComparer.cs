using System.Collections.Generic;

namespace RunescapeBot.ImageTools
{
    public class BlobVerticalComparer : IComparer<Blob>
    {
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
