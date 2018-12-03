using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public class RectangleColliderTest
    {
        [TestMethod]
        public void SetPositionTest()
        {
            var collider = new RectangleCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "RectangleCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "RectangleCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "RectangleCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "RectangleCollider.Bottomが不正");
        }
    }
}
