using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class TileGridPlateColliderTest
    {
        [Test]
        public void IsCollidedNearTest()
        {
            var tileGridMapObject = new TileGridMapObject(0, 0, 3, 3);
            var tileChip = new TileChip()
            {
                HitBelow = true,
                HitLeft = true,
                HitRight = true,
                HitUpper = true,
            };
            tileGridMapObject[0, 0] = tileChip;
            tileGridMapObject[1, 0] = tileChip;
            tileGridMapObject[2, 0] = tileChip;
            tileGridMapObject[0, 1] = tileChip;
            tileGridMapObject[1, 1] = null;
            tileGridMapObject[2, 1] = tileChip;
            tileGridMapObject[0, 2] = tileChip;
            tileGridMapObject[1, 2] = tileChip;
            tileGridMapObject[2, 2] = tileChip;
            var cc = new ColliderCollection();
            tileGridMapObject.OnJoin(null!, cc);
            var collided = false;
            PlateNeedleCollision? collision = null;
            var needleCollider = new NeedleCollider(null)
            {
                Reactivities = PlateAttributeFlag.HitBelow | PlateAttributeFlag.HitLeft | PlateAttributeFlag.HitRight | PlateAttributeFlag.HitUpper,
                StartPoint = new Vector2D(48.0, 48.0),
                OnCollided = (ref PlateNeedleCollision pnc) =>
                {
                    collision = pnc;
                },
            };
            cc.Add(needleCollider);
            var testCases = new[]
            {
                // 真ん中
                ( X : 10.0, Y:10.0, Expects : false ),
                // 上下左右
                ( X : 0.0, Y:-32.0, Expects : true ),
                ( X : 0.0, Y:32.0, Expects : true ),
                ( X : -32.0, Y:0.0, Expects : true ),
                ( X : 32.0, Y:0.0, Expects : true ),
                // 斜め
                ( X : -24.0, Y:-32.0, Expects : true ),
                ( X : 24.0, Y:-32.0, Expects : true ),
                ( X : -24.0, Y:80.0, Expects : true ),
                ( X : 24.0, Y:80.0, Expects : true ),
                ( X : -32.0, Y:-24.0, Expects : true ),
                ( X : -32.0, Y:24.0, Expects : true ),
                ( X : 32.0, Y:-24.0, Expects : true ),
                ( X : 32.0, Y:24.0, Expects : true ),
            };
            foreach (var (X, Y, Expects) in testCases)
            {
                needleCollider.DirectedLength = new Vector2D(X, Y);
                collided = false;
                cc.Collide();
                Assert.AreEqual(Expects, collided, $"({X}, {Y})で失敗");
            }
        }

        [Test]
        public void IsCollidedFarTest()
        {
            var tileGridMapObject = new TileGridMapObject(0, 0, 3, 3);
            var tileChip = new TileChip()
            {
                HitBelow = true,
                HitLeft = true,
                HitRight = true,
                HitUpper = true,
            };
            tileGridMapObject[0, 0] = tileChip;
            tileGridMapObject[1, 0] = null;
            tileGridMapObject[2, 0] = tileChip;
            tileGridMapObject[0, 1] = null;
            tileGridMapObject[1, 1] = null;
            tileGridMapObject[2, 1] = null;
            tileGridMapObject[0, 2] = tileChip;
            tileGridMapObject[1, 2] = null;
            tileGridMapObject[2, 2] = tileChip;
            var cc = new ColliderCollection();
            tileGridMapObject.OnJoin(null!, cc);
            var collided = false;
            var needleCollider = new NeedleCollider(null)
            {
                Reactivities = PlateAttributeFlag.HitBelow | PlateAttributeFlag.HitLeft | PlateAttributeFlag.HitRight | PlateAttributeFlag.HitUpper,
                StartPoint = new Vector2D(48.0, 48.0),
                OnCollided = (ref PlateNeedleCollision _) =>
                {
                    collided = true;
                },
            };
            cc.Add(needleCollider);
            var testCases = new[]
            {
                // 真ん中
                ( X : 10.0, Y:10.0, Expects : false ),
                // 上下左右
                ( X : 0.0, Y:-32.0, Expects : false ),
                ( X : 0.0, Y:32.0, Expects : false ),
                ( X : -32.0, Y:0.0, Expects : false ),
                ( X : 32.0, Y:0.0, Expects : false ),
                // 斜め
                ( X : -24.0, Y:-32.0, Expects : true ),
                ( X : 24.0, Y:-32.0, Expects : true ),
                ( X : -24.0, Y:80.0, Expects : true ),
                ( X : 24.0, Y:80.0, Expects : true ),
                ( X : -32.0, Y:-24.0, Expects : true ),
                ( X : -32.0, Y:24.0, Expects : true ),
                ( X : 32.0, Y:-24.0, Expects : true ),
                ( X : 32.0, Y:24.0, Expects : true ),
            };
            foreach (var (X, Y, Expects) in testCases)
            {
                needleCollider.DirectedLength = new Vector2D(X, Y);
                collided = false;
                cc.Collide();
                Assert.AreEqual(Expects, collided, $"({X}, {Y})で失敗");
            }
        }
    }
}
