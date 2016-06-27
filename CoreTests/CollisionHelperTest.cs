using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Core.CollisionHelper;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class CollisionHelperTest
    {
        [TestMethod]
        public void SegmentSegmentTest()
        {
            // 交差しているパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(10, 20, 20, 10);
                Assert.IsTrue(Collision2D.SegmentSegment(s1, s2), "交差しているパターンで失敗");
            }
            // 交差していないパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 120, 120, 110);
                Assert.IsFalse(Collision2D.SegmentSegment(s1, s2), "交差していないパターンで失敗");
            }
            // 平行なパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 10, 120, 20);
                Assert.IsFalse(Collision2D.SegmentSegment(s1, s2), "平行なパターンで失敗");
            }
        }

        [TestMethod]
        public void SegmentSegmentCrossPointTest()
        {
            // 交差しているパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(10, 20, 20, 10);
                var cp = Collision2D.SegmentSegmentCrossPoint(s1, s2);
                Assert.AreEqual(15, cp.X, 0.0001, "交差しているパターンで失敗");
                Assert.AreEqual(15, cp.Y, 0.0001, "交差しているパターンで失敗");
            }
            // 交差していないパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 120, 120, 110);
                Assert.IsNull(Collision2D.SegmentSegmentCrossPoint(s1, s2), "交差していないパターンで失敗");
            }
            // 平行なパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 10, 120, 20);
                Assert.IsNull(Collision2D.SegmentSegmentCrossPoint(s1, s2), "平行なパターンで失敗");
            }
        }
    }
}
