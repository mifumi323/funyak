using NUnit.Framework;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public sealed class ColliderCollectionTest
    {
        [Test]
        public void CollideTest()
        {
            var colliderCollection = new ColliderCollection();

            var line1 = new LineMapObject(0, 0, 0, 0);
            var segment = new SegmentPlateCollider(line1);
            var segmentCollided = false;
            segment.SetSegment(new Segment2D(100.0, 200.0, 300.0, 200.0));
            segment.PlateInfo.Flags = PlateAttributeFlag.HitUpper;
            segment.OnCollided = (ref PlateNeedleCollision collision) =>
            {
                segmentCollided = true;
            };
            colliderCollection.Add(segment);

            var line2 = new LineMapObject(0, 0, 0, 0);
            var needle = new NeedleCollider(line2);
            var needleCollided = false;
            needle.SetSegment(new Segment2D(200.0, 100.0, 200.0, 300.0));
            needle.Reactivities = PlateAttributeFlag.HitUpper;
            needle.OnCollided = (ref PlateNeedleCollision collision) =>
            {
                needleCollided = true;
            };
            colliderCollection.Add(needle);

            colliderCollection.Collide();
            Assert.IsTrue(segmentCollided);
            Assert.IsTrue(needleCollided);
        }

        [Test]
        public void CollideDifferentFlagTest()
        {
            var colliderCollection = new ColliderCollection();

            var line1 = new LineMapObject(0, 0, 0, 0);
            var segment = new SegmentPlateCollider(line1);
            var segmentCollided = false;
            segment.SetSegment(new Segment2D(100.0, 200.0, 300.0, 200.0));
            segment.PlateInfo.Flags = PlateAttributeFlag.HitUpper;
            segment.OnCollided = (ref PlateNeedleCollision collision) =>
            {
                segmentCollided = true;
            };
            colliderCollection.Add(segment);

            var line2 = new LineMapObject(0, 0, 0, 0);
            var needle = new NeedleCollider(line2);
            var needleCollided = false;
            needle.SetSegment(new Segment2D(200.0, 100.0, 200.0, 300.0));
            needle.Reactivities = PlateAttributeFlag.HitBelow;
            needle.OnCollided = (ref PlateNeedleCollision collision) =>
            {
                needleCollided = true;
            };
            colliderCollection.Add(needle);

            // 共通する属性がないので当たっていても「当たっていない」判定になるのが正解
            colliderCollection.Collide();
            Assert.IsFalse(segmentCollided);
            Assert.IsFalse(needleCollided);
        }
    }
}
