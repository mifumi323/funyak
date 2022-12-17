﻿using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 線のマップオブジェクト
    /// </summary>
    public class LineMapObject : SegmentMapObject
    {
        private readonly SegmentPlateCollider collider;
        private CollidableSegment segment;

        /// <summary>
        /// 上の当たり判定
        /// </summary>
        public bool HitUpper
        {
            get => collider.PlateInfo.HasFlag(PlateAttributeFlag.HitUpper);
            set
            {
                collider.PlateInfo.SetFlag(PlateAttributeFlag.HitUpper, value);
                segment.HitUpper = value;
            }
        }

        /// <summary>
        /// 下の当たり判定
        /// </summary>
        public bool HitBelow
        {
            get => collider.PlateInfo.HasFlag(PlateAttributeFlag.HitBelow);
            set
            {
                collider.PlateInfo.SetFlag(PlateAttributeFlag.HitBelow, value);
                segment.HitBelow = value;
            }
        }

        /// <summary>
        /// 左の当たり判定
        /// </summary>
        public bool HitLeft
        {
            get => collider.PlateInfo.HasFlag(PlateAttributeFlag.HitLeft);
            set
            {
                collider.PlateInfo.SetFlag(PlateAttributeFlag.HitLeft, value);
                segment.HitLeft = value;
            }
        }

        /// <summary>
        /// 右の当たり判定
        /// </summary>
        public bool HitRight
        {
            get => collider.PlateInfo.HasFlag(PlateAttributeFlag.HitRight);
            set
            {
                collider.PlateInfo.SetFlag(PlateAttributeFlag.HitRight, value);
                segment.HitRight = value;
            }
        }

        /// <summary>
        /// 摩擦力(1.0が通常)
        /// </summary>
        public double Friction
        {
            get => collider.PlateInfo.Friction;
            set
            {
                collider.PlateInfo.Friction = value;
                segment.Friction = value;
            }
        }

        /// <summary>
        /// 線のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x1">始点のX座標</param>
        /// <param name="y1">始点のY座標</param>
        /// <param name="x2">終点のX座標</param>
        /// <param name="y2">終点のY座標</param>
        public LineMapObject(double x1, double y1, double x2, double y2) : base()
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            collider = new SegmentPlateCollider(this);
            collider.PlateInfo.Flags = PlateAttributeFlag.None;
            collider.PlateInfo.Friction = 1.0;
            collider.SetSegment(new Segment2D(x1, y1, x2, y2));
            segment = new CollidableSegment()
            {
                Segment = new Segment2D(x1, y1, x2, y2),
                HitUpper = false,
                HitBelow = false,
                HitLeft = false,
                HitRight = false,
                Friction = 1.0,
            };
        }

        public Segment2D ToSegment2D() => segment.Segment;

        public CollidableSegment ToCollidableSegment() => segment;

        public override void OnJoin(Map map, ColliderCollection colliderCollection) => colliderCollection.Add(collider);
        public override void OnLeave(Map map, ColliderCollection colliderCollection) => colliderCollection.Remove(collider);

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            base.CheckCollision(args);
            segment.Segment = new Segment2D(X1, Y1, X2, Y2);
        }
    }
}
