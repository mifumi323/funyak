using System;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.MapObject
{
    // 当たり判定関係
    public partial class FunyaMapObject
    {
        private readonly PointCollider centerCollider;
        public RegionAttributeFlag CenterReactivities
        {
            get => centerCollider.Reactivities;
            set => centerCollider.Reactivities = value;
        }

        private readonly NeedleCollider topCollider;
        private readonly NeedleCollider bottomCollider;
        private readonly NeedleCollider leftCollider;
        private readonly NeedleCollider rightCollider;

        private readonly IPositionAdjuster adjuster = new PositionAdjusterAverage();
        private readonly IPositionAdjuster adjusterX = new PositionAdjusterAverage();
        private readonly IPositionAdjuster adjusterY = new PositionAdjusterAverage();
        private readonly IPositionAdjuster adjusterHigh = new PositionAdjusterHigh();
        private readonly IPositionAdjuster adjusterLow = new PositionAdjusterLow();
        private readonly IPositionAdjuster adjusterLeft = new PositionAdjusterLeft();
        private readonly IPositionAdjuster adjusterRight = new PositionAdjusterRight();

        public override void OnJoin(ColliderCollection colliderCollection)
        {
            colliderCollection.Add(centerCollider);
            colliderCollection.Add(topCollider);
            colliderCollection.Add(bottomCollider);
            colliderCollection.Add(leftCollider);
            colliderCollection.Add(rightCollider);
        }

        public override void OnLeave(ColliderCollection colliderCollection)
        {
            colliderCollection.Remove(centerCollider);
            colliderCollection.Remove(topCollider);
            colliderCollection.Remove(bottomCollider);
            colliderCollection.Remove(leftCollider);
            colliderCollection.Remove(rightCollider);
        }

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            // 位置とか
            var x = X;
            var y = Y;
            var vy = VelocityY;
            var centerX = GetCenterX(x);
            var centerY = GetCenterY(y);
            var top = GetTop(y);
            var bottom = GetBottom(y);
            var left = GetLeft(x);
            var right = GetRight(x);

            // 中心の共通当たり判定
            centerCollider.SetPoint(x, y);
            CollidedGravity = double.NaN;
            CollidedWind = double.NaN;

            // 上下の共通当たり判定は中心点をずらす
            topCollider.Set(new Vector2D(centerX, centerY - vy), new Vector2D(0, top - (centerY - vy)));
            bottomCollider.Set(new Vector2D(centerX, centerY - vy), new Vector2D(0, bottom - (centerY - vy)));

            // 左右の共通当たり判定は素直に中心から出す
            leftCollider.Set(new Vector2D(centerX, centerY), new Vector2D(left - centerX, 0));
            rightCollider.Set(new Vector2D(centerX, centerY), new Vector2D(right - centerX, 0));

            // 位置調整オブジェクト
            adjuster.Reset();
            adjusterX.Reset();
            adjusterY.Reset();
            adjusterHigh.Reset();
            adjusterLow.Reset();
            adjusterLeft.Reset();
            adjusterRight.Reset();
        }

        private void OnCenterCollided(ref RegionPointCollision collision)
        {
            if (collision.RegionInfo.Flags.Has(RegionAttributeFlag.Gravity))
            {
                CollidedGravity = collision.RegionInfo.Gravity;
            }
            if (collision.RegionInfo.Flags.Has(RegionAttributeFlag.Wind))
            {
                CollidedWind = collision.RegionInfo.Wind;
            }
        }

        private void OnTopCollided(ref PlateNeedleCollision collision)
        {
            OnNeedleCollided(collision, adjusterLow);
        }

        private void OnBottomCollided(ref PlateNeedleCollision collision)
        {
            OnNeedleCollided(collision, adjusterHigh);
        }

        private void OnLeftCollided(ref PlateNeedleCollision collision)
        {
            OnNeedleCollided(collision, adjusterRight);
        }

        private void OnRightCollided(ref PlateNeedleCollision collision)
        {
            OnNeedleCollided(collision, adjusterLeft);
        }

        private void OnNeedleCollided(PlateNeedleCollision collision, IPositionAdjuster adjuster)
        {
            var needleSegment = collision.Needle.Needle;
            var plateSegment = collision.PlateSegment;
            var plateVector = plateSegment.End - plateSegment.Start;
            var plateNormal = new Vector2D(plateVector.Y, -plateVector.X);
            plateNormal.Norm();
            var dot = plateNormal.Dot(needleSegment.End - needleSegment.Start);

            if (dot != 0)
            {
                var n = dot < 0 ? plateNormal : -plateNormal; // 法線は向かい合う向きに

                var velocity = new Vector2D(VelocityX, VelocityY);
                if (velocity.Dot(n) <= Segment2D.DELTA)
                {
                    var plateStartToNeedleEnd = needleSegment.End - plateSegment.Start;
                    var needleEndToCharaPoint = needleSegment.End - new Vector2D(X, Y);
                    var newPoint = plateSegment.Start + plateVector * (plateVector.Dot(plateStartToNeedleEnd) / plateVector.LengthSq) - needleEndToCharaPoint;
                    var newVelocity = velocity - n * velocity.Dot(n);
                    adjuster.Add(newPoint.X, newPoint.Y, newVelocity.X, newVelocity.Y, n.X, n.Y, collision.PlateInfo.Friction);
                }
            }
        }

        public override void RealizeCollision(RealizeCollisionArgs args)
        {
            adjusterY.Add(adjusterHigh);
            adjusterY.Add(adjusterLow);
            if (adjusterY.Far(X, Y, PositionAdjustLowerLimit, VelocityX, VelocityY, PositionAdjustLowerLimit))
            {
                adjuster.Add(adjusterY);
            }
            adjusterX.Add(adjusterLeft);
            adjusterX.Add(adjusterRight);
            if (adjusterX.Far(X, Y, PositionAdjustLowerLimit, VelocityX, VelocityY, PositionAdjustLowerLimit))
            {
                adjuster.Add(adjusterX);
            }

            var touchedAny = adjuster.HasValue;
            var touchedBottom = adjusterHigh.HasValue;
            var touchedTop = adjusterLow.HasValue;
            var touchedRight = adjusterLeft.HasValue;
            var touchedLeft = adjusterRight.HasValue;

            var x = touchedAny ? adjuster.X : X;
            var y = touchedAny ? adjuster.Y : Y;
            var vx = touchedAny ? adjuster.VelocityX : VelocityX;
            var vy = touchedAny ? adjuster.VelocityY : VelocityY;
            var nx = touchedBottom ? adjusterHigh.NormalX : GroundNormalX;
            var ny = touchedBottom ? adjusterHigh.NormalY : GroundNormalY;
            var friction = touchedBottom ? adjusterHigh.Friction : GroundFriction;

            if (Math.Abs(X - x) >= PositionAdjustLowerLimit) X = x;
            if (Math.Abs(Y - y) >= PositionAdjustLowerLimit) Y = y;
            VelocityX = vx;
            VelocityY = vy;
            GroundNormalX = nx;
            GroundNormalY = ny;
            GroundFriction = friction;
            TouchedLeft = touchedLeft;
            TouchedTop = touchedTop;
            TouchedRight = touchedRight;
            TouchedBottom = touchedBottom;

            if (Right < 0 || args.MapWidth < Left || Bottom < 0 || args.MapHeight < Top)
            {
                State = FunyaMapObjectState.Die;
            }

            RealizeCollision(touchedBottom);

            StateCounter++;
        }

        /// <summary>
        /// 衝突結果を反映
        /// </summary>
        private void RealizeCollision(bool landed)
        {
            switch (State)
            {
                case FunyaMapObjectState.Stand:
                case FunyaMapObjectState.Run:
                case FunyaMapObjectState.Walk:
                case FunyaMapObjectState.Charge:
                    if (!landed)
                    {
                        State = FunyaMapObjectState.Fall;
                    }
                    break;
                case FunyaMapObjectState.Jump:
                case FunyaMapObjectState.Fall:
                    if (landed)
                    {
                        if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.Right))
                        {
                            State = FunyaMapObjectState.Run;
                        }
                        else if (Input.IsPressed(Keys.Jump))
                        {
                            State = FunyaMapObjectState.Charge;
                        }
                        else if (Input.IsPressed(Keys.Down))
                        {
                            State = FunyaMapObjectState.Walk;
                        }
                        else
                        {
                            State = FunyaMapObjectState.Stand;
                        }
                    }
                    break;
                case FunyaMapObjectState.Float:
                case FunyaMapObjectState.BreatheIn:
                case FunyaMapObjectState.BreatheOut:
                case FunyaMapObjectState.Tired:
                case FunyaMapObjectState.Frozen:
                case FunyaMapObjectState.Damaged:
                case FunyaMapObjectState.Die:
                    break;
                default:
                    throw new Exception("FunyaMapObjectのStateがおかしいぞ。");
            }
        }

        /// <summary>
        /// 線分との当たり判定
        /// </summary>
        /// <param name="lineNormal">線分の、当たり判定を行いたい方向に向けた法線ベクトル</param>
        /// <param name="charaSegment">キャラクターの、当たり判定に用いる線分</param>
        /// <param name="lineSegment">当たり判定対象の線分</param>
        /// <param name="charaVector">キャラクターの線分の、始点から終点へのベクトル</param>
        /// <param name="lineVector">当たり判定対象の線分の、始点から終点へのベクトル</param>
        /// <param name="velocity">当たり判定対象の線分に対する、キャラクターの相対速度</param>
        /// <param name="adjuster">位置調整オブジェクト</param>
        /// <returns>当たっていたらtrue</returns>
        private bool CheckCollisionSegment(Vector2D lineNormal, Segment2D charaSegment, Segment2D lineSegment, Vector2D charaVector, Vector2D lineVector, Vector2D velocity, double friction, IPositionAdjuster adjuster)
        {
            if (velocity.Dot(lineNormal) <= Segment2D.DELTA)
            {
                if (charaSegment.IsCrossed(lineSegment))
                {
                    var lineStartToCharaEnd = charaSegment.End - lineSegment.Start;
                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaEnd) / lineVector.LengthSq) - charaVector;
                    var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                    adjuster.Add(newPoint.X, newPoint.Y, newVelocity.X, newVelocity.Y, lineNormal.X, lineNormal.Y, friction);

                    return true;
                }
            }
            return false;
        }
    }
}
