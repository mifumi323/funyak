using System.IO;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class CornerHitTest : GameTestBase
    {
        public CornerHitTest()
        {
            MapFilePath = Path.Combine("TestFiles", "CornerHitTest.json");
            FailOnTimeout = false;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            var a = Map.FindMapObject("A");
            var b = Map.FindMapObject("B");
            var c = Map.FindMapObject("C");
            var d = Map.FindMapObject("D");
            var x = Map.FindAreaEnvironment("X");
            Assert.IsNotNull(a, "Aが見つかりません。");
            Assert.IsNotNull(b, "Bが見つかりません。");
            Assert.IsNotNull(c, "Cが見つかりません。");
            Assert.IsNotNull(d, "Dが見つかりません。");
            Assert.IsNotNull(x, "Xが見つかりません。");
            Assert.IsFalse(Map.GetEnvironment(a.X, a.Y) == x, "Aがすり抜けた！");
            Assert.IsFalse(Map.GetEnvironment(b.X, b.Y) == x, "Bがすり抜けた！");
            Assert.IsFalse(Map.GetEnvironment(c.X, c.Y) == x, "Cがすり抜けた！");
            Assert.IsFalse(Map.GetEnvironment(d.X, d.Y) == x, "Dがすり抜けた！");
            return false;
        }

        [Test]
        public override void Run()
        {
            base.Run();
        }
    }
}
