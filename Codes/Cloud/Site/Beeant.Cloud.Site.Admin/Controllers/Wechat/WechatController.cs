using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Cloud.Site.Admin.Models.Wechat;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Controllers.Wechat
{
    [SiteAuthorizeFilter]
    public class WechatController : SiteAuthorizeBaseController
    {
     
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var site = this.GetEntity<SiteEntity>(SiteId);
            if (site == null || string.IsNullOrEmpty(site.WechatAppId) || !site.IsOpenSubscribeUser)
                return Content("您尚未开通获取关注用户功能");
            var model = new WechatModel();
            return View("~/Views/Wechat/index.cshtml", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextOpenid"></param>
        /// <returns></returns>
        public virtual ActionResult Users(string nextOpenid)
        {
            var model = new WechatUserListModel();
            var siteWechat = this.Wechat(SiteId);
            if (siteWechat != null)
            {
                var accessToken = siteWechat.GetAccessToken();
                var users=siteWechat.GetUsers(nextOpenid, accessToken);
                if (users != null && users.ContainsKey("data"))
                {
                    model.Users = new List<WechatUserModel>();
                    model.TotalUserCount = users["total"].Convert<int>();
                    var count = users["count"].Convert<int>();
                    var data = (dynamic)users["data"];
                    if (count > model.PageSize)
                    {
                        model.NextOpenId = data.openid[model.PageSize-1].ToString();
                    }
                    count = count < model.PageSize ? count : model.PageSize;
                    for (int i = 0; i < count; i++)
                    {
                    
                        IDictionary<string,object> user = siteWechat.GetUser(data.openid[i].ToString(), accessToken);
                        if (user != null)
                        {
                            var userModel = new WechatUserModel
                            {
                                OpenId = user["openid"].Convert<string>(),
                                Nickname = user["nickname"].Convert<string>(),
                                City = user["city"].Convert<string>(),
                                Country = user["country"].Convert<string>(),
                                Province = user["province"].Convert<string>(),
                                Headimgurl = user["headimgurl"].Convert<string>(),
                                Remark  = user["remark"].Convert<string>()
                            };
                            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                            long lTime = long.Parse(string.Format("{0}0000000", user["subscribe_time"]));
                            TimeSpan toNow = new TimeSpan(lTime);
                            userModel.SubscribeTime = dtStart.Add(toNow);
                            switch (user["sex"].Convert<string>())
                            {
                                case "1":
                                    userModel.Sex = "男"; break;
                                case "2":
                                    userModel.Sex = "女"; break;
                                case "0":
                                    userModel.Sex = "未知"; break;
                            }
                            model.Users.Add(userModel);
                        }
                    }
                  
                }
            }
            return View("~/Views/Wechat/_User.cshtml", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public virtual ActionResult UpdateRemark(string openid,string remark)
        {
            var result=new Dictionary<string,object>();
            var siteWechat = this.Wechat(SiteId);
            if (siteWechat != null)
            {
                var rev = siteWechat.UpdateRemark(openid, remark);
                result.Add("Status",rev!=null);
            }
            return this.Jsonp(result);
        }
    }
}
