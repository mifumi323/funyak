using System;
using System.Collections.Generic;
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

        private double c2rX;
        private double c2rY;
        private double c2rVelocityX;
        private double c2rVelocityY;
        private double c2rGroundNormalX;
        private double c2rGroundNormalY;
        private double c2rGroundFriction;
        private bool c2rTouchedLeft;
        private bool c2rTouchedTop;
        private bool c2rTouchedRight;
        private bool c2rTouchedBottom;

        private IPositionAdjuster adjuster = new PositionAdjusterAverage();
        private IPositionAdjuster adjusterX = new PositionAdjusterAverage();
        private IPositionAdjuster adjusterY = new PositionAdjusterAverage();
        private IPositionAdjuster adjusterHigh = new PositionAdjusterHigh();
        private IPositionAdjuster adjusterLow = new PositionAdjusterLow();
        private IPositionAdjuster adjusterLeft = new PositionAdjusterLeft();
        private IPositionAdjuster adjusterRight = new PositionAdjusterRight();

        public override void OnJoin(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Add(centerCollider);
        }

        public override void OnLeave(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Remove(centerCollider);
        }

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            // 共通当たり判定の位置更新
            centerCollider.SetPoint(X, Y);
            CollidedGravity = double.NaN;
            CollidedWind = double.NaN;

            // 位置とか
            var x = X;
            var y = Y;
            var vx = VelocityX;
            var vy = VelocityY;
            var nx = GroundNormalX;
            var ny = GroundNormalY;
            var centerX = GetCenterX(x);
            var centerY = GetCenterY(y);
            var top = GetTop(y);
            var bottom = GetBottom(y);
            var left = GetLeft(x);
            var right = GetRight(x);

            // 暫定結果とか
            var friction = GroundFriction;
            var touchedLeft = false;
            var touchedTop = false;
            var touchedRight = false;
            var touchedBottom = false;

            // 当たり判定用図形とか
            var topSegment = new Segment2D(centerX, centerY - vy, centerX, top);
            var bottomSegment = new Segment2D(centerX, centerY - vy, centerX, bottom);
            var leftSegment = new Segment2D(centerX, centerY, left, centerY);
            var rightSegment = new Segment2D(centerX, centerY, right, centerY);
            var topVector = new Vector2D(centerX - x, top - y);
            var bottomVector = new Vector2D(centerX - x, bottom - y);
            var leftVector = new Vector2D(left - x, centerY - y);
            var rightVector = new Vector2D(right - x, centerY - y);
            var velocity = new Vector2D(vx, vy);

            // 位置調整オブジェクト
            adjuster.Reset();
            adjusterX.Reset();
            adjusterY.Reset();
            adjusterHigh.Reset();
            adjusterLow.Reset();
            adjusterLeft.Reset();
            adjusterRight.Reset();

            // 判定対象の種類ごとに振り分ける
            var collidableSegments = new List<CollidableSegment>();
            foreach (var mapObject in args.GetMapObjects(this))
            {
                if (mapObject is LineMapObject lineMapObject)
                {
                    collidableSegments.Add(lineMapObject.ToCollidableSegment());
                }

                if (mapObject is TileGridMapObject tileMapObject)
                {
                    tileMapObject.AddCollidableSegmentsToList(collidableSegments, left, top, right, bottom);
                }
            }

            // 線との当たり判定
            foreach (var collidableSegment in collidableSegments)
            {
                var lineSegment = collidableSegment.Segment;
                var lineVector = lineSegment.End - lineSegment.Start;
                var lineNormal = new Vector2D(lineVector.Y, -lineVector.X);
                var lineFriction = collidableSegment.Friction;
                lineNormal.Norm();
                var lineNormalNegative = -lineNormal;

                // 主人公の下側と線の上側
                if (collidableSegment.HitUpper)
                {
                    if (lineNormal.Y != 0)
                    {
                        var n = lineNormal.Y < 0 ? lineNormal : lineNormalNegative;
                        CheckCollisionSegment(n, bottomSegment, lineSegment, bottomVector, lineVector, x, y, velocity, lineFriction, adjusterHigh);
                    }
                }

                // 主人公の上側と線の下側
                if (collidableSegment.HitBelow)
                {
                    if (lineNormal.Y != 0)
                    {
                        var n = lineNormal.Y > 0 ? lineNormal : lineNormalNegative;
                        CheckCollisionSegment(n, topSegment, lineSegment, topVector, lineVector, x, y, velocity, lineFriction, adjusterLow);
                    }
                }

                // 主人公の右側と線の左側
                if (collidableSegment.HitLeft)
                {
                    if (lineNormal.X != 0)
                    {
                        var n = lineNormal.X < 0 ? lineNormal : lineNormalNegative;
                        CheckCollisionSegment(n, rightSegment, lineSegment, rightVector, lineVector, x, y, velocity, lineFriction, adjusterLeft);
                    }
                }

                // 主人公の左側と線の右側
                if (collidableSegment.HitRight)
                {
                    if (lineNormal.X != 0)
                    {
                        var n = lineNormal.X > 0 ? lineNormal : lineNormalNegative;
                        CheckCollisionSegment(n, leftSegment, lineSegment, leftVector, lineVector, x, y, velocity, lineFriction, adjusterRight);
                    }
                }
            }

            touchedBottom = adjusterHigh.HasValue;
            touchedTop = adjusterLow.HasValue;
            touchedRight = adjusterLeft.HasValue;
            touchedLeft = adjusterRight.HasValue;

            adjusterY.Add(adjusterHigh);
            adjusterY.Add(adjusterLow);
            if (adjusterY.Far(x, y, PositionAdjustLowerLimit, vx, vy, PositionAdjustLowerLimit))
            {
                adjuster.Add(adjusterY);
            }
            adjusterX.Add(adjusterLeft);
            adjusterX.Add(adjusterRight);
            if (adjusterX.Far(x, y, PositionAdjustLowerLimit, vx, vy, PositionAdjustLowerLimit))
            {
                adjuster.Add(adjusterX);
            }
            if (adjuster.HasValue)
            {
                x = adjuster.X;
                y = adjuster.Y;
                vx = adjuster.VelocityX;
                vy = adjuster.VelocityY;
            }
            if (adjusterHigh.HasValue)
            {
                nx = adjusterHigh.NormalX;
                ny = adjusterHigh.NormalY;
                friction = adjusterHigh.Friction;
            }

            c2rX = x;
            c2rY = y;
            c2rVelocityX = vx;
            c2rVelocityY = vy;
            c2rGroundNormalX = nx;
            c2rGroundNormalY = ny;
            c2rGroundFriction = friction;
            c2rTouchedLeft = touchedLeft;
            c2rTouchedTop = touchedTop;
            c2rTouchedRight = touchedRight;
            c2rTouchedBottom = touchedBottom;
        }

        public void OnCenterCollided(ref RegionPointCollision collision)
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

        public override void RealizeCollision(RealizeCollisionArgs args)
        {
            if (Math.Abs(X - c2rX) >= PositionAdjustLowerLimit) X = c2rX;
            if (Math.Abs(Y - c2rY) >= PositionAdjustLowerLimit) Y = c2rY;
            VelocityX = c2rVelocityX;
            VelocityY = c2rVelocityY;
            GroundNormalX = c2rGroundNormalX;
            GroundNormalY = c2rGroundNormalY;
            GroundFriction = c2rGroundFriction;
            TouchedLeft = c2rTouchedLeft;
            TouchedTop = c2rTouchedTop;
            TouchedRight = c2rTouchedRight;
            TouchedBottom = c2rTouchedBottom;

            if (Right < 0 || args.MapWidth < Left || Bottom < 0 || args.MapHeight < Top)
            {
                State = FunyaMapObjectState.Die;
            }

            RealizeCollision(c2rTouchedBottom);

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
        private bool CheckCollisionSegment(Vector2D lineNormal, Segment2D charaSegment, Segment2D lineSegment, Vector2D charaVector, Vector2D lineVector, double x, double y, Vector2D velocity, double friction, IPositionAdjuster adjuster)
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
