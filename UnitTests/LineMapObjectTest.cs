using NUnit.Framework;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.UnitTests
{
    public class LineMapObjectTest
    {
        [Test]
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

        [Test]
        public void PositionTest()
        {
            var lineMapObject = new LineMapObject(0, 1, 2, 3)
            {
                X1 = 10,
                Y1 = 11,
                X2 = 12,
                Y2 = 13
            };
            Assert.AreEqual(10, lineMapObject.X1);
            Assert.AreEqual(11, lineMapObject.Y1);
            Assert.AreEqual(12, lineMapObject.X2);
            Assert.AreEqual(13, lineMapObject.Y2);
            Assert.AreEqual(11, lineMapObject.X);
            Assert.AreEqual(12, lineMapObject.Y);
            Assert.AreEqual(10, lineMapObject.Left);
            Assert.AreEqual(11, lineMapObject.Top);
            Assert.AreEqual(12, lineMapObject.Right);
            Assert.AreEqual(13, lineMapObject.Bottom);
        }
    }
}
