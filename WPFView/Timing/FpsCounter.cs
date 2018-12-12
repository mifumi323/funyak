using System;

namespace MifuminSoft.funyak.View.Timing
{
    public class FpsCounter
    {
        long count = 0;
        long prevTime = 0;

        /// <summary>経過フレーム数</summary>
        public long Frame { get; private set; } = 0;
        /// <summary>FPS</summary>
        public double Fps { get; private set; } = 0.0;

        public void Step()
        {
            Frame++;
            // FPS計測
            if (count == 0)
            {
                count = 60;
                long time = DateTime.Now.Ticks;
                Fps = 10000000.0 * count / (time - prevTime);
                prevTime = time;
            }
            count--;
        }
    }
}
