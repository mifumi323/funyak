using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    public class SandwichedFunyaTest : GameTestBase
    {
        public SandwichedFunyaTest()
        {
            MapFilePath = "SandwichedFunyaTest.yml";
            FailOnTimeout = false;
            TimeoutFrames = 200;
        }

        public override bool IsSuccess()
        {
            var main = Map.FindMapObject("main");
            Assert.IsNotNull(main, "mainが見つかりません。");
            if (main.Y > 100)
            {
                // プルプルしたら死ぬ
                Assert.AreEqual(100, main.X, 0.1);
            }
            return false;
        }

        [Test]
        public override void Run()
        {
            base.Run();
        }
    }
}
