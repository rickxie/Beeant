using System;
using Beeant.Domain.Entities.Order;

namespace Beeant.Application.Services.Order
{
    public class OrderProductApplicationService : RealizeApplicationService<OrderProductEntity>
    {

        /// <summary>
        /// 重新异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool HandleException(Exception ex, OrderProductEntity info)
        {
            var rev = base.HandleException(ex, info);
            if (rev)
                return true;
            switch (ex.Message)
            {
                case "ProductCountNotEnough":
                    if (info.Product != null && info.Product.Errors != null &&
                                 info.Product.Errors.Count > 0)
                    {
                        info.AddError("ProductCountNotEnough", info.Name);
                    }
                    return true;
                default:
                    return false;
            }
        }

    }
}
