using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Core.CollisionHelper;
using MifuminSoft.funyak.Core.Input;

namespace MifuminSoft.funyak.Core.MapObject
{
    /// <summary>
    /// 主人公のマップオブジェクトの状態
    /// </summary>
    public enum MainMapObjectState
    {
        Standing,
        Floating,
    }

    /// <summary>
    /// 主人公のマップオブジェクト
    /// </summary>
    public class MainMapObject : IDynamicMapObject
    {
        #region 主人公の状態

        /// <summary>
        /// 浮遊モードであるか否か
        /// </summary>
        public bool Floating { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public MainMapObjectState State { get; set; }

        #endregion

        #region 主人公の位置

        /// <summary>
        /// 入力
        /// </summary>
        public IInput Input { get; set; }

        /// <summary>
        /// X座標
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 速度のX成分
        /// </summary>
        public double VelocityX { get; set; }

        /// <summary>
        /// 速度のY成分
        /// </summary>
        public double VelocityY { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 角速度
        /// </summary>
        public double AngularVelocity { get; set; }

        /// <summary>
        /// 前フレームのX座標
        /// </summary>
        public double PreviousX { get; set; }

        /// <summary>
        /// 前フレームのY座標
        /// </summary>
        public double PreviousY { get; set; }

        /// <summary>
        /// 前フレームの速度のX成分
        /// </summary>
        public double PreviousVelocityX { get; set; }

        /// <summary>
        /// 前フレームの速度のY成分
        /// </summary>
        public double PreviousVelocityY { get; set; }

        public double Left
        {
            get
            {
                return GetLeft(X);
            }
        }

        public double GetLeft(double x)
        {
            return x - 14;
        }

        public double Top
        {
            get
            {
                return GetTop(Y);
            }
        }

        public double GetTop(double y)
        {
            return y - 14;
        }

        public double Right
        {
            get
            {
                return GetRight(X);
            }
        }

        public double GetRight(double x)
        {
            return x + 14;
        }

        public double Bottom
        {
            get
            {
                return GetBottom(Y);
            }
        }

        public double GetBottom(double y)
        {
            return y + 14;
        }

        #endregion

        #region パラメータ

        /// <summary>
        /// 外見を示す値
        /// </summary>
        public int Appearance { get; set; }

        /// <summary>
        /// 浮遊加速度
        /// </summary>
        public double FloatingAccel { get; set; } = 0.16;

        /// <summary>
        /// 浮遊摩擦係数
        /// </summary>
        public double FloatingFriction { get; set; } = 0.0173;

        /// <summary>
        /// 回転加速度
        /// </summary>
        public double AngularAccel { get; set; } = 0.234375;

        /// <summary>
        /// 回転摩擦係数
        /// </summary>
        public double AngularFriction { get; set; } = 0.00667;

        #endregion

        /// <summary>
        /// 場所を指定して主人公のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public MainMapObject(double x, double y)
        {
            State = MainMapObjectState.Floating;
            Floating = true;

            Input = new NullInput();
            X = x;
            Y = y;
            VelocityX = 0;
            VelocityY = 0;
        }

        public void UpdateSelf(UpdateMapObjectArgs args)
        {
            if (Floating)
            {
                UpdateSelfFloating(args);
            }
            else
            {
                UpdateSelfNormal(args);
            }
        }

        /// <summary>
        /// 浮遊状態の状態更新
        /// </summary>
        private void UpdateSelfFloating(UpdateMapObjectArgs args)
        {
            // パラメータの保持
            var wind = args.GetWind(X, Y);

            // 角度の処理
            var adx = X - PreviousX;
            var ady = Y - PreviousY;
            var addx = VelocityX - PreviousVelocityX;
            var addy = VelocityY - PreviousVelocityY;
            AngularVelocity += (adx * addy - ady * addx) * AngularAccel - AngularVelocity * AngularFriction;
            Angle += AngularVelocity;
            if (Angle > 180) Angle -= 360;
            if (Angle < -180) Angle += 360;

            // 変化前の値を保持
            PreviousX = X;
            PreviousY = Y;
            PreviousVelocityX = VelocityX;
            PreviousVelocityY = VelocityY;

            // 動作本体
            switch (State)
            {
                case MainMapObjectState.Floating:
                    double accelX = Input.X * FloatingAccel;
                    double accelY = Input.Y * FloatingAccel;
                    double frictionX = (VelocityX - wind) * FloatingFriction;
                    double frictionY = VelocityY * FloatingFriction;
                    VelocityX += accelX - frictionX;
                    VelocityY += accelY - frictionY;
                    X += VelocityX;
                    Y += VelocityY;
                    break;
                default:
                    throw new Exception("MainMapObjectのStateがおかしいぞ。");
            }
        }

        /// <summary>
        /// 通常状態の状態更新
        /// </summary>
        private void UpdateSelfNormal(UpdateMapObjectArgs args)
        {
            // 角度の処理
            AngularVelocity = 0;
            Angle = 0;

            // 変化前の値を保持
            PreviousX = X;
            PreviousY = Y;
            PreviousVelocityX = VelocityX;
            PreviousVelocityY = VelocityY;
        }

        public Action CheckCollision(CheckMapObjectCollisionArgs args)
        {

            // 種類ごとに振り分ける
            var lineMapObjects = new List<LineMapObject>();
            foreach (var mapObject in args.GetMapObjects(this))
            {
                var lineMapObject = mapObject as LineMapObject;
                if (lineMapObject != null)
                {
                    lineMapObjects.Add(lineMapObject);
                }
            }

            var tempX = X;
            var tempY = Y;
            var tempVX = VelocityX;
            var tempVY = VelocityY;

            // X軸移動とY軸移動が別々に起こったものとして2回判定を行う。
            // こうすることにより、すり抜けバグを防止できる。
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    // 1回目はY軸移動前
                    tempY -= tempVY;
                }
                else
                {
                    // 2回目はY軸移動後
                    tempY += tempVY;
                }

                // 当たり判定用図形を生成
                var topSegment = new Segment2D(tempX, tempY, tempX, GetTop(tempY));
                var bottomSegment = new Segment2D(tempX, tempY, tempX, GetBottom(tempY));
                var leftSegment = new Segment2D(tempX, tempY, GetLeft(tempX), tempY);
                var rightSegment = new Segment2D(tempX, tempY, GetRight(tempX), tempY);
                var topVector = topSegment.End - topSegment.Start;
                var bottomVector = bottomSegment.End - bottomSegment.Start;
                var leftVector = leftSegment.End - leftSegment.Start;
                var rightVector = rightSegment.End - rightSegment.Start;
                var velocity = new Vector2D(tempVX, tempVY);

                // 線との当たり判定
                foreach (var lineMapObject in lineMapObjects)
                {
                    var lineSegment = lineMapObject.ToSegment2D();
                    var lineVector = lineSegment.End - lineSegment.Start;
                    var lineNormal = new Vector2D(lineVector.Y, -lineVector.X);
                    lineNormal.Norm();

                    // 主人公の下側と線の上側
                    if (lineMapObject.HitUpper)
                    {
                        if (Collision2D.SegmentSegment(bottomSegment, lineSegment))
                        {
                            var lineStartToCharaBottom = bottomSegment.End - lineSegment.Start;
                            var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaBottom) / lineVector.LengthSq) - bottomVector;
                            var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                            tempX = newPoint.X;
                            tempY = newPoint.Y;
                            tempVX = newVelocity.X;
                            tempVY = newVelocity.Y;
                        }
                    }

                    // 主人公の上側と線の下側
                    if (lineMapObject.HitBelow)
                    {
                        if (Collision2D.SegmentSegment(topSegment, lineSegment))
                        {
                            var lineStartToCharaBottom = topSegment.End - lineSegment.Start;
                            var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaBottom) / lineVector.LengthSq) - topVector;
                            var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                            tempX = newPoint.X;
                            tempY = newPoint.Y;
                            tempVX = newVelocity.X;
                            tempVY = newVelocity.Y;
                        }
                    }

                    // 主人公の右側と線の左側
                    if (lineMapObject.HitLeft)
                    {
                        if (Collision2D.SegmentSegment(rightSegment, lineSegment))
                        {
                            var lineStartToCharaBottom = rightSegment.End - lineSegment.Start;
                            var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaBottom) / lineVector.LengthSq) - rightVector;
                            var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                            tempX = newPoint.X;
                            tempY = newPoint.Y;
                            tempVX = newVelocity.X;
                            tempVY = newVelocity.Y;
                        }
                    }

                    // 主人公の左側と線の右側
                    if (lineMapObject.HitRight)
                    {
                        if (Collision2D.SegmentSegment(leftSegment, lineSegment))
                        {
                            var lineStartToCharaBottom = leftSegment.End - lineSegment.Start;
                            var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaBottom) / lineVector.LengthSq) - leftVector;
                            var newVelocity = velocity - lineNormal * velocity.Dot(lineNormal);
                            tempX = newPoint.X;
                            tempY = newPoint.Y;
                            tempVX = newVelocity.X;
                            tempVY = newVelocity.Y;
                        }
                    }
                }
            }

            return () =>
            {
                X = tempX;
                Y = tempY;
                VelocityX = tempVX;
                VelocityY = tempVY;
            };
        }
    }
}
