using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class Vector2DTest
    {
        [TestMethod]
        public void XYTest()
        {
            var v = new Vector2D(1.0, 2.0);
            Assert.AreEqual(1.0, v.X, 0.0001);
            Assert.AreEqual(2.0, v.Y, 0.0001);
        }

        [TestMethod]
        public void AddTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v2 = new Vector2D(3.0, 5.0);
            var v = v1 + v2;
            Assert.AreEqual(4.0, v.X, 0.0001);
            Assert.AreEqual(7.0, v.Y, 0.0001);
        }

        [TestMethod]
        public void SubTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v2 = new Vector2D(3.0, 5.0);
            var v = v1 - v2;
            Assert.AreEqual(-2.0, v.X, 0.0001);
            Assert.AreEqual(-3.0, v.Y, 0.0001);
        }

        [TestMethod]
        public void MulTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v = v1 * 3.0;
            Assert.AreEqual(3.0, v.X, 0.0001);
            Assert.AreEqual(6.0, v.Y, 0.0001);
        }

        [TestMethod]
        public void DivTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v = v1 / 4.0;
            Assert.AreEqual(0.25, v.X, 0.0001);
            Assert.AreEqual(0.5, v.Y, 0.0001);
        }

        [TestMethod]
        public void NegativeTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v = -v1;
            Assert.AreEqual(-1.0, v.X, 0.0001);
            Assert.AreEqual(-2.0, v.Y, 0.0001);
        }

        [TestMethod]
        public void DotTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v2 = new Vector2D(3.0, 5.0);
            var d = v1.Dot(v2);
            Assert.AreEqual(13.0, d, 0.0001);
        }

        [TestMethod]
        public void CrossTest()
        {
            var v1 = new Vector2D(1.0, 2.0);
            var v2 = new Vector2D(3.0, 5.0);
            var c = v1.Cross(v2);
            Assert.AreEqual(-1.0, c, 0.0001);
        }

        [TestMethod]
        public void LengthSqTest()
        {
            var v1 = new Vector2D(3.0, 4.0);
            var ls = v1.LengthSq;
            Assert.AreEqual(25.0, ls, 0.0001);
        }

        [TestMethod]
        public void LengthTest()
        {
            {
                var v1 = new Vector2D(0.0, 1.0);
                var l = v1.Length;
                Assert.AreEqual(1.0, l, 0.0001);
            }
            {
                var v1 = new Vector2D(2.0, 0.0);
                var l = v1.Length;
                Assert.AreEqual(2.0, l, 0.0001);
            }
            {
                var v1 = new Vector2D(3.0, 4.0);
                var l = v1.Length;
                Assert.AreEqual(5.0, l, 0.0001);
            }
        }

        [TestMethod]
        public void GetNormTest()
        {
            var v1 = new Vector2D(3.0, 4.0);
            var v = v1.GetNorm();
            Assert.AreEqual(0.6, v.X, 0.0001);
            Assert.AreEqual(0.8, v.Y, 0.0001);
        }

        [TestMethod]
        public void NormTest()
        {
            var v = new Vector2D(4.0, 3.0);
            v.Norm();
            Assert.AreEqual(0.8, v.X, 0.0001);
            Assert.AreEqual(0.6, v.Y, 0.0001);
        }
    }
}
