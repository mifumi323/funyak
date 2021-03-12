using NUnit.Framework;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.UnitTests
{
    public class FunyaMapObjectTest
    {
        [Test]
        public void InitializeTest()
        {
            var mainMapObject = new FunyaMapObject(0, 0);
            Assert.AreEqual(0, mainMapObject.X);
            Assert.AreEqual(0, mainMapObject.Y);
            Assert.AreEqual(-14, mainMapObject.Left);
            Assert.AreEqual(-14, mainMapObject.Top);
            Assert.AreEqual(14, mainMapObject.Right);
            Assert.AreEqual(14, mainMapObject.Bottom);
        }

        [Test]
        public void PositionTest()
        {
            var mainMapObject = new FunyaMapObject(0, 0)
            {
                X = 100,
                Y = 100
            };
            Assert.AreEqual(100, mainMapObject.X);
            Assert.AreEqual(100, mainMapObject.Y);
            Assert.AreEqual(86, mainMapObject.Left);
            Assert.AreEqual(86, mainMapObject.Top);
            Assert.AreEqual(114, mainMapObject.Right);
            Assert.AreEqual(114, mainMapObject.Bottom);
        }
    }
}
