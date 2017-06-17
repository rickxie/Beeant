using System.Collections.Generic;
using Beeant.Domain.Entities.Account;

namespace Beeant.Presentation.Mobile.Password.Models.Account
{
    public class WechatModel
    {
        /// <summary>
        /// 微信平台
        /// </summary>
        public IList<AccountNumberEntity> AccountNumbers { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

    }
}
