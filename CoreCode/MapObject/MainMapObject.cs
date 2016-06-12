using System;
using MifuminSoft.funyak.Core.Input;

namespace MifuminSoft.funyak.Core.MapObject
{
    public enum State
    {
        Standing,
        Floating,
    }

    public class MainMapObject : IDynamicMapObject
    {
        /// <summary>
        /// 浮遊モードであるか否か
        /// </summary>
        public bool Floating { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public State State { get; set; }

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

        public int Appearance { get; set; }

        public MainMapObject(double x, double y)
        {
            State = State.Floating;
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
