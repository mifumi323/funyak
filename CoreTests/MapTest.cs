using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.MapEnvironment;
using MifuminSoft.funyak.MapObject;

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

        [TestMethod]
        public void FindMapObjectTest()
        {
            var map = new Map(320, 224);
            var namedMapObject = new MainMapObject(80, 112)
            {
                Name = "knownName",
            };
            map.AddMapObject(namedMapObject);
            var unnamedMapObject = new MainMapObject(240, 112)
            {
            };
            map.AddMapObject(unnamedMapObject);

            Assert.AreEqual(namedMapObject, map.FindMapObject("knownName"), "既知の名前によるマップオブジェクトの検索に失敗");
            Assert.AreEqual(null, map.FindAreaEnvironment("unknownName"), "未知の名前によるマップオブジェクトの検索に失敗");
        }

        [TestMethod]
        public void FindAreaEnvironmentTest()
        {
            var map = new Map(320, 224);
            var namedArea = new AreaEnvironment()
            {
                Name = "knownName",
            };
            map.AddAreaEnvironment(namedArea);
            var unnamedArea = new AreaEnvironment()
            {
            };
            map.AddAreaEnvironment(unnamedArea);

            Assert.AreEqual(namedArea, map.FindAreaEnvironment("knownName"), "既知の名前による局所的環境の検索に失敗");
            Assert.AreEqual(null, map.FindAreaEnvironment("unknownName"), "未知の名前による局所的環境の検索に失敗");
        }
    }
}
