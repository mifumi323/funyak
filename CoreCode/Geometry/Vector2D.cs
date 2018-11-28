using System;

namespace MifuminSoft.funyak.Geometry
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

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector2D operator *(Vector2D v, double d) => new Vector2D(v.X * d, v.Y * d);

        public static Vector2D operator /(Vector2D v, double d) => new Vector2D(v.X / d, v.Y / d);

        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);

        #endregion

        #region 自分との演算

        public double Dot(Vector2D v) => X * v.X + Y * v.Y;

        public double Cross(Vector2D v) => X * v.Y - Y * v.X;

        #endregion

        #region 情報取得

        public double Length => Math.Sqrt(LengthSq);

        public double LengthSq => X * X + Y * Y;

        public Vector2D GetNorm() => new Vector2D(X / Length, Y / Length);

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
