using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
{
    public class Cluster :IEnumerable<Blob>
    {
        public Cluster()
        {
            cluster = new List<Blob>();
        }

        /// <summary>
        /// List of blobs in this cluster
        /// </summary>
        private List<Blob> cluster;

        /// <summary>
        /// Gets a blob from the cluster.
        /// </summary>
        /// <param name="index">Index of the blob to get.</param>
        /// <returns>A blob.</returns>
        public Blob this[int index]
        {
            get
            {
                return cluster[index];
            }
        }

        /// <summary>
        /// Gets the number of blobs in the cluster.
        /// </summary>
        public int Count { get { return cluster.Count; } }

        /// <summary>
        /// Gets the center of area of the centers of the cluster blobs.
        /// Weighs all of the blobs equally regardless of size.
        /// </summary>
        public Point Center
        {
            get
            {
                if (_center == null)
                {
                    if (Count == 0)
                    {
                        _center = new Point(0, 0);
                    }
                    else
                    {
                        int totalX = 0;
                        int totalY = 0;

                        foreach (Blob blob in cluster)
                        {
                            totalX += blob.Center.X;
                            totalY += blob.Center.Y;
                        }

                        _center = new Point(totalX / Count, totalY / Count);
                    }
                }

                return (Point)_center;
            }
        }
        private Point? _center;

        /// <summary>
        /// Adds a blob to the cluster
        /// </summary>
        /// <param name="blob">Blob to be added to the cluster.</param>
        public void AddBlob(Blob blob)
        {
            cluster.Add(blob);
            _center = null;
        }

        /// <summary>
        /// Combines another cluster with this cluster.
        /// </summary>
        /// <param name="otherCluster">The other cluster to combine with this one.</param>
        public void Combine(Cluster otherCluster)
        {
            foreach (Blob blob in otherCluster)
            {
                AddBlob(blob);
            }
        }

        /// <summary>
        /// Determines if a blob belongs in this cluster.
        /// </summary>
        /// <param name="blob">Blob to check against the cluster.</param>
        /// <param name="maxClusterSpread">The maximum distance that can separate subsets of a cluster.</param>
        /// <returns>True if the blob belongs in the cluster.</returns>
        public bool BlobFits(Blob blob, double maxClusterSpread)
        {
            for (int i = 0; i < Count; i++)
            {
                if (blob.DistanceTo(Center) <= maxClusterSpread)
                {
                    return true;
                }
            }

            return false;   //The free blob was not in range of any of the blobs in the cluster.
        }

        #region Enumerator

        public IEnumerator<Blob> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
