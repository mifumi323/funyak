using System;

namespace MifuminSoft.funyak.Core.Input
{
    /// <summary>
    /// ゲーム内の入力の共通化可能な機能を提供します。
    /// UpdateImplを実装してください。
    /// </summary>
    public abstract class InputBase : IInput
    {
        private double x, y;
        private bool[] keys = new bool[Enum.GetValues(typeof(Keys)).Length];
        private bool[] prevKeys = new bool[Enum.GetValues(typeof(Keys)).Length];

        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        public bool IsPressed(Keys key)
        {
            return keys[(int)key];
        }

        public bool IsPushed(Keys key)
        {
            return keys[(int)key] && !prevKeys[(int)key];
        }

        public bool IsReleased(Keys key)
        {
            return !keys[(int)key] && prevKeys[(int)key];
        }

        public void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                prevKeys[i] = keys[i];
                keys[i] = false;
            }
            x = y = 0;
            UpdateImpl();
        }

        /// <summary>
        /// 実際の入力の更新処理を実装します。
        /// SetDirectionメソッド及びSetKeyメソッドを呼び出して入力を更新してください。
        /// </summary>
        protected abstract void UpdateImpl();

        /// <summary>
        /// 方向の入力状態を設定します。
        /// </summary>
        /// <param name="x">方向入力のX成分</param>
        /// <param name="y">方向入力のY成分</param>
        protected void SetDirection(double x, double y)
        {
            if (x * x + y * y > 1)
            {
                var length = Math.Sqrt(x * x + y * y);
                x /= length;
                y /= length;
            }
            keys[(int)Keys.Left] = (x <= -0.5);
            keys[(int)Keys.Up] = (y <= -0.5);
            keys[(int)Keys.Right] = (x >= 0.5);
            keys[(int)Keys.Down] = (y >= 0.5);
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 方向入力以外の押しボタン式のキーの入力状態を設定します。
        /// </summary>
        /// <param name="key">方向以外のキーの種類</param>
        /// <param name="pressed">現在のフレームにおいて押されているか否か</param>
        protected void SetKey(Keys key, bool pressed)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    throw new ArgumentOutOfRangeException(nameof(key), key, "上下左右の入力はSetDirectionメソッドを使用してください。");
                default:
                    keys[(int)key] = pressed;
                    break;
            }
        }
    }
}
