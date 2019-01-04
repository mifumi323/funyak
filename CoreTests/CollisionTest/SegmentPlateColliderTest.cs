using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public class SegmentPlateColliderTest
    {
        [TestMethod]
        public void SetSegmentTest()
        {
            var collider = new SegmentPlateCollider(null);
            collider.SetSegment(new Segment2D(100.0, 200.0, 300.0, 400.0));
            Assert.AreEqual(100.0, collider.Left, 0.0001, "SegmentPlateCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "SegmentPlateCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "SegmentPlateCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "SegmentPlateCollider.Bottomが不正");

            // 始点と終点を入れ替えても上下左右は同じ
            collider.SetSegment(new Segment2D(350.0, 450.0, 150.0, 250.0));
            Assert.AreEqual(150.0, collider.Left, 0.0001, "始点と終点を入れ替えたSegmentPlateCollider.Leftが不正");
            Assert.AreEqual(250.0, collider.Top, 0.0001, "始点と終点を入れ替えたSegmentPlateCollider.Topが不正");
            Assert.AreEqual(350.0, collider.Right, 0.0001, "始点と終点を入れ替えたSegmentPlateCollider.Rightが不正");
            Assert.AreEqual(450.0, collider.Bottom, 0.0001, "始点と終点を入れ替えたSegmentPlateCollider.Bottomが不正");
        }

        [TestMethod]
        public void IsCollidedTest()
        {
            // 交差しているパターン
            var collider = new SegmentPlateCollider(null);
            collider.SetSegment(new Segment2D(100.0, 200.0, 300.0, 400.0));

            var needle = new NeedleCollider(null);
            needle.SetSegment(new Segment2D(300.0, 200.0, 100.0, 400.0));
            var isCollided = collider.IsCollided(needle, out var collision);
            Assert.IsTrue(isCollided);
            Assert.AreEqual(collider, collision.Plate);
            Assert.AreEqual(needle, collision.Needle);
            Assert.AreEqual(200.0, collision.CrossPoint.X, 0.0001);
            Assert.AreEqual(300.0, collision.CrossPoint.Y, 0.0001);
        }

        [TestMethod]
        public void NotIsCollidedTest()
        {
            // 交差していないパターン
            var collider = new SegmentPlateCollider(null);
            collider.SetSegment(new Segment2D(100.0, 200.0, 300.0, 400.0));

            var needle = new NeedleCollider(null);
            needle.SetSegment(new Segment2D(300.0, 200.0, 300.0, 300.0));
            var isCollided = collider.IsCollided(needle, out var collision);
            Assert.IsFalse(isCollided);
        }
    }
}
