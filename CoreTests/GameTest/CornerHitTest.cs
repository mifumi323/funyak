using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    [TestClass]
    public class CornerHitTest : GameTestBase
    {
        public CornerHitTest()
        {
            MapFilePath = @"TestFiles\CornerHitTest.json";
            FailOnTimeout = false;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            return false;
        }

        [TestMethod]
        public override void Run()
        {
            base.Run();
        }
    }
}
