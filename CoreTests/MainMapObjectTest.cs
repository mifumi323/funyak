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
        }
    }
}
