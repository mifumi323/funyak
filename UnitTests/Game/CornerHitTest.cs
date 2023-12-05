using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class CornerHitTest : GameTestBase
    {
        public CornerHitTest()
        {
            MapFilePath = "CornerHitTest.yml";
            FailOnTimeout = false;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            var a = Map.FindMapObject("A");
            var b = Map.FindMapObject("B");
            var c = Map.FindMapObject("C");
            var d = Map.FindMapObject("D");
            var x = Map.FindMapObject("X") as RegionMapObject;
            Assert.IsNotNull(a, "Aが見つかりません。");
            Assert.IsNotNull(b, "Bが見つかりません。");
            Assert.IsNotNull(c, "Cが見つかりません。");
            Assert.IsNotNull(d, "Dが見つかりません。");
            Assert.IsNotNull(x, "Xが見つかりません。");
            Assert.IsFalse(new Vector2D(a.X, a.Y).In(x.Collider), "Aがすり抜けた！");
            Assert.IsFalse(new Vector2D(b.X, b.Y).In(x.Collider), "Bがすり抜けた！");
            Assert.IsFalse(new Vector2D(c.X, c.Y).In(x.Collider), "Cがすり抜けた！");
            Assert.IsFalse(new Vector2D(d.X, d.Y).In(x.Collider), "Dがすり抜けた！");
            return false;
        }

        [Test]
        public override void Run()
        {
            base.Run();
        }
    }
}
