using System;

namespace MifuminSoft.funyak.Input
{
    /// <summary>
    /// 元の入力を加工した入力
    /// </summary>
    public class ArrangeInput : IInput
    {
        /// <summary>
        /// 元の入力
        /// </summary>
        public IInput Original { get; private set; }

        /// <summary>
        /// 横方向に反転
        /// </summary>
        public bool HorizontalReverse { get; set; } = false;

        /// <summary>
        /// 縦方向に反転
        /// </summary>
        public bool VerticalReverse { get; set; } = false;

        public ArrangeInput(IInput original)
        {
            if (original == null) throw new ArgumentNullException();
            Original = original;
        }

        public double X
        {
            get
            {
                return HorizontalReverse ? -Original.X : Original.X;
            }
        }

        public double Y
        {
            get
            {
                return VerticalReverse ? -Original.Y : Original.Y;
            }
        }

        public bool IsPressed(Keys key)
        {
            return Original.IsPressed(GetActualKey(key));
        }

        public bool IsPushed(Keys key)
        {
            return Original.IsPushed(GetActualKey(key));
        }

        public bool IsReleased(Keys key)
        {
            return Original.IsReleased(GetActualKey(key));
        }

        public void Update()
        {
        }

        private Keys GetActualKey(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    return HorizontalReverse ? Keys.Right : Keys.Left;
                case Keys.Up:
                    return VerticalReverse ? Keys.Down : Keys.Up;
                case Keys.Right:
                    return HorizontalReverse ? Keys.Left : Keys.Right;
                case Keys.Down:
                    return VerticalReverse ? Keys.Up : Keys.Down;
                default:
                    return key;
            }
        }
    }
}
