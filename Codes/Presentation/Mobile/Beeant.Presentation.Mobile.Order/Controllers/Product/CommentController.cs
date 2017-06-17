using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Presentation.Mobile.Order.Models.Product;
using Winner.Persistence;

namespace Beeant.Presentation.Mobile.Order.Controllers.Product
{
    [AuthorizeFilter]
    public class CommentController : MobileBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(long productId,long orderId)
        {
            return View("~/Views/Product/Comment/Index.cshtml");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(CommentModel model)
        {
            var entity = new CommentEntity
            {
                Product = new ProductEntity {Id = model.ProductId},
                Order = new OrderEntity {Id = model.OrderId},
                Account = new AccountEntity {Id = Identity.Id},
                Detail = model.Detail,
                Type = model.Type,
                SaveType=SaveType.Add
            };
            var rev = this.SaveEntity(entity);
            model.Result = rev;
            model.Message =rev?"评论成功": entity.Errors?.FirstOrDefault()?.Message;
            return View("~/Views/Product/Comment/Index.cshtml", model);
        }


    }
}
