using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var map = new Map(320, 224);
            Assert.AreEqual(320, map.Width);
            Assert.AreEqual(224, map.Height);
        }
    }
}
