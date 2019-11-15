using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class Segment2DTest
    {
        [TestMethod]
        public void IsCrossedTest()
        {
            // 交差しているパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(10, 20, 20, 10);
                Assert.IsTrue(s1.IsCrossed(s2), "交差しているパターンで失敗");
            }
            // 交差していないパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 120, 120, 110);
                Assert.IsFalse(s1.IsCrossed(s2), "交差していないパターンで失敗");
            }
            // 平行なパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 10, 120, 20);
                Assert.IsFalse(s1.IsCrossed(s2), "平行なパターンで失敗");
            }
        }

        [TestMethod]
        public void TryGetCrossPointTest()
        {
            // 交差しているパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(10, 20, 20, 10);
                var result = s1.TryGetCrossPoint(s2, out var cp);
                Assert.IsTrue(result, "交差しているパターンで失敗");
                Assert.AreEqual(15, cp.X, 0.0001, "交差しているパターンで失敗");
                Assert.AreEqual(15, cp.Y, 0.0001, "交差しているパターンで失敗");
            }
            // 交差していないパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 120, 120, 110);
                var result = s1.TryGetCrossPoint(s2, out var cp);
                Assert.IsFalse(result, "交差していないパターンで失敗");
            }
            // 平行なパターン
            {
                var s1 = new Segment2D(10, 10, 20, 20);
                var s2 = new Segment2D(110, 10, 120, 20);
                var result = s1.TryGetCrossPoint(s2, out var cp);
                Assert.IsFalse(result, "平行なパターンで失敗");
            }
        }
    }
}
