using System.Collections.Generic;
using System.Web.Mvc;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Mobile.Models.Inquery;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Mobile.Controllers.Inquery
{
    public class InqueryController : MobileSiteBaseController
    {
        private const string CodeName = "InquerySubmitCount";
        public virtual bool IsShowCode
        {
            get
            {
                var count = CodeHelper.GetErrorCount(CodeName);
                if (count == null || count >= 5)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new InqueryModel {IsShowCode = IsShowCode};
            CodeHelper.InitilzeCodeErrorCount(CodeName);
            return View(GetViewPath("~/Views/Inquery/index.cshtml"), model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(InqueryModel model)
        {
            var result=new Dictionary<string,object>();
            CodeHelper.AddCodeErrorCount(CodeName);
            if (IsShowCode && !CodeHelper.ValidateCode(model.Code, "InqueryCode"))
            {
                result.Add("Status", false);
                result.Add("Message", "codererror");
            }
            else
            {
                var entity = new InqueryEntity
                {
                    Mobile = model.Mobile,
                    Linkman = model.Linkman,
                    Content = model.Content,
                    Site=new SiteEntity { Id = SiteId},
                    SaveType = SaveType.Add
                };
                var rev = this.SaveEntity(entity);
                result.Add("Status", rev);
            }
            return this.Jsonp(result);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public virtual void Code()
        {
            CodeHelper.CreateCode("InqueryCode");
        }
    }
}
