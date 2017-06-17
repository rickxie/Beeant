using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beeant.Domain.Entities.Search;

namespace Beeant.Application.Services.Search
{
    /// <summary>
    /// 搜索关键词接口。
    /// </summary>
    /// <remarks>
    /// <para>孙涛</para>
    /// <para>2015/8/17</para>
    /// </remarks>
    public interface IKeyQueueSaveApplicationService
    {
        /// <summary>
        /// 添加搜索关键词记录。
        /// </summary>
        /// <param name="KeyEntity">搜索关键词信息。</param>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        void Add(KeyEntity keyEntity);
    }
}
