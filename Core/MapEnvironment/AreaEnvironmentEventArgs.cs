using System;

namespace MifuminSoft.funyak.MapEnvironment
{
    public class AreaEnvironmentEventArgs : EventArgs
    {
        /// <summary>
        /// 追加された環境
        /// </summary>
        public AreaEnvironment AreaEnvironment { get; private set; }

        public AreaEnvironmentEventArgs(AreaEnvironment areaEnvironment) => AreaEnvironment = areaEnvironment;
    }
}
