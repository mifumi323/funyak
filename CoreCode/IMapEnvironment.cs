namespace MifuminSoft.funyak.Core
{
    /// <summary>
    /// マップの環境を表します。
    /// </summary>
    public interface IMapEnvironment
    {
        /// <summary>
        /// 重力を取得します。
        /// </summary>
        double Gravity { get; }

        /// <summary>
        /// 風速を取得します。
        /// </summary>
        double Wind { get; }
    }
}
