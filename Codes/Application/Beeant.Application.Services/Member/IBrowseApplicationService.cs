using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beeant.Domain.Entities.Member;

namespace Beeant.Application.Services.Member
{
    /// <summary>
    /// 会员商品浏览记录接口。
    /// </summary>
    /// <remarks>
    /// <para>孙涛</para>
    /// <para>2015/8/14</para>
    /// </remarks>
    public interface IBrowseApplicationService
    {
        /// <summary>
        /// 添加商品浏览记录。
        /// </summary>
        /// <param name="info">浏览信息对象。</param>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/14</para>
        /// </remarks>
        void Push(BrowseEntity info);
    }
}
