﻿using NUnit.Framework;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class NeedleColliderTest
    {
        [Test]
        public void SetTest()
        {
            var collider = new NeedleCollider(null);

            // 始点と終点をセット
            collider.Set(new Vector2D(100.0, 200.0), new Vector2D(300.0, 400.0));
            Assert.AreEqual(100.0, collider.Left, 0.0001, "NeedleCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "NeedleCollider.Topが不正");
            Assert.AreEqual(400.0, collider.Right, 0.0001, "NeedleCollider.Rightが不正");
            Assert.AreEqual(600.0, collider.Bottom, 0.0001, "NeedleCollider.Bottomが不正");

            // 移動
            collider.StartPoint += new Vector2D(300.0, 300.0);
            Assert.AreEqual(400.0, collider.Left, 0.0001, "移動したNeedleCollider.Leftが不正");
            Assert.AreEqual(500.0, collider.Top, 0.0001, "移動したNeedleCollider.Topが不正");
            Assert.AreEqual(700.0, collider.Right, 0.0001, "移動したNeedleCollider.Rightが不正");
            Assert.AreEqual(900.0, collider.Bottom, 0.0001, "移動したNeedleCollider.Bottomが不正");

            // 向きを逆転
            collider.DirectedLength = -collider.DirectedLength;
            Assert.AreEqual(100.0, collider.Left, 0.0001, "逆向きのNeedleCollider.Leftが不正");
            Assert.AreEqual(100.0, collider.Top, 0.0001, "逆向きのNeedleCollider.Topが不正");
            Assert.AreEqual(400.0, collider.Right, 0.0001, "逆向きのNeedleCollider.Rightが不正");
            Assert.AreEqual(500.0, collider.Bottom, 0.0001, "逆向きのNeedleCollider.Bottomが不正");
        }

        [Test]
        public void StartMarginTest()
        {
            var collider = new NeedleCollider(null);

            collider.Set(new Vector2D(100.0, 200.0), new Vector2D(300.0, 400.0), 1.0);
            Assert.AreEqual(100.0 - 0.6, collider.Left, 0.0001, "NeedleCollider.Leftが不正");
            Assert.AreEqual(200.0 - 0.8, collider.Top, 0.0001, "NeedleCollider.Topが不正");
            Assert.AreEqual(400.0, collider.Right, 0.0001, "NeedleCollider.Rightが不正");
            Assert.AreEqual(600.0, collider.Bottom, 0.0001, "NeedleCollider.Bottomが不正");
        }

        [Test]
        public void EndMarginTest()
        {
            var collider = new NeedleCollider(null);

            collider.Set(new Vector2D(100.0, 200.0), new Vector2D(300.0, 400.0), 0.0, 1.0);
            Assert.AreEqual(100.0, collider.Left, 0.0001, "NeedleCollider.Leftが不正");
            Assert.AreEqual(200.0, collider.Top, 0.0001, "NeedleCollider.Topが不正");
            Assert.AreEqual(400.0 + 0.6, collider.Right, 0.0001, "NeedleCollider.Rightが不正");
            Assert.AreEqual(600.0 + 0.8, collider.Bottom, 0.0001, "NeedleCollider.Bottomが不正");
        }
    }
}
