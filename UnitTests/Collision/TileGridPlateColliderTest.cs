using System.Collections.Generic;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;
using NUnit.Framework;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class TileGridPlateColliderTest
    {
        private class TestCase
        {
            public TileGridMapObject MapObject { get; private set; }
            public string Name { get; private set; }
            public double StartX { get; private set; }
            public double StartY { get; private set; }
            public double DirectionX { get; private set; }
            public double DirectionY { get; private set; }
            public bool ExpectedIsCollided { get; private set; }
            public double ExpectedCollidedX { get; private set; }
            public double ExpectedCollidedY { get; private set; }

            public TestCase(string name, TileGridMapObject mapObject, double startX, double startY, double directionX, double directionY, bool expectedIsCollided, double expectedCollidedX, double expectedCollidedY)
            {
                Name = name;
                MapObject = mapObject;
                StartX = startX;
                StartY = startY;
                DirectionX = directionX;
                DirectionY = directionY;
                ExpectedIsCollided = expectedIsCollided;
                ExpectedCollidedX = expectedCollidedX;
                ExpectedCollidedY = expectedCollidedY;
            }
        }

        [Test]
        public void IsCollidedTest()
        {
            var chip = new TileChip()
            {
                HitBelow = true,
                HitLeft = true,
                HitRight = true,
                HitUpper = true,
            };
            var moNear = new TileGridMapObject(10, 20, 3, 3, new[]{
                chip, chip, chip,
                chip, null, chip,
                chip, chip, chip,
            });
            var moFar = new TileGridMapObject(10, 20, 3, 3, new[]{
                chip, null, chip,
                null, null, null,
                chip, null, chip,
            });
            var testCases = new[]
            {
                // 真ん中(当たらない)
                new TestCase("近・真中", moNear, 58, 68,  10,  10, false,  0,  0),
                new TestCase("遠・真中", moFar,  58, 68,  10,  10, false,  0,  0),

                // シンプルな上下左右
                new TestCase("近・真上", moNear, 58, 68,   0, -32, true,  58, 52),
                new TestCase("近・真下", moNear, 58, 68,   0,  32, true,  58, 84),
                new TestCase("近・真左", moNear, 58, 68, -32,   0, true,  42, 68),
                new TestCase("近・真右", moNear, 58, 68,  32,   0, true,  74, 68),
                new TestCase("遠・真上", moFar,  58, 68,   0, -32, false,  0,  0),
                new TestCase("遠・真下", moFar,  58, 68,   0,  32, false,  0,  0),
                new TestCase("遠・真左", moFar,  58, 68, -32,   0, false,  0,  0),
                new TestCase("遠・真右", moFar,  58, 68,  32,   0, false,  0,  0),

                new TestCase("近・斜め", moNear, 58, 68, -24, -32, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68,  24, -32, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68, -24,  80, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68,  24,  80, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68, -32, -24, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68, -32,  24, true,   0,  0),
                new TestCase("近・斜め", moNear, 58, 68,  32, -24, true,   0,  0),
                new TestCase("近・斜め", moFar,  58, 68,  32,  24, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68, -24, -32, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68,  24, -32, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68, -24,  80, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68,  24,  80, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68, -32, -24, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68, -32,  24, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68,  32, -24, true,   0,  0),
                new TestCase("遠・斜め", moFar,  58, 68,  32,  24, true,   0,  0),
            };
            foreach (var testCase in testCases)
            {
                var cc = new ColliderCollection();
                testCase.MapObject.OnJoin(null!, cc);
                var collided = false;
                PlateNeedleCollision collision = default;
                var needleCollider = new NeedleCollider(null)
                {
                    Reactivities = PlateAttributeFlag.HitBelow | PlateAttributeFlag.HitLeft | PlateAttributeFlag.HitRight | PlateAttributeFlag.HitUpper,
                    StartPoint = new Vector2D(testCase.StartX, testCase.StartY),
                    OnCollided = (ref PlateNeedleCollision c) =>
                    {
                        collided = true;
                        collision = c;
                    },
                };
                cc.Add(needleCollider);
                needleCollider.DirectedLength = new Vector2D(testCase.DirectionX, testCase.DirectionY);
                cc.Collide();
                Assert.AreEqual(testCase.ExpectedIsCollided, collided, $"{testCase.Name}({testCase.DirectionX}, {testCase.DirectionY})で接触不一致");
                if (collided)
                {
                    Assert.AreEqual(testCase.ExpectedCollidedX, collision.CrossPoint.X, $"{testCase.Name}({testCase.DirectionX}, {testCase.DirectionY})でX座標不一致");
                    Assert.AreEqual(testCase.ExpectedCollidedY, collision.CrossPoint.Y, $"{testCase.Name}({testCase.DirectionX}, {testCase.DirectionY})でY座標不一致");
                }
            }
        }
    }
}
