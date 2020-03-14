using NUnit.Framework;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class PointColliderTest
    {
        [Test]
        public void SetPointTest()
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

        [Test]
        public void ShiftTest()
        {
            var collider = new PointCollider(null);
            collider.SetPoint(100.0, 200.0);
            collider.Shift(10.0, 20.0);
            Assert.AreEqual(110.0, collider.X, 0.0001, "PointCollider.Xが不正");
            Assert.AreEqual(220.0, collider.Y, 0.0001, "PointCollider.Yが不正");
            Assert.AreEqual(110.0, collider.Left, 0.0001, "PointCollider.Leftが不正");
            Assert.AreEqual(220.0, collider.Top, 0.0001, "PointCollider.Topが不正");
            Assert.AreEqual(110.0, collider.Right, 0.0001, "PointCollider.Rightが不正");
            Assert.AreEqual(220.0, collider.Bottom, 0.0001, "PointCollider.Bottomが不正");
        }
    }
}
