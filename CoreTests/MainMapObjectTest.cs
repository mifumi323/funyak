using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class MainMapObjectTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var mainMapObject = new MainMapObject(0, 0);
            Assert.AreEqual(0, mainMapObject.X);
            Assert.AreEqual(0, mainMapObject.Y);
            Assert.AreEqual(-14, mainMapObject.Left);
            Assert.AreEqual(-14, mainMapObject.Top);
            Assert.AreEqual(14, mainMapObject.Right);
            Assert.AreEqual(14, mainMapObject.Bottom);
        }

        [TestMethod]
        public void PositionTest()
        {
            var mainMapObject = new MainMapObject(0, 0);
            mainMapObject.X = 100;
            mainMapObject.Y = 100;
            Assert.AreEqual(100, mainMapObject.X);
            Assert.AreEqual(100, mainMapObject.Y);
            Assert.AreEqual(86, mainMapObject.Left);
            Assert.AreEqual(86, mainMapObject.Top);
            Assert.AreEqual(114, mainMapObject.Right);
            Assert.AreEqual(114, mainMapObject.Bottom);
        }
    }
}
