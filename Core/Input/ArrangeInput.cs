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

        public ArrangeInput(IInput original) => Original = original ?? throw new ArgumentNullException();

        public double X => HorizontalReverse ? -Original.X : Original.X;

        public double Y => VerticalReverse ? -Original.Y : Original.Y;

        public bool IsPressed(Keys key) => Original.IsPressed(GetActualKey(key));

        public bool IsPushed(Keys key) => Original.IsPushed(GetActualKey(key));

        public bool IsReleased(Keys key) => Original.IsReleased(GetActualKey(key));

        public void Update() { }

        private Keys GetActualKey(Keys key)
        {
            return key switch
            {
                Keys.Left => HorizontalReverse ? Keys.Right : Keys.Left,
                Keys.Up => VerticalReverse ? Keys.Down : Keys.Up,
                Keys.Right => HorizontalReverse ? Keys.Left : Keys.Right,
                Keys.Down => VerticalReverse ? Keys.Up : Keys.Down,
                _ => key,
            };
        }
    }
}
