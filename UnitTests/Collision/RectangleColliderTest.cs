﻿using NUnit.Framework;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class RectangleColliderTest
    {
        [Test]
        public void SetPositionTest()
        {
            var collider = new RectangleCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "RectangleCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "RectangleCollider.Topが不正");
            Assert.AreEqual(300.0, collider.Right, 0.0001, "RectangleCollider.Rightが不正");
            Assert.AreEqual(400.0, collider.Bottom, 0.0001, "RectangleCollider.Bottomが不正");
        }

        [Test]
        public void ShiftTest()
        {
            var collider = new RectangleCollider(null);
            collider.SetPosition(100.0, 200.0, 300.0, 400.0);
            collider.Shift(10.0, 20.0);
            Assert.AreEqual(110.0, collider.Left, 0.0001, "RectangleCollider.Leftが不正");
            Assert.AreEqual(220.0, collider.Top, 0.0001, "RectangleCollider.Topが不正");
            Assert.AreEqual(310.0, collider.Right, 0.0001, "RectangleCollider.Rightが不正");
            Assert.AreEqual(420.0, collider.Bottom, 0.0001, "RectangleCollider.Bottomが不正");
        }

        [Test]
        public void ContainsTest()
        {
            var collider = new RectangleCollider(null);
            collider.SetPosition(200.0, 300.0, 400.0, 500.0);
            var testCases = new[]
            {
                // 真ん中
                new { X = 300.0, Y = 400.0, Expects = true },
                // 四隅
                new { X = 200.0, Y = 300.0, Expects = true },
                new { X = 400.0, Y = 300.0, Expects = true },
                new { X = 400.0, Y = 500.0, Expects = true },
                new { X = 200.0, Y = 500.0, Expects = true },
                // 辺上
                new { X = 200.0, Y = 400.0, Expects = true },
                new { X = 300.0, Y = 300.0, Expects = true },
                new { X = 400.0, Y = 400.0, Expects = true },
                new { X = 300.0, Y = 500.0, Expects = true },
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
