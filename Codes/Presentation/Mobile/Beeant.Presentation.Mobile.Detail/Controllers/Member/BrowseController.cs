using System.Web.Http.Filters;
using Dependent;
using Beeant.Application.Services.Member;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Presentation.Mobile.Detail.Controllers.Member
{
    public class BrowseController : MobileBaseController
    {

        #region 推送浏览记录
        /// <summary>
        /// 推送产品
        /// </summary>
        /// <param name="productId"></param>
        public virtual void Push(long productId)
        {
            if(Identity==null)
                return;
            Ioc.Resolve<IBrowseApplicationService>().Push(new BrowseEntity
            {
                Product = new ProductEntity { Id = productId },
                Account = new AccountEntity { Id = Identity.Id },
                SaveType = SaveType.Add
            });
        }
        #endregion
         

    }
}
