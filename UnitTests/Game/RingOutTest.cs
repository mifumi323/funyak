using System.IO;
using System.Linq;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class RingOutTest : GameTestBase
    {
        public RingOutTest()
        {
            MapFilePath = Path.Combine("TestFiles", "RingOutTest.json");
            FailOnTimeout = true;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            return new string[] { "A", "B", "C", "D" }.All(id => (Map.FindMapObject(id) as MainMapObject).State == MainMapObjectState.Die);
        }

        [Test]
        public override void Run() => base.Run();
    }
}
