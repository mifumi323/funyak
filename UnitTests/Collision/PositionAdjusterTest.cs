using MifuminSoft.funyak.Collision;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class PositionAdjusterTest
    {
        [Test]
        public void AverageTest()
        {
            var adjuster = new PositionAdjusterAverage();

            // 初期状態
            Assert.IsFalse(adjuster.HasValue);
            Assert.IsFalse(adjuster.Far(0, 0, 0.01, 0, 0, 0.01)); // 何も入れなければ問答無用でfalse

            // 値を入れる
            adjuster.Add(1, 2, 3, 4, 1, 0, 0);
            adjuster.Add(8, 9, 10, 11, 0, 1, 1);

            Assert.IsTrue(adjuster.HasValue);

            Assert.AreEqual(4.5, adjuster.X, 0.01);
            Assert.AreEqual(5.5, adjuster.Y, 0.01);
            Assert.AreEqual(6.5, adjuster.VelocityX, 0.01);
            Assert.AreEqual(7.5, adjuster.VelocityY, 0.01);
            Assert.AreEqual(0.7071, adjuster.NormalX, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0.7071, adjuster.NormalY, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0.5, adjuster.Friction, 0.01);

            Assert.IsFalse(adjuster.Far(4.5, 5.5, 0.01, 6.5, 7.5, 0.01)); // ピッタリ同じ
            Assert.IsTrue(adjuster.Far(5, 5.5, 0.01, 6.5, 7.5, 0.01)); // Xが離れている
            Assert.IsTrue(adjuster.Far(4.5, 6, 0.01, 6.5, 7.5, 0.01)); // Yが離れている
            Assert.IsTrue(adjuster.Far(4.5, 5.5, 0.01, 7, 7.5, 0.01)); // VXが離れている
            Assert.IsTrue(adjuster.Far(4.5, 5.5, 0.01, 6.5, 8, 0.01)); // VYが離れている
            Assert.IsFalse(adjuster.Far(4.509, 5.509, 0.01, 6.509, 7.509, 0.01)); // 微妙に近い
        }

        [Test]
        public void HighTest()
        {
            var adjuster = new PositionAdjusterHigh();

            // 初期状態
            Assert.IsFalse(adjuster.HasValue);
            Assert.IsFalse(adjuster.Far(0, 0, 0.01, 0, 0, 0.01)); // 何も入れなければ問答無用でfalse

            // 値を入れる
            adjuster.Add(1, 2, 3, 4, 1, 0, 0);
            adjuster.Add(8, 9, 10, 11, 0, 1, 1);

            Assert.IsTrue(adjuster.HasValue);

            Assert.AreEqual(1, adjuster.X, 0.01);
            Assert.AreEqual(2, adjuster.Y, 0.01);
            Assert.AreEqual(3, adjuster.VelocityX, 0.01);
            Assert.AreEqual(4, adjuster.VelocityY, 0.01);
            Assert.AreEqual(1, adjuster.NormalX, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0, adjuster.NormalY, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0, adjuster.Friction, 0.01);

            Assert.IsFalse(adjuster.Far(1, 2, 0.01, 3, 4, 0.01)); // ピッタリ同じ
            Assert.IsTrue(adjuster.Far(1.5, 2, 0.01, 3, 4, 0.01)); // Xが離れている
            Assert.IsTrue(adjuster.Far(1, 2.5, 0.01, 3, 4, 0.01)); // Yが離れている
            Assert.IsTrue(adjuster.Far(1, 2, 0.01, 3.5, 4, 0.01)); // VXが離れている
            Assert.IsTrue(adjuster.Far(1, 2, 0.01, 3, 4.5, 0.01)); // VYが離れている
            Assert.IsFalse(adjuster.Far(1.009, 2.009, 0.01, 3.009, 4.009, 0.01)); // 微妙に近い
        }

        [Test]
        public void LowTest()
        {
            var adjuster = new PositionAdjusterLow();

            // 初期状態
            Assert.IsFalse(adjuster.HasValue);
            Assert.IsFalse(adjuster.Far(0, 0, 0.01, 0, 0, 0.01)); // 何も入れなければ問答無用でfalse

            // 値を入れる
            adjuster.Add(1, 2, 3, 4, 1, 0, 0);
            adjuster.Add(8, 9, 10, 11, 0, 1, 1);

            Assert.IsTrue(adjuster.HasValue);

            Assert.AreEqual(8, adjuster.X, 0.01);
            Assert.AreEqual(9, adjuster.Y, 0.01);
            Assert.AreEqual(10, adjuster.VelocityX, 0.01);
            Assert.AreEqual(11, adjuster.VelocityY, 0.01);
            Assert.AreEqual(0, adjuster.NormalX, 0.01); // 長さ1に正規化される
            Assert.AreEqual(1, adjuster.NormalY, 0.01); // 長さ1に正規化される
            Assert.AreEqual(1, adjuster.Friction, 0.01);

            Assert.IsFalse(adjuster.Far(8, 9, 0.01, 10, 11, 0.01)); // ピッタリ同じ
            Assert.IsTrue(adjuster.Far(8.5, 9, 0.01, 10, 11, 0.01)); // Xが離れている
            Assert.IsTrue(adjuster.Far(8, 9.5, 0.01, 10, 11, 0.01)); // Yが離れている
            Assert.IsTrue(adjuster.Far(8, 9, 0.01, 10.5, 11, 0.01)); // VXが離れている
            Assert.IsTrue(adjuster.Far(8, 9, 0.01, 10, 11.5, 0.01)); // VYが離れている
            Assert.IsFalse(adjuster.Far(8.009, 9.009, 0.01, 10.009, 11.009, 0.01)); // 微妙に近い
        }

        [Test]
        public void LeftTest()
        {
            var adjuster = new PositionAdjusterLeft();

            // 初期状態
            Assert.IsFalse(adjuster.HasValue);
            Assert.IsFalse(adjuster.Far(0, 0, 0.01, 0, 0, 0.01)); // 何も入れなければ問答無用でfalse

            // 値を入れる
            adjuster.Add(1, 2, 3, 4, 1, 0, 0);
            adjuster.Add(8, 9, 10, 11, 0, 1, 1);

            Assert.IsTrue(adjuster.HasValue);

            Assert.AreEqual(1, adjuster.X, 0.01);
            Assert.AreEqual(2, adjuster.Y, 0.01);
            Assert.AreEqual(3, adjuster.VelocityX, 0.01);
            Assert.AreEqual(4, adjuster.VelocityY, 0.01);
            Assert.AreEqual(1, adjuster.NormalX, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0, adjuster.NormalY, 0.01); // 長さ1に正規化される
            Assert.AreEqual(0, adjuster.Friction, 0.01);

            Assert.IsFalse(adjuster.Far(1, 2, 0.01, 3, 4, 0.01)); // ピッタリ同じ
            Assert.IsTrue(adjuster.Far(1.5, 2, 0.01, 3, 4, 0.01)); // Xが離れている
            Assert.IsTrue(adjuster.Far(1, 2.5, 0.01, 3, 4, 0.01)); // Yが離れている
            Assert.IsTrue(adjuster.Far(1, 2, 0.01, 3.5, 4, 0.01)); // VXが離れている
            Assert.IsTrue(adjuster.Far(1, 2, 0.01, 3, 4.5, 0.01)); // VYが離れている
            Assert.IsFalse(adjuster.Far(1.009, 2.009, 0.01, 3.009, 4.009, 0.01)); // 微妙に近い
        }

        [Test]
        public void RightTest()
        {
            var adjuster = new PositionAdjusterRight();

            // 初期状態
            Assert.IsFalse(adjuster.HasValue);
            Assert.IsFalse(adjuster.Far(0, 0, 0.01, 0, 0, 0.01)); // 何も入れなければ問答無用でfalse

            // 値を入れる
            adjuster.Add(1, 2, 3, 4, 1, 0, 0);
            adjuster.Add(8, 9, 10, 11, 0, 1, 1);

            Assert.IsTrue(adjuster.HasValue);

            Assert.AreEqual(8, adjuster.X, 0.01);
            Assert.AreEqual(9, adjuster.Y, 0.01);
            Assert.AreEqual(10, adjuster.VelocityX, 0.01);
            Assert.AreEqual(11, adjuster.VelocityY, 0.01);
            Assert.AreEqual(0, adjuster.NormalX, 0.01); // 長さ1に正規化される
            Assert.AreEqual(1, adjuster.NormalY, 0.01); // 長さ1に正規化される
            Assert.AreEqual(1, adjuster.Friction, 0.01);

            Assert.IsFalse(adjuster.Far(8, 9, 0.01, 10, 11, 0.01)); // ピッタリ同じ
            Assert.IsTrue(adjuster.Far(8.5, 9, 0.01, 10, 11, 0.01)); // Xが離れている
            Assert.IsTrue(adjuster.Far(8, 9.5, 0.01, 10, 11, 0.01)); // Yが離れている
            Assert.IsTrue(adjuster.Far(8, 9, 0.01, 10.5, 11, 0.01)); // VXが離れている
            Assert.IsTrue(adjuster.Far(8, 9, 0.01, 10, 11.5, 0.01)); // VYが離れている
            Assert.IsFalse(adjuster.Far(8.009, 9.009, 0.01, 10.009, 11.009, 0.01)); // 微妙に近い
        }
    }
}
