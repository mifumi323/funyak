using System.Windows.Controls;

namespace MifuminSoft.funyak.View.AreaEnvironment
{
    public interface IAreaEnvironmentView
    {
        /// <summary>
        /// 表示先のCanvas
        /// </summary>
        Canvas Canvas { get; set; }

        /// <summary>
        /// 表示を更新します
        /// </summary>
        /// <param name="args">更新のパラメータ</param>
        void Update(AreaEnvironmentViewUpdateArgs args);
    }
}
