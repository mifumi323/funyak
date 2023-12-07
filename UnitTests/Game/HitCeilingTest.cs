using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class HitCeilingTest : GameTestBase
    {
        public HitCeilingTest()
        {
            MapFilePath = "HitCeilingTest.yml";
            FailOnTimeout = false;
            TimeoutFrames = 100;
            var input = new ReplayInput()
            {
                FrameLength = TimeoutFrames,
            };
            Input = input;

            input.InputLog.Add(new InputRecord(1, Keys.Jump, 1.0));
            input.InputLog.Add(new InputRecord(2, Keys.Jump, 0.0));
        }

        public override bool IsSuccess()
        {
            var main = Map.FindMapObject("main");
            var x = Map.FindMapObject("X") as RegionMapObject;
            Assert.IsNotNull(main, "mainが見つかりません。");
            Assert.IsNotNull(x, "Xが見つかりません。");
            Assert.IsFalse(new Vector2D(main.X, main.Y).In(x.Collider), "すり抜けた！");
            return false;
        }

        [Test]
        public override void Run()
        {
            base.Run();
        }
    }
}
