using System.Collections.Generic;
using Winner;
using Winner.Filter;

namespace Beeant.Application.Dtos.Order
{
    #region 下单DT0


    /// <summary>
    /// 下单
    /// </summary>
    public class OrderReturnDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddError(string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            var error = Creator.Get<IValidation>().GetErrorInfo(GetType().FullName, propertyName);
            error.Message = string.Format(error.Message, paramters);
            Errors.Add(error);
        }
     
    }
    #endregion
 
}
