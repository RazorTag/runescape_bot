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

        /// <summary>
        /// Used to sort from closest to farthest
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
