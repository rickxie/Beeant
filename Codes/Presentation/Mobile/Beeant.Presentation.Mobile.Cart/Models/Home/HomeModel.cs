using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Cart;

namespace Beeant.Presentation.Mobile.Cart.Models.Home
{

    /// <summary>
    /// 购物车
    /// </summary>
    public class HomeModel:PagerModel
    {
        /// <summary>
        /// 购物车产品
        /// </summary>
        public IList<ShopcartEntity> Shopcarts { get; set; }
       
    }
}
