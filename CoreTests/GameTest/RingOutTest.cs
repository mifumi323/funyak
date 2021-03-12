using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    [TestClass]
    public class RingOutTest : GameTestBase
    {
        public RingOutTest()
        {
            MapFilePath = @"TestFiles\RingOutTest.json";
            FailOnTimeout = true;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            return new string[] { "A", "B", "C", "D" }.All(id => (Map.FindMapObject(id) as FunyaMapObject).State == FunyaMapObjectState.Die);
        }

        [TestMethod]
        public override void Run() => base.Run();
    }
}
