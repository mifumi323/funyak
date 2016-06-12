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
        /// <summary>
        /// 浮遊モードであるか否か
        /// </summary>
        public bool Floating { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public MainMapObjectState State { get; set; }

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

        /// <summary>
        /// 外見を示す値
        /// </summary>
        public int Appearance { get; set; }

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
        }

        public void UpdateSelf()
        {
            // TODO: 動こうぜ
        }

        public Action CheckCollision()
        {
            return null;
        }
    }
}
