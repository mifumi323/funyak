using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class NeedleCollider : ColliderBase
    {
        private Vector2D startPoint;
        private Vector2D directedLength;
        private double startMargin;
        private double endMargin;

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
        /// 実際の当たり判定が始点方向に広がる大きさを設定・取得します。
        /// </summary>
        public double StartMargin
        {
            get => startMargin;
            set
            {
                startMargin = value;
                UpdateNeedle();
            }
        }

        /// <summary>
        /// 実際の当たり判定が終点方向に広がる大きさを設定・取得します。
        /// </summary>
        public double EndMargin
        {
            get => endMargin;
            set
            {
                endMargin = value;
                UpdateNeedle();
            }
        }

        public Segment2D ActualNeedle { get; private set; }

        /// <summary>
        /// 衝突時に呼ばれるコールバックを指定します。
        /// </summary>
        public PlateNeedleCollision.Listener OnCollided { get; set; }

        public NeedleCollider(MapObjectBase owner) : base(owner) { }

        private void UpdateNeedle()
        {
            Needle = new Segment2D(StartPoint, EndPoint);
            if (StartMargin != 0.0 || EndMargin != 0.0)
            {
                var length = DirectedLength.Length;
                var actualStartPoint = StartPoint - DirectedLength * (StartMargin / length);
                var actualEndPoint = EndPoint + DirectedLength * (EndMargin / length);
                ActualNeedle = new Segment2D(actualStartPoint, actualEndPoint);
            }
            else
            {
                ActualNeedle = Needle;
            }
            UpdatePosition(ActualNeedle);
        }

        /// <summary>
        /// 設定できるプロパティをまとめて設定します。複数のプロパティを変更する場合、このメソッドを使用してください。
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="directedLength"></param>
        /// <param name="startMargin"></param>
        /// <param name="endMargin"></param>
        public void Set(Vector2D startPoint, Vector2D directedLength, double startMargin = 0.0, double endMargin = 0.0)
        {
            this.startPoint = startPoint;
            this.directedLength = directedLength;
            this.startMargin = startMargin;
            this.endMargin = endMargin;
            UpdateNeedle();
        }

        public void SetSegment(Segment2D segment) => Set(segment.Start, segment.End - segment.Start);

        public override void Shift(double dx, double dy) => StartPoint = new Vector2D(startPoint.X + dx, startPoint.Y + dy);
    }
}
