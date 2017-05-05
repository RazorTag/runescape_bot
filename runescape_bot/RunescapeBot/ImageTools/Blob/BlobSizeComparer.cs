using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public class BlobSizeComparer : IComparer<Blob>
    {
        public int Compare(Blob x, Blob y)
        {
            if (x.Size > y.Size)
            {
                return 1;
            }
            if (x.Size < y.Size)
            {
                return -1;
            }

            //y = x
            return 0;
        }
    }
}
