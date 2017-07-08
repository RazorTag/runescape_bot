using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools.Tests
{
    [TestClass()]
    public class BlobTests
    {
        private Blob GenerateTestBlobRectangle(int width, int height)
        {
            Blob blob = new Blob();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    blob.AddPixel(new Point(x, y));
                }
            }

            return blob;
        }

        [TestMethod()]
        public void CenterTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.IsTrue(blob.Center.Equals(new Point(4, 2)));
        }

        [TestMethod()]
        public void SizeTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.Size, 45);
        }

        [TestMethod()]
        public void WidthTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.Width, 9);
        }

        [TestMethod()]
        public void HeightTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.Height, 5);
        }

        [TestMethod()]
        public void LeftBoundTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.LeftBound, 0);
        }

        [TestMethod()]
        public void RightBoundTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.RightBound, 8);
        }

        [TestMethod()]
        public void TopBoundTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.TopBound, 0);
        }

        [TestMethod()]
        public void BottomBoundTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Assert.AreEqual(blob.BottomBound, 4);
        }

        [TestMethod()]
        public void GetTopTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Point top = blob.GetTop();
            Assert.IsTrue(top.Equals(new Point(4, 0)));
        }

        [TestMethod()]
        public void GetBottomTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Point bottom = blob.GetBottom();
            Assert.IsTrue(bottom.Equals(new Point(4, 4)));
        }

        [TestMethod()]
        public void GetLeftTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Point left = blob.GetLeft();
            Assert.IsTrue(left.Equals(new Point(0, 2)));
        }

        [TestMethod()]
        public void GetRightTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Point right = blob.GetRight();
            Assert.IsTrue(right.Equals(new Point(8, 2)));
        }

        [TestMethod()]
        public void GetTopLeftTest()
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            Point topLeft = blob.GetTopLeft();
            Assert.IsTrue(topLeft.Equals(new Point(0, 0)));
        }

        [TestMethod()]
        [DataRow(0, 0, 4.472136)]
        [DataRow(54, 67, 82.006097)]
        public void DistanceToTest(int x, int y, double expectedDistance)
        {
            Blob blob = GenerateTestBlobRectangle(9, 5);
            double distance = blob.DistanceTo(new Point(x, y));
            Assert.AreEqual(expectedDistance, distance, 0.001);
        }
    }
}