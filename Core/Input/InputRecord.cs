namespace MifuminSoft.funyak.Input
{
    /// <summary>
    /// 入力の記録
    /// </summary>
    public struct InputRecord
    {
        /// <summary>
        /// いつ入力されたか
        /// </summary>
        public int Frame;
        /// <summary>
        /// 何が入力されたか
        /// </summary>
        public Keys Key;
        /// <summary>
        /// 入力の大きさ(押しボタンの場合0か1)
        /// </summary>
        public double Value;

        public InputRecord(int frame, Keys key, double value)
        {
            Frame = frame;
            Key = key;
            Value = value;
        }
    }
}
