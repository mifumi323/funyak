﻿using System;
using System.Collections.Generic;
using MifuminSoft.funyak.CollisionHelper;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 主人公のマップオブジェクトの状態
    /// </summary>
    public enum MainMapObjectState
    {
        Standing,
        Floating,
        Falling,
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
        /// 重力加速度
        /// </summary>
        public double GravityAccel { get; set; } = 0.4 / 3;

        /// <summary>
        /// 落下摩擦係数
        /// </summary>
        public double FallFriction { get; set; } = 0.1;

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

        /// <summary>
        /// 速度上限値
        /// </summary>
        public double VelocityLimit { get; set; } = 13;

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
            var gravity = args.GetGravity(X, Y);
            SwitchFloatingMode(gravity);
            if (Floating)
            {
                UpdateSelfFloating(args);
            }
            else
            {
                UpdateSelfNormal(args, gravity);
            }
        }

        private void SwitchFloatingMode(double gravity)
        {
            if (Floating)
            {
                if (gravity > 0)
                {
                    Floating = false;
                    State = MainMapObjectState.Falling;
                }
            }
            else
            {
                if (gravity <= 0)
                {
                    Floating = true;
                    State = MainMapObjectState.Floating;
                }
            }
        }

        /// <summary>
        /// 浮遊状態の状態更新
        /// </summary>
        private void UpdateSelfFloating(UpdateMapObjectArgs args)
        {
            // パラメータの保持
            var wind = args.GetWind(X, Y);

            RotateSelf();

            UpdatePreviousValue();

            // 動作本体
            switch (State)
            {
                case MainMapObjectState.Floating:
                    double accelX = Input.X * FloatingAccel;
                    double accelY = Input.Y * FloatingAccel;
                    UpdatePositionFloating(wind, accelX, accelY, Input.IsPressed(Keys.Jump) ? 2 : 1);
                    break;
                default:
                    throw new Exception("MainMapObjectのStateがおかしいぞ。");
            }
        }

        /// <summary>
        /// 浮遊状態の位置更新を行います。
        /// </summary>
        /// <param name="wind">風速</param>
        /// <param name="accelX">X軸方向の加速度</param>
        /// <param name="accelY">Y軸方向の加速度</param>
        /// <param name="accelCount">加速・減速を行う回数</param>
        private void UpdatePositionFloating(double wind, double accelX = 0.0, double accelY = 0.0, int accelCount = 1)
        {
            for (int i = 0; i < accelCount; i++)
            {
                double frictionX = (VelocityX - wind) * FloatingFriction;
                double frictionY = VelocityY * FloatingFriction;
                VelocityX += accelX - frictionX;
                VelocityY += accelY - frictionY;
            }
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (VelocityX * VelocityX + VelocityY * VelocityY > VelocityLimit * VelocityLimit)
            {
                var r = VelocityLimit / Math.Sqrt(VelocityX * VelocityX + VelocityY * VelocityY);
                VelocityX *= r;
                VelocityY *= r;
            }
            X += VelocityX;
            Y += VelocityY;
        }

        /// <summary>
        /// 変化前の値を保持します。
        /// </summary>
        private void UpdatePreviousValue()
        {
            PreviousX = X;
            PreviousY = Y;
            PreviousVelocityX = VelocityX;
            PreviousVelocityY = VelocityY;
        }

        /// <summary>
        /// 自身の移動に応じて回転させます。
        /// </summary>
        private void RotateSelf()
        {
            var adx = X - PreviousX;
            var ady = Y - PreviousY;
            var addx = VelocityX - PreviousVelocityX;
            var addy = VelocityY - PreviousVelocityY;
            AngularVelocity += (adx * addy - ady * addx) * AngularAccel - AngularVelocity * AngularFriction;
            Angle += AngularVelocity;
            while (Angle > 180) Angle -= 360;
            while (Angle < -180) Angle += 360;
        }

        /// <summary>
        /// 通常状態の状態更新
        /// </summary>
        private void UpdateSelfNormal(UpdateMapObjectArgs args, double gravity)
        {
            // パラメータの保持
            var wind = args.GetWind(X, Y);

            ResetRotation();

            UpdatePreviousValue();

            // 動作本体
            switch (State)
            {
                case MainMapObjectState.Standing:
                    break;
                case MainMapObjectState.Falling:
                    double accelX = Input.X * FloatingAccel;
                    double accelY = gravity * GravityAccel;
                    double frictionX = (VelocityX - wind) * FloatingFriction;
                    double frictionY = VelocityY * FallFriction;
                    VelocityX += accelX - frictionX;
                    VelocityY += accelY - frictionY;
                    UpdatePosition();
                    break;
                default:
                    throw new Exception("MainMapObjectのStateがおかしいぞ。");
            }
        }

        /// <summary>
        /// 回転を初期状態に戻します。
        /// </summary>
        private void ResetRotation()
        {
            AngularVelocity = 0;
            Angle = 0;
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

            // 結果
            var tempX = X;
            var tempY = Y;
            var tempVX = VelocityX;
            var tempVY = VelocityY;
            var landed = false;

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

                var sumX = 0.0;
                var sumY = 0.0;
                var sumVX = 0.0;
                var sumVY = 0.0;
                var count = 0.0;

                // 線との当たり判定
                foreach (var lineMapObject in lineMapObjects)
                {
                    var lineSegment = lineMapObject.ToSegment2D();
                    var lineVector = lineSegment.End - lineSegment.Start;
                    var lineNormal = new Vector2D(lineVector.Y, -lineVector.X);
                    lineNormal.Norm();
                    var lineNormalNegative = -lineNormal;

                    // 主人公の下側と線の上側
                    if (lineMapObject.HitUpper)
                    {
                        if (lineNormal.Y != 0)
                        {
                            var n = lineNormal.Y < 0 ? lineNormal : lineNormalNegative;
                            if (velocity.Dot(n) <= 0)
                            {
                                if (Collision2D.SegmentSegment(bottomSegment, lineSegment))
                                {
                                    var lineStartToCharaBottom = bottomSegment.End - lineSegment.Start;
                                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaBottom) / lineVector.LengthSq) - bottomVector;
                                    var newVelocity = velocity - n * velocity.Dot(n);
                                    sumX += newPoint.X;
                                    sumY += newPoint.Y;
                                    sumVX += newVelocity.X;
                                    sumVY += newVelocity.Y;
                                    count++;

                                    landed = true;
                                }
                            }
                        }
                    }

                    // 主人公の上側と線の下側
                    if (lineMapObject.HitBelow)
                    {
                        if (lineNormal.Y != 0)
                        {
                            var n = lineNormal.Y > 0 ? lineNormal : lineNormalNegative;
                            if (velocity.Dot(n) <= 0)
                            {
                                if (Collision2D.SegmentSegment(topSegment, lineSegment))
                                {
                                    var lineStartToCharaTop = topSegment.End - lineSegment.Start;
                                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaTop) / lineVector.LengthSq) - topVector;
                                    var newVelocity = velocity - n * velocity.Dot(n);
                                    sumX += newPoint.X;
                                    sumY += newPoint.Y;
                                    sumVX += newVelocity.X;
                                    sumVY += newVelocity.Y;
                                    count++;
                                }
                            }
                        }
                    }

                    // 主人公の右側と線の左側
                    if (lineMapObject.HitLeft)
                    {
                        if (lineNormal.X != 0)
                        {
                            var n = lineNormal.X < 0 ? lineNormal : lineNormalNegative;
                            if (velocity.Dot(n) <= 0)
                            {
                                if (Collision2D.SegmentSegment(rightSegment, lineSegment))
                                {
                                    var lineStartToCharaRight = rightSegment.End - lineSegment.Start;
                                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaRight) / lineVector.LengthSq) - rightVector;
                                    var newVelocity = velocity - n * velocity.Dot(n);
                                    sumX += newPoint.X;
                                    sumY += newPoint.Y;
                                    sumVX += newVelocity.X;
                                    sumVY += newVelocity.Y;
                                    count++;
                                }
                            }
                        }
                    }

                    // 主人公の左側と線の右側
                    if (lineMapObject.HitRight)
                    {
                        if (lineNormal.X != 0)
                        {
                            var n = lineNormal.X > 0 ? lineNormal : lineNormalNegative;
                            if (velocity.Dot(n) <= 0)
                            {
                                if (Collision2D.SegmentSegment(leftSegment, lineSegment))
                                {
                                    var lineStartToCharaLeft = leftSegment.End - lineSegment.Start;
                                    var newPoint = lineSegment.Start + lineVector * (lineVector.Dot(lineStartToCharaLeft) / lineVector.LengthSq) - leftVector;
                                    var newVelocity = velocity - n * velocity.Dot(n);
                                    sumX += newPoint.X;
                                    sumY += newPoint.Y;
                                    sumVX += newVelocity.X;
                                    sumVY += newVelocity.Y;
                                    count++;
                                }
                            }
                        }
                    }
                }

                if (count > 0)
                {
                    tempX = sumX / count;
                    tempY = sumY / count;
                    tempVX = sumVX / count;
                    tempVY = sumVY / count;
                }
            }

            return () =>
            {
                X = tempX;
                Y = tempY;
                VelocityX = tempVX;
                VelocityY = tempVY;

                if (!Floating)
                {
                    if (landed)
                    {
                        State = MainMapObjectState.Standing;
                    }
                }
            };
        }
    }
}
