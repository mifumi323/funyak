using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    /// <summary>
    /// GameTestBase自体のテスト
    /// </summary>
    [TestClass]
    public class GameTestBaseTest : GameTestBase
    {
        public override bool IsSuccess()
        {
            return false;
        }

        [TestMethod]
        public void FailWhenMapFileIsNull()
        {
            MapFilePath = null;
            FailOnTimeout = true;
            TimeoutFrames = 1;
            try
            {
                Run();
            }
            catch (ArgumentNullException)
            {
                return;
            }
            Assert.Fail("テスト用ファイルがnullでも失敗になりませんでした。");
        }

        [TestMethod]
        public void FailWhenFailToReadMapFile()
        {
            MapFilePath = @"NoSuchFile";
            FailOnTimeout = true;
            TimeoutFrames = 1;
            try
            {
                Run();
            }
            catch (FileNotFoundException)
            {
                return;
            }
            Assert.Fail("テスト用ファイルが存在しなくても失敗になりませんでした。");
        }

        [TestMethod]
        public void FailOnTimeoutTest()
        {
            MapFilePath = @"TestFiles\GameTestBaseTest.json";
            FailOnTimeout = true;
            TimeoutFrames = 100;
            try
            {
                Run();
            }
            catch (AssertFailedException ex)
            {
                if (ex.Message.Contains("テストが規定時間以内に成功しませんでした。"))
                {
                    return;
                }
                throw;
            }
            Assert.Fail("テストが規定時間以内に成功しなくても失敗になりませんでした。");
        }

        [TestMethod]
        public void SucceedOnTimeoutTest()
        {
            MapFilePath = @"TestFiles\GameTestBaseTest.json";
            FailOnTimeout = false;
            TimeoutFrames = 100;
            try
            {
                Run();
            }
            catch (AssertFailedException ex)
            {
                if (ex.Message.Contains("テストが規定時間以内に成功しませんでした。"))
                {
                    Assert.Fail("テストが規定時間以内に失敗しなくても成功になりませんでした。");
                }
                throw;
            }
        }
    }
}
