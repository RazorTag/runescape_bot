using RunescapeBot.Common;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.ImageTools
{
    public class BlobProximityComparer : IComparer<Blob>
    {
        private Point SearchPoint;

        public BlobProximityComparer(Point searchPoint)
        {
            this.SearchPoint = searchPoint;
        }


        public int Compare(Blob a, Blob b)
        {
            double aDistance = Geometry.DistanceBetweenPoints(a.Center, SearchPoint);
            double bDistance = Geometry.DistanceBetweenPoints(b.Center, SearchPoint);

            if (aDistance > bDistance)
            {
                return 1;
            }
            if (aDistance < bDistance)
            {
                return -1;
            }

            return 0;   //a and b are equidistant from the search point
        }
    }
}
