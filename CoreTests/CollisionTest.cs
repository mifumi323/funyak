using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class CollisionTest
    {
        [TestMethod]
        public void PointColliderTest()
        {
            var collider = new PointCollider(null);
            collider.SetPoint(100.0, 200.0);
            Assert.AreEqual(100.0, collider.X, 0.0001, "PointCollider.Xが不正");
            Assert.AreEqual(200.0, collider.Y, 0.0001, "PointCollider.Yが不正");
            Assert.AreEqual(100.0, collider.Left, 0.0001, "PointCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "PointCollider.Topが不正");
            Assert.AreEqual(100.0, collider.Right, 0.0001, "PointCollider.Rightが不正");
            Assert.AreEqual(200.0, collider.Bottom, 0.0001, "PointCollider.Bottomが不正");
        }

        [TestMethod]
        public void RegionColliderTest()
        {
            var collider = new RegionCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "RegionCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "RegionCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "RegionCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "RegionCollider.Bottomが不正");
        }

        [TestMethod]
        public void SegmentColliderTest()
        {
            var collider = new SegmentCollider(null);
            collider.SetSegment(new Segment2D(100.0, 200.0, 300.0, 400.0));
            Assert.AreEqual(100.0, collider.Left, 0.0001, "SegmentCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "SegmentCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "SegmentCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "SegmentCollider.Bottomが不正");

            // 始点と終点を入れ替えても上下左右は同じ
            collider.SetSegment(new Segment2D(350.0, 450.0, 150.0, 250.0));
            Assert.AreEqual(150.0, collider.Left, 0.0001, "始点と終点を入れ替えたSegmentCollider.Leftが不正");
            Assert.AreEqual(250.0, collider.Top, 0.0001, "始点と終点を入れ替えたSegmentCollider.Topが不正");
            Assert.AreEqual(350.0, collider.Right, 0.0001, "始点と終点を入れ替えたSegmentCollider.Rightが不正");
            Assert.AreEqual(450.0, collider.Bottom, 0.0001, "始点と終点を入れ替えたSegmentCollider.Bottomが不正");
        }
    }
}
