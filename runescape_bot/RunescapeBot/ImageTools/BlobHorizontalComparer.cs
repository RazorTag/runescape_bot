using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public class BlobHorizontalComparer : IComparer<Blob>
    {
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

            //y = x
            return 0;
        }
    }
}
