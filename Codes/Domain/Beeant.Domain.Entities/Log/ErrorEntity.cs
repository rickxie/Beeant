using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class ErrorEntity : BaseEntity<ErrorEntity>
    {
    
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 错误页面
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string Device { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public AccountEntity Account { get; set; }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="ex"></param>
        public virtual void SetEntity(Exception ex)
        {
            if(ex==null)return;
            while (ex.InnerException!=null)
            {
                ex = ex.InnerException;
            }
            Message = ex.Message;
            Detail = ex.StackTrace;
        }
    }
}
