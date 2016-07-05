namespace MifuminSoft.funyak.Input
{
    /// <summary>
    /// ゲーム内の入力を取り扱います。
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// 入力のX成分(-1.0～1.0)
        /// </summary>
        double X { get; }

        /// <summary>
        /// 入力のY成分(-1.0～1.0)
        /// </summary>
        double Y { get; }

        /// <summary>
        /// 指定したキーが現在のフレームにおいて押されているかどうかを返します。
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <returns>キーが押されていればtrue、押されていなければfalse</returns>
        bool IsPressed(Keys key);

        /// <summary>
        /// 指定したキーが現在のフレームで押された状態に変化したかどうかを返します。
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <returns>キーが押された状態に変化していればtrue、押された状態に変化していなければfalse</returns>
        bool IsPushed(Keys key);

        /// <summary>
        /// 指定したキーが現在のフレームで放された状態に変化したかどうかを返します。
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <returns>キーが放された状態に変化していればtrue、放された状態に変化していなければfalse</returns>
        bool IsReleased(Keys key);

        /// <summary>
        /// 入力の状態を最新の状態に更新します。
        /// </summary>
        void Update();
    }

    /// <summary>
    /// キーの種類
    /// </summary>
    public enum Keys : int
    {
        /// <summary>
        /// 左キー
        /// </summary>
        Left = 0,

        /// <summary>
        /// 上キー
        /// </summary>
        Up = 1,

        /// <summary>
        /// 右キー
        /// </summary>
        Right = 2,

        /// <summary>
        /// 下キー
        /// </summary>
        Down = 3,

        /// <summary>
        /// ジャンプキー
        /// </summary>
        Jump = 4,

        /// <summary>
        /// 攻撃キー
        /// </summary>
        Attack = 5,

        /// <summary>
        /// おまけ機能用のキー(ふにゃが笑うよ！)
        /// </summary>
        Smile = 6,
    }
}
