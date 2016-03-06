using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class LineMapObjectTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var lineMapObject = new LineMapObject(0, 1, 2, 3);
            Assert.AreEqual(0, lineMapObject.X1);
            Assert.AreEqual(1, lineMapObject.Y1);
            Assert.AreEqual(2, lineMapObject.X2);
            Assert.AreEqual(3, lineMapObject.Y2);
            Assert.AreEqual(1, lineMapObject.X);
            Assert.AreEqual(2, lineMapObject.Y);
            Assert.AreEqual(0, lineMapObject.Left);
            Assert.AreEqual(1, lineMapObject.Top);
            Assert.AreEqual(2, lineMapObject.Right);
            Assert.AreEqual(3, lineMapObject.Bottom);
        }
    }
}
