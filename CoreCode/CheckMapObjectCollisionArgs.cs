namespace MifuminSoft.funyak.Core
{
    /// <summary>
    /// マップオブジェクトの当たり判定の引数を格納します。
    /// </summary>
    public class CheckMapObjectCollisionArgs
    {
        private Map map;

        public CheckMapObjectCollisionArgs(Map map)
        {
            this.map = map;
        }
    }
}