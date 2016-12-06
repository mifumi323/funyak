using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.MapEnvironment;

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

            Assert.AreEqual(map.FindAreaEnvironment("knownName"), namedArea, "既知の名前による局所的環境の検索に失敗");
            Assert.AreEqual(map.FindAreaEnvironment("unknownName"), null, "未知の名前による局所的環境の検索に失敗");
        }
    }
}
