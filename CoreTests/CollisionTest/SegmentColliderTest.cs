using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public class SegmentColliderTest
    {
        [TestMethod]
        public void SetSegmentTest()
        {
            var collider = new SegmentCollider(null);
            var listener = new Map(1000.0, 1000.0);
            collider.SetSegment(new Segment2D(100.0, 200.0, 300.0, 400.0), listener);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "SegmentCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "SegmentCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "SegmentCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "SegmentCollider.Bottomが不正");

            // 始点と終点を入れ替えても上下左右は同じ
            collider.SetSegment(new Segment2D(350.0, 450.0, 150.0, 250.0), listener);
            Assert.AreEqual(150.0, collider.Left, 0.0001, "始点と終点を入れ替えたSegmentCollider.Leftが不正");
            Assert.AreEqual(250.0, collider.Top, 0.0001, "始点と終点を入れ替えたSegmentCollider.Topが不正");
            Assert.AreEqual(350.0, collider.Right, 0.0001, "始点と終点を入れ替えたSegmentCollider.Rightが不正");
            Assert.AreEqual(450.0, collider.Bottom, 0.0001, "始点と終点を入れ替えたSegmentCollider.Bottomが不正");
        }
    }
}
