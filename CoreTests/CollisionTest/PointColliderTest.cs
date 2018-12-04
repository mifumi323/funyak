using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public class PointColliderTest
    {
        [TestMethod]
        public void SetPointTest()
        {
            var collider = new PointCollider(null);
            var listener = new Map(1000.0, 1000.0);
            collider.SetPoint(100.0, 200.0, listener);
            Assert.AreEqual(100.0, collider.X, 0.0001, "PointCollider.Xが不正");
            Assert.AreEqual(200.0, collider.Y, 0.0001, "PointCollider.Yが不正");
            Assert.AreEqual(100.0, collider.Left, 0.0001, "PointCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "PointCollider.Topが不正");
            Assert.AreEqual(100.0, collider.Right, 0.0001, "PointCollider.Rightが不正");
            Assert.AreEqual(200.0, collider.Bottom, 0.0001, "PointCollider.Bottomが不正");
        }
    }
}
