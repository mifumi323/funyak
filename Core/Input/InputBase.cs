using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifuminSoft.funyak.Core.Input
{
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

        protected abstract void UpdateImpl();

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
        }

        protected void SetKey(Keys key, bool pressed)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    throw new ArgumentOutOfRangeException(nameof(key));
                default:
                    keys[(int)key] = pressed;
                    break;
            }
        }
    }
}
