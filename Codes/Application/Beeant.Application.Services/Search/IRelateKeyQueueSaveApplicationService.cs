using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beeant.Domain.Entities.Search;

namespace Beeant.Application.Services.Search
{
    /// <summary>
    /// 搜索相关词接口。
    /// </summary>
    /// <remarks>
    /// <para>孙涛</para>
    /// <para>2015/8/17</para>
    /// </remarks>
    public interface IRelateKeyQueueSaveApplicationService
    {
        /// <summary>
        /// 添加搜索相关词记录。
        /// </summary>
        /// <param name="RelateKeyEntity">搜索相关词信息。</param>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        void Add(RelateKeyEntity relateKeyEntity);
    }
}
