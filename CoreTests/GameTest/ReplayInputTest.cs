using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    [TestClass]
    public class ReplayInputTest : GameTestBase
    {
        public ReplayInputTest()
        {
            MapFilePath = @"TestFiles\ReplayInputTest.json";
            FailOnTimeout = true;
            TimeoutFrames = 15 * 60;
            var input = new ReplayInput()
            {
                FrameLength = TimeoutFrames,
            };

            input.InputLog.Add(new InputRecord(0 * 60, Keys.Left, 1.0));
            input.InputLog.Add(new InputRecord(0 * 60, Keys.Up, 0.0));

            input.InputLog.Add(new InputRecord(3 * 60, Keys.Left, 0.0));
            input.InputLog.Add(new InputRecord(3 * 60, Keys.Up, 1.0));

            input.InputLog.Add(new InputRecord(6 * 60, Keys.Right, 1.0));
            input.InputLog.Add(new InputRecord(6 * 60, Keys.Up, 0.0));

            input.InputLog.Add(new InputRecord(9 * 60, Keys.Left, 0.0));
            input.InputLog.Add(new InputRecord(9 * 60, Keys.Down, 1.0));
            Input = input;
        }

        public override bool IsSuccess()
        {
            var main = Map.FindMapObject("main");
            var goal = Map.FindAreaEnvironment("goal");
            Assert.IsNotNull(main, "mainが見つかりません。");
            Assert.IsNotNull(goal, "goalが見つかりません。");
            return Map.GetEnvironment(main.X, main.Y) == goal;
        }

        [TestMethod]
        public override void Run()
        {
            base.Run();
        }
    }
}
