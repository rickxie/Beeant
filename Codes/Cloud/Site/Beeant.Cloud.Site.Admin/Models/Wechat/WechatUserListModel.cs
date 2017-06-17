using System;
using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;

namespace Beeant.Cloud.Site.Admin.Models.Wechat
{
    public class WechatUserModel
    {
        /// <summary>
        /// openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Headimgurl { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public DateTime SubscribeTime { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Remark { get; set; }

    }
    public class WechatUserListModel:PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize { get; set; } = 24;
        public string NextOpenId { get; set; }
        /// <summary>
        /// 总体用户数
        /// </summary>
        public int TotalUserCount { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        public IList<WechatUserModel> Users { get; set; }
    }
}
