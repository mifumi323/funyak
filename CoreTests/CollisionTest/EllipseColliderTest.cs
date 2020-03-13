using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public sealed class EllipseColliderTest
    {
        [TestMethod]
        public void SetPositionTest()
        {
            var collider = new EllipseCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "EllipseCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "EllipseCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "EllipseCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "EllipseCollider.Bottomが不正");
        }

        [TestMethod]
        public void ShiftTest()
        {
            var collider = new EllipseCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            collider.Shift(30.0, 60.0);
            Assert.AreEqual(130.0, collider.Left, 0.0001, "EllipseCollider.Leftが不正");
            Assert.AreEqual(260.0, collider.Top, 0.0001, "EllipseCollider.Topが不正");
            Assert.AreEqual(330.0, collider.Right, 0.0001, "EllipseCollider.Rightが不正");
            Assert.AreEqual(460.0, collider.Bottom, 0.0001, "EllipseCollider.Bottomが不正");
        }

        [TestMethod]
        public void ContainsTest()
        {
            var collider = new EllipseCollider(null);
            collider.SetPosition(200.0, 300.0, 400.0, 500.0);
            var testCases = new[]
            {
                // 真ん中
                new { X = 300.0, Y=400.0, Expects = true },
                // AABB四隅
                new { X = 200.0, Y=300.0, Expects = false },
                new { X = 400.0, Y=300.0, Expects = false },
                new { X = 400.0, Y=500.0, Expects = false },
                new { X = 200.0, Y=500.0, Expects = false },
                // AABB内接点
                new { X = 200.0, Y=400.0, Expects = true },
                new { X = 300.0, Y=300.0, Expects = true },
                new { X = 400.0, Y=400.0, Expects = true },
                new { X = 300.0, Y=500.0, Expects = true },
                // 真ん中以外の内部
                new { X = 230.0, Y=330.0, Expects = true },
                new { X = 370.0, Y=330.0, Expects = true },
                new { X = 370.0, Y=470.0, Expects = true },
                new { X = 230.0, Y=470.0, Expects = true },
                // AABB内部の楕円外部
                new { X = 220.0, Y=320.0, Expects = false },
                new { X = 380.0, Y=320.0, Expects = false },
                new { X = 380.0, Y=480.0, Expects = false },
                new { X = 220.0, Y=480.0, Expects = false },
                // 完全に外
                new { X = 100.0, Y=400.0, Expects = false },
                new { X = 300.0, Y=200.0, Expects = false },
                new { X = 500.0, Y=400.0, Expects = false },
                new { X = 300.0, Y=600.0, Expects = false },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.Expects, collider.Contains(testCase.X, testCase.Y, out _), $"({testCase.X}, {testCase.Y})で失敗");
            }
        }

        [TestMethod]
        public void ContainsWideTest()
        {
            // 真円じゃない場合
            var collider = new EllipseCollider(null);
            collider.SetPosition(400.0, 300.0, 800.0, 500.0);
            var testCases = new[]
            {
                // 真ん中
                new { X = 600.0, Y=400.0, Expects = true },
                // AABB四隅
                new { X = 400.0, Y=300.0, Expects = false },
                new { X = 800.0, Y=300.0, Expects = false },
                new { X = 800.0, Y=500.0, Expects = false },
                new { X = 400.0, Y=500.0, Expects = false },
                // AABB内接点
                new { X = 400.0, Y=400.0, Expects = true },
                new { X = 600.0, Y=300.0, Expects = true },
                new { X = 800.0, Y=400.0, Expects = true },
                new { X = 600.0, Y=500.0, Expects = true },
                // 真ん中以外の内部
                new { X = 460.0, Y=330.0, Expects = true },
                new { X = 740.0, Y=330.0, Expects = true },
                new { X = 740.0, Y=470.0, Expects = true },
                new { X = 460.0, Y=470.0, Expects = true },
                // AABB内部の楕円外部
                new { X = 440.0, Y=320.0, Expects = false },
                new { X = 760.0, Y=320.0, Expects = false },
                new { X = 760.0, Y=480.0, Expects = false },
                new { X = 440.0, Y=480.0, Expects = false },
                // 完全に外
                new { X = 300.0, Y=400.0, Expects = false },
                new { X = 600.0, Y=200.0, Expects = false },
                new { X = 900.0, Y=400.0, Expects = false },
                new { X = 600.0, Y=600.0, Expects = false },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.Expects, collider.Contains(testCase.X, testCase.Y, out _), $"({testCase.X}, {testCase.Y})で失敗");
            }
        }

        [TestMethod]
        public void ContainsWidth0Test()
        {
            var collider = new EllipseCollider(null);
            collider.SetPosition(300.0, 300.0, 300.0, 500.0);
            var testCases = new[]
            {
                // 真ん中
                new { X = 300.0, Y=400.0, Expects = true },
                // 上下
                new { X = 300.0, Y=300.0, Expects = true },
                new { X = 300.0, Y=500.0, Expects = true },
                // 外
                new { X = 100.0, Y=400.0, Expects = false },
                new { X = 300.0, Y=200.0, Expects = false },
                new { X = 500.0, Y=400.0, Expects = false },
                new { X = 300.0, Y=600.0, Expects = false },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.Expects, collider.Contains(testCase.X, testCase.Y, out _), $"({testCase.X}, {testCase.Y})で失敗");
            }
        }

        [TestMethod]
        public void ContainsHeight0Test()
        {
            var collider = new EllipseCollider(null);
            collider.SetPosition(200.0, 400.0, 400.0, 400.0);
            var testCases = new[]
            {
                // 真ん中
                new { X = 300.0, Y = 400.0, Expects = true },
                // 左右
                new { X = 200.0, Y = 400.0, Expects = true },
                new { X = 400.0, Y = 400.0, Expects = true },
                // 外
                new { X = 100.0, Y = 400.0, Expects = false },
                new { X = 300.0, Y = 200.0, Expects = false },
                new { X = 500.0, Y = 400.0, Expects = false },
                new { X = 300.0, Y = 600.0, Expects = false },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.Expects, collider.Contains(testCase.X, testCase.Y, out _), $"({testCase.X}, {testCase.Y})で失敗");
            }
        }
    }
}
