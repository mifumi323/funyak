﻿using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class PointCollider : ColliderBase
    {
        public RegionAttributeFlag Reactivities { get; set; }

        public double X => Left;
        public double Y => Top;

        /// <summary>
        /// 衝突時に呼ばれるコールバックを指定します。
        /// </summary>
        public RegionPointCollision.Listener? OnCollided { get; set; }

        public PointCollider(MapObjectBase owner) : base(owner) { }

        public void SetPoint(double x, double y) => UpdatePosition(x, y, x, y);
    }
}
