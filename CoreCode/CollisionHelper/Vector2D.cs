using System;

namespace MifuminSoft.funyak.CollisionHelper
{
    /// <summary>
    /// 2次元のベクトルを表します。
    /// </summary>
    public struct Vector2D
    {
        public double X;
        public double Y;

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region 算術演算子に対応

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator *(Vector2D v, double d)
        {
            return new Vector2D(v.X * d, v.Y * d);
        }

        public static Vector2D operator /(Vector2D v, double d)
        {
            return new Vector2D(v.X / d, v.Y / d);
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }

        #endregion

        #region 自分との演算

        public double Dot(Vector2D v)
        {
            return X * v.X + Y * v.Y;
        }

        public double Cross(Vector2D v)
        {
            return X * v.Y - Y * v.X;
        }

        #endregion

        #region 情報取得

        public double Length
        {
            get
            {
                return Math.Sqrt(LengthSq);
            }
        }

        public double LengthSq
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public Vector2D GetNorm()
        {
            return new Vector2D(X / Length, Y / Length);
        }

        #endregion

        #region 変更

        public void Norm()
        {
            double length = Length;
            X /= length;
            Y /= length;
        }

        #endregion
    }
}
