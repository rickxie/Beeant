using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winner.Wcf
{
    public interface IWcfHost
    {
        /// <summary>
        /// 开启
        /// </summary>
        /// <returns></returns>
        bool Start(Type type);

        /// <summary>
        /// 停止监听
        /// </summary>
        /// <returns></returns>
        bool Stop(Type type);
    }
}
