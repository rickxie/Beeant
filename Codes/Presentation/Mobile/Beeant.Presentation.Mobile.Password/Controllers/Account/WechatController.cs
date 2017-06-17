using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Presentation.Mobile.Password.Models.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Password.Controllers.Account
{
    [AuthorizeFilter]
    public class WechatController : MobileBaseController
    {

        #region 首页

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(string message)
        {
            var model = new WechatModel
            {
                AccountNumbers = GetAccountNumbers(),
                Message=message
            };
            return View("~/Views/Account/Wchat/Index.cshtml", model);
        }
        /// <summary>
        /// 得到第三方平台
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AccountNumberEntity> GetAccountNumbers()
        {
            var query = new QueryInfo();
            query.Query<AccountNumberEntity>()
                .Where(it => it.Account.Id == Identity.Id && it.Name=="Wechat")
                .Select(it => new object[] {it.Id, it.Name, it.Number,it.Tag});
            return this.GetEntities<AccountNumberEntity>(query);
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Bind()
        {
            var url = string.Format("{0}/Wechat/Add", this.GetUrl("PresentationMobilePasswordUrl"));
            url = string.Format("{0}/Wechat/Oauth?url={1}", this.GetUrl("DistributedOutsideReceptionUrl"),
                Server.UrlEncode(url));
            url = this.Wechat().CreateAuthorityUrl(url, false);
            return Redirect(url);
        }
        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Add()
        {
            string message = "绑定失败"; 
            var weinxinUser = this.Wechat().GetAuthorityUser();
            if (weinxinUser != null && weinxinUser.ContainsKey("openid") && weinxinUser.ContainsKey("nickname"))
            {
                var openid = weinxinUser["openid"].ToString();
                var nickname= weinxinUser["nickname"].ToString();
                var accountNumber = GetAccountNumberByOpenId(openid);
                if (accountNumber != null && accountNumber.Account != null && accountNumber.Account.Id != Identity.Id)
                {
                    var entity = new AccountEntity
                    {
                        AccountNumbers = new List<AccountNumberEntity>(),
                        AccountIdentites = new List<AccountIdentityEntity>()
                    };
                    var accountIdentity = GetAccountIdentityByOpenId(openid);
                    if (accountIdentity != null)
                    {
                        accountIdentity.SaveType=SaveType.Remove;
                        entity.AccountIdentites.Add(accountIdentity);
                    }
                    entity.AccountNumbers.Add(accountNumber);
                    accountNumber.SaveType = SaveType.Remove;
                    var rev = this.SaveEntity(entity);
                    if (rev)
                    {
                        Add(openid, nickname, out message);
                    }
                }
                else if (accountNumber == null)
                {
                    Add(openid, nickname, out message);
                }
                else if (accountNumber.Account != null && accountNumber.Account.Id == Identity.Id)
                {
                    message = "该微信已经绑定";
                }
            }
            return RedirectToAction("Index",new RouteValueDictionary { { "message", message } });
        }

        private const string WechatName = "Wechat";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual AccountEntity Add(string openId, string name, out string message)
        {
            var entity = new AccountEntity();
            entity.AccountNumbers = new List<AccountNumberEntity>
            {
                new AccountNumberEntity
                {
                    Number = openId,
                    Account = new AccountEntity {Id = Identity.Id},
                    Tag = WechatName,
                    Name=name,
                    SaveType = SaveType.Add
                }
            };
            entity.AccountIdentites = new List<AccountIdentityEntity>
            {
                new AccountIdentityEntity
                {
                    Number = openId,
                    Account = new AccountEntity {Id = Identity.Id},
                    Name = WechatName,
                    SaveType = SaveType.Add
                }
            };
            var rev = this.SaveEntity(entity);
            message = rev ? "绑定成功" : entity.Errors?.FirstOrDefault()?.Message;
            return entity;
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        protected virtual AccountNumberEntity GetAccountNumberByOpenId(string openid)
        {
            var query = new QueryInfo();
            query.Query<AccountNumberEntity>()
                .Where(it => it.Account.Id == Identity.Id && it.Name == WechatName &&　it.Number==openid)
                .Select(it => new object[] { it.Id,it.Account.Id});
            var entities=this.GetEntities<AccountNumberEntity>(query);
            var entity = entities?.FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        protected virtual AccountIdentityEntity GetAccountIdentityByOpenId(string openid)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>()
                .Where(it => it.Account.Id == Identity.Id && it.Name == WechatName && it.Number == openid)
                .Select(it => new object[] { it.Id, it.Account.Id });
            var entities = this.GetEntities<AccountIdentityEntity>(query);
            var entity = entities?.FirstOrDefault();
            return entity;
        }

        #endregion


        #region 修改

        [DataFilter(EntityType = typeof(AccountNumberEntity))]
        [HttpPost]
        public virtual ActionResult Modify(WechatModel model)
        {
            if (model == null)
                return null;
            var entity = new AccountNumberEntity {Id = model.Id, Name = model.Name, SaveType = SaveType.Modify};
            entity.SetProperty(it => it.Name);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        #endregion

        #region 删除
       
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var entity = new AccountEntity
                {
                    AccountIdentites = new List<AccountIdentityEntity>(),
                    AccountNumbers = new List<AccountNumberEntity>()
                };
                var infos = GetAccountNumbers(id.Cast<long>().ToArray());
                if (infos != null)
                {
                    foreach (var info  in infos)
                    {
                        info.SaveType=SaveType.Remove;
                        entity.AccountNumbers.Add(info);
                        var accountIdentity = GetAccountIdentityByOpenId(info.Number);
                        if (accountIdentity != null)
                        {
                            accountIdentity.SaveType=SaveType.Remove;
                            entity.AccountIdentites.Add(accountIdentity);
                        }
                    }
                }
                rev = this.SaveEntity(entity);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AccountNumberEntity> GetAccountNumbers(long[] id)
        {
            var query = new QueryInfo();
            query.Query<AccountNumberEntity>()
                .Where(it => it.Account.Id == Identity.Id && id.Contains(it.Id))
                .Select(it => new object[] { it.Id, it.Account.Id,it.Number });
            var entities = this.GetEntities<AccountNumberEntity>(query);
            return entities;
        }
        #endregion
    }

  
}
