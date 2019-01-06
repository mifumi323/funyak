using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class NeedleCollider : ColliderBase
    {
        private Vector2D startPoint;
        private Vector2D directedLength;

        /// <summary>
        /// 針の始点を設定または取得します。
        /// </summary>
        public Vector2D StartPoint
        {
            get => startPoint;
            set
            {
                startPoint = value;
                UpdateNeedle();
            }
        }

        /// <summary>
        /// 針の方向と長さを決定するベクトルを設定または取得します。
        /// </summary>
        public Vector2D DirectedLength
        {
            get => directedLength;
            set
            {
                directedLength = value;
                UpdateNeedle();
            }
        }

        /// <summary>
        /// 針の終点を取得します。StartPointとDirectedLengthから自動計算されるため、設定はできません。
        /// </summary>
        public Vector2D EndPoint => startPoint + directedLength;

        public Segment2D Needle { get; private set; }

        /// <summary>
        /// 衝突時に呼ばれるコールバックを指定します。
        /// </summary>
        public PlateNeedleCollision.Listener OnCollided { get; set; }

        public NeedleCollider(MapObjectBase owner) : base(owner) { }

        private void UpdateNeedle() => UpdatePosition(Needle = new Segment2D(StartPoint, EndPoint));

        public void Set(Vector2D startPoint, Vector2D directedLength)
        {
            this.startPoint = startPoint;
            this.directedLength = directedLength;
            UpdateNeedle();
        }

        public void SetSegment(Segment2D segment) => Set(segment.Start, segment.End - segment.Start);
    }
}
