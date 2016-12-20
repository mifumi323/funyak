using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    [TestClass]
    public class RunOnSlopeTest : GameTestBase
    {
        MainMapObject[] mapObject = new MainMapObject[4];
        MainMapObjectState[] state = new MainMapObjectState[4];
        int[] stable = new int[4];

        public RunOnSlopeTest()
        {
            MapFilePath = @"TestFiles\RunOnSlopeTest.json";
            FailOnTimeout = true;
            TimeoutFrames = 600;
            var input = new ReplayInput()
            {
                FrameLength = TimeoutFrames,
            };
            input.InputLog.Add(new InputRecord(0, Keys.Left, 1.0));
            Input = input;
        }

        public override bool IsSuccess()
        {
            var stableAll = true;
            for (int i = 0; i < 4; i++)
            {
                mapObject[i] = Map.FindMapObject(i.ToString()) as MainMapObject;
                Assert.IsNotNull(mapObject[i], i.ToString() + "が見つかりません。");
                if (mapObject[i].State == state[i])
                {
                    stable[i]++;
                }
                else
                {
                    state[i] = mapObject[i].State;
                    stable[i] = 0;
                }
                if (stable[i] < 120) stableAll = false;
            }
            return stableAll;
        }

        [TestMethod]
        public override void Run()
        {
            base.Run();
        }
    }
}
