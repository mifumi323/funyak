using System;

namespace MifuminSoft.funyak.Collision
{
    /// <summary>
    /// 当たり判定の結果の位置補正に使います
    /// </summary>
    public interface IPositionAdjuster
    {
        void Add(double x, double y, double vx, double vy, double nx, double ny, double f);
        void Add(IPositionAdjuster positionAdjuster);
        bool HasValue { get; }
        double X { get; }
        double Y { get; }
        double VelocityX { get; }
        double VelocityY { get; }
        double NormalX { get; }
        double NormalY { get; }
        double Friction { get; }
    }

    /// <summary>
    /// 位置補正の基本実装
    /// </summary>
    public abstract class PositionAdjusterBase : IPositionAdjuster
    {
        protected double x = 0.0;
        protected double y = 0.0;
        protected double vx = 0.0;
        protected double vy = 0.0;
        protected double nx = 0.0;
        protected double ny = 0.0;
        protected double f = 0.0;
        protected int count = 0;

        public abstract void Add(double x, double y, double vx, double vy, double nx, double ny, double f);

        public void Add(IPositionAdjuster positionAdjuster)
        {
            if (positionAdjuster.HasValue)
            {
                Add(positionAdjuster.X, positionAdjuster.Y, positionAdjuster.VelocityX, positionAdjuster.VelocityY, positionAdjuster.NormalX, positionAdjuster.NormalY, positionAdjuster.Friction);
            }
        }

        public bool HasValue => count > 0;
        public double X => x / count;
        public double Y => y / count;
        public double VelocityX => vx / count;
        public double VelocityY => vy / count;
        public double NormalX => nx / Math.Sqrt(nx * nx + ny * ny);
        public double NormalY => ny / Math.Sqrt(nx * nx + ny * ny);
        public double Friction => f / count;
    }

    /// <summary>
    /// 平均の位置に補正する
    /// </summary>
    public class PositionAdjusterAverage : PositionAdjusterBase
    {
        public override void Add(double x, double y, double vx, double vy, double nx, double ny, double f)
        {
            this.x += x;
            this.y += y;
            this.vx += vx;
            this.vy += vy;
            this.nx += nx;
            this.ny += ny;
            this.f += f;
            count++;
        }
    }

    /// <summary>最も高い位置(小さいY座標)に補正する</summary>
    public class PositionAdjusterHigh : PositionAdjusterBase
    {
        public PositionAdjusterHigh() => y = double.PositiveInfinity;

        public override void Add(double x, double y, double vx, double vy, double nx, double ny, double f)
        {
            if (y < this.y)
            {
                this.x = x;
                this.y = y;
                this.vx = vx;
                this.vy = vy;
                this.nx = nx;
                this.ny = ny;
                this.f = f;
                count = 1;
            }
            else if (y == this.y)
            {
                this.x += x;
                this.y += y;
                this.vx += vx;
                this.vy += vy;
                this.nx += nx;
                this.ny += ny;
                this.f += f;
                count++;
            }
        }
    }

    /// <summary>最も低い位置(大きいY座標)に補正する</summary>
    public class PositionAdjusterLow : PositionAdjusterBase
    {
        public PositionAdjusterLow() => y = double.NegativeInfinity;

        public override void Add(double x, double y, double vx, double vy, double nx, double ny, double f)
        {
            if (y > this.y)
            {
                this.x = x;
                this.y = y;
                this.vx = vx;
                this.vy = vy;
                this.nx = nx;
                this.ny = ny;
                this.f = f;
                count = 1;
            }
            else if (y == this.y)
            {
                this.x += x;
                this.y += y;
                this.vx += vx;
                this.vy += vy;
                this.nx += nx;
                this.ny += ny;
                this.f += f;
                count++;
            }
        }
    }

    /// <summary>最も左の位置(小さいX座標)に補正する</summary>
    public class PositionAdjusterLeft : PositionAdjusterBase
    {
        public PositionAdjusterLeft() => x = double.PositiveInfinity;

        public override void Add(double x, double y, double vx, double vy, double nx, double ny, double f)
        {
            if (x < this.x)
            {
                this.x = x;
                this.y = y;
                this.vx = vx;
                this.vy = vy;
                this.nx = nx;
                this.ny = ny;
                this.f = f;
                count = 1;
            }
            else if (x == this.x)
            {
                this.x += x;
                this.y += y;
                this.vx += vx;
                this.vy += vy;
                this.nx += nx;
                this.ny += ny;
                this.f += f;
                count++;
            }
        }
    }

    /// <summary>最も右の位置(大きいX座標)に補正する</summary>
    public class PositionAdjusterRight : PositionAdjusterBase
    {
        public PositionAdjusterRight() => x = double.NegativeInfinity;

        public override void Add(double x, double y, double vx, double vy, double nx, double ny, double f)
        {
            if (x > this.x)
            {
                this.x = x;
                this.y = y;
                this.vx = vx;
                this.vy = vy;
                this.nx = nx;
                this.ny = ny;
                this.f = f;
                count = 1;
            }
            else if (x == this.x)
            {
                this.x += x;
                this.y += y;
                this.vx += vx;
                this.vy += vy;
                this.nx += nx;
                this.ny += ny;
                this.f += f;
                count++;
            }
        }
    }
}
