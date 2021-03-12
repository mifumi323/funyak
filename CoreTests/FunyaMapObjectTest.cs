using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class FunyaMapObjectTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var funyaMapObject = new FunyaMapObject(0, 0);
            Assert.AreEqual(0, funyaMapObject.X);
            Assert.AreEqual(0, funyaMapObject.Y);
            Assert.AreEqual(-14, funyaMapObject.Left);
            Assert.AreEqual(-14, funyaMapObject.Top);
            Assert.AreEqual(14, funyaMapObject.Right);
            Assert.AreEqual(14, funyaMapObject.Bottom);
        }

        [TestMethod]
        public void PositionTest()
        {
            var funyaMapObject = new FunyaMapObject(0, 0)
            {
                X = 100,
                Y = 100
            };
            Assert.AreEqual(100, funyaMapObject.X);
            Assert.AreEqual(100, funyaMapObject.Y);
            Assert.AreEqual(86, funyaMapObject.Left);
            Assert.AreEqual(86, funyaMapObject.Top);
            Assert.AreEqual(114, funyaMapObject.Right);
            Assert.AreEqual(114, funyaMapObject.Bottom);
        }
    }
}
