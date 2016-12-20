using System.Collections.Generic;

namespace MifuminSoft.funyak.Input
{
    public class ReplayInput : InputBase
    {
        public List<InputRecord> InputLog { get; private set; } = new List<InputRecord>();
        public int FrameLength { get; set; }

        private int frame = 0;
        private int index = 0;
        private double previousX = 0, previousY = 0;

        protected override void UpdateImpl()
        {
            double x = previousX, y = previousY;
            while (index < InputLog.Count && InputLog[index].Frame <= frame)
            {
                switch (InputLog[index].Key)
                {
                    case Keys.Left:
                        x = -InputLog[index].Value;
                        break;
                    case Keys.Up:
                        y = -InputLog[index].Value;
                        break;
                    case Keys.Right:
                        x = InputLog[index].Value;
                        break;
                    case Keys.Down:
                        y = InputLog[index].Value;
                        break;
                    default:
                        SetKey(InputLog[index].Key, InputLog[index].Value != 0);
                        break;
                }
                index++;
            }
            SetDirection(x, y);
            previousX = x;
            previousY = y;
            frame++;
            if (frame >= FrameLength)
            {
                frame = 0;
                index = 0;
            }
        }
    }
}
