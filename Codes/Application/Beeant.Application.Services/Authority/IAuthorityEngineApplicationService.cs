using System.Collections.Generic;
using Beeant.Domain.Entities.Authority;

namespace Beeant.Application.Services.Authority
{
    public interface IAuthorityEngineApplicationService
    {
        /// <summary>
        /// 根据角色加载菜单
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="subSystemUrl"></param>
        /// <returns></returns>
        IList<MenuEntity> GetMenus(long accountId,string subSystemUrl="");

        /// <summary>
        /// 得到验证资源
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        VerificationEntity GeVerificationEntity(long accountId, string resourceUrl);


        /// <summary>
        /// 得到工作流
        /// </summary>
        /// <returns></returns>
        AuthorityEnginEntity GetEngin();
    }
}
