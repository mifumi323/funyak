using System.IO;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class RunOnSlopeTest : GameTestBase
    {
        readonly FunyaMapObject[] mapObject = new FunyaMapObject[4];
        readonly FunyaMapObjectState[] state = new FunyaMapObjectState[4];
        readonly int[] stable = new int[4];

        public RunOnSlopeTest()
        {
            MapFilePath = "RunOnSlopeTest.json";
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
                mapObject[i] = Map.FindMapObject(i.ToString()) as FunyaMapObject;
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

        [Test]
        public override void Run()
        {
            base.Run();
        }
    }
}
