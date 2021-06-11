using System;
using System.IO;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Game
{
    /// <summary>
    /// GameTestBase自体のテスト
    /// </summary>
    public class GameTestBaseTest : GameTestBase
    {
        public GameTestBaseTest()
        {
            MapFilePath = "GameTestBaseTest.yml";
            FailOnTimeout = false;
            TimeoutFrames = 100;
        }

        public override bool IsSuccess()
        {
            return false;
        }

        [Test]
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

        [Test]
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

        [Test]
        public void FailOnTimeoutTest()
        {
            MapFilePath = "GameTestBaseTest.yml";
            FailOnTimeout = true;
            TimeoutFrames = 100;
            try
            {
                Run();
            }
            catch (AssertionException ex)
            {
                if (ex.Message.Contains("テストが規定時間以内に成功しませんでした。"))
                {
                    Assert.Pass();
                    return;
                }
                throw;
            }
            Assert.Fail("テストが規定時間以内に成功しなくても失敗になりませんでした。");
        }

        [Test]
        public void SucceedOnTimeoutTest()
        {
            MapFilePath = "GameTestBaseTest.yml";
            FailOnTimeout = false;
            TimeoutFrames = 100;
            try
            {
                Run();
            }
            catch (AssertionException ex)
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
