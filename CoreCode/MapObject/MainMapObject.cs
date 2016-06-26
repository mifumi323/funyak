using System;
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
                return X - 14.0;
            }
        }

        public double Top
        {
            get
            {
                return Y - 14.0;
            }
        }

        public double Right
        {
            get
            {
                return X + 14.0;
            }
        }

        public double Bottom
        {
            get
            {
                return Y + 14.0;
            }
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

        public Action CheckCollision()
        {
            return null;
        }
    }
}
