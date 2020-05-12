using System;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 線のマップオブジェクト
    /// </summary>
    public class LineMapObject : MapObjectBase, IBounds
    {
        private readonly SegmentPlateCollider collider;
        private CollidableSegment segment;

        /// <summary>
        /// 始点のX座標
        /// </summary>
        public double X1
        {
            get => segment.Segment.Start.X;
            set => segment.Segment.Start.X = value;
        }

        /// <summary>
        /// 始点のY座標
        /// </summary>
        public double Y1
        {
            get => segment.Segment.Start.Y;
            set => segment.Segment.Start.Y = value;
        }

        /// <summary>
        /// 終点のX座標
        /// </summary>
        public double X2
        {
            get => segment.Segment.End.X;
            set => segment.Segment.End.X = value;
        }

        /// <summary>
        /// 終点のY座標
        /// </summary>
        public double Y2
        {
            get => segment.Segment.End.Y;
            set => segment.Segment.End.Y = value;
        }

        /// <summary>
        /// 有効か否か
        /// </summary>
        public bool Active
        {
            get => collider.PlateInfo.HasFlag(PlateAttributeFlag.Active);
            set => collider.PlateInfo.SetFlag(PlateAttributeFlag.Active, value);
        }

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
        /// 色(未指定または無効な色の場合透明)
        /// </summary>
        public string? Color { get; set; }

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

        public override double X
        {
            get => (X1 + X2) / 2;
            set
            {
                var diff = value - X;
                X1 += diff;
                X2 += diff;
            }
        }
        public override double Y
        {
            get => (Y1 + Y2) / 2;
            set
            {
                var diff = value - Y;
                Y1 += diff;
                Y2 += diff;
            }
        }

        public double Left => Math.Min(X1, X2);
        public double Right => Math.Max(X1, X2);
        public double Top => Math.Min(Y1, Y2);
        public double Bottom => Math.Max(Y1, Y2);

        /// <summary>
        /// 線のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x1">始点のX座標</param>
        /// <param name="y1">始点のY座標</param>
        /// <param name="x2">終点のX座標</param>
        /// <param name="y2">終点のY座標</param>
        public LineMapObject(double x1, double y1, double x2, double y2)
        {
            collider = new SegmentPlateCollider(this);
            collider.PlateInfo.Flags = PlateAttributeFlag.Active;
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

        public override void OnJoin(Map map, CollisionManager collisionManager) => collisionManager.Add(collider);
        public override void OnLeave(Map map, CollisionManager collisionManager) => collisionManager.Remove(collider);
    }
}
