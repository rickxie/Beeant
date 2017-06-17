using System;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Cloud.Crm
{
    public class CrmDataFilterAttribute : CrmAuthorizeFilterAttribute
    {

        private string _idParamterName = "id";
        public Type EntityType { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string IdParamterName
        {
            get { return _idParamterName; }
            set { _idParamterName = value; }
        }

        private string _identityName = "Crm.Id";
        /// <summary>
        /// 值名称
        /// </summary>
        public string IdentityName
        {
            get { return _identityName; }
            set { _identityName = value; }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="filterContext"></param>
        public override void RedirectPage(ActionExecutingContext filterContext)
        {
            if (Identity == null)
                base.RedirectPage(filterContext);
            else
                filterContext.Result = new ContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>

        public override bool CheckFilter(ActionExecutingContext filterContext)
        {
            var crmId = filterContext.Controller.ViewBag.CrmId ?? 0;
            if (crmId == 0)
                return false;
            var reuslt = filterContext.Controller.ValueProvider.GetValue(IdParamterName);
            if (reuslt == null || reuslt.RawValue == null)
                return false;
            var rawValue = reuslt.RawValue;
            if (typeof(string[]) == rawValue.GetType())
            {
                var values = rawValue.Convert<string[]>();
                if (values == null)
                    return false;
                var ids = values.Select(value => value.Convert<long>()).ToArray();
                if (ids.Length == 0)
                    return false;
                var query = new QueryInfo();
                query.SetPageSize(1).From(EntityType.FullName)
                     .Where(string.Format("@IdArray.Contains(Id) && {0}==@CrmId", IdentityName))
                     .SetParameter("IdArray", ids)
                     .SetParameter("CrmId", crmId)
                     .Select("Id");
                Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
                if (query.DataCount == ids.Length)
                    return true;
            }
            else
            {
                var id = rawValue.Convert<long>();
                if (id == 0)
                    return false;
                var query = new QueryInfo { IsReturnCount = false };
                query.From(EntityType.FullName)
                     .Where(string.Format("Id==@Id && {0}==@CrmId", IdentityName))
                     .SetParameter("Id", id)
                     .SetParameter("CrmId", crmId)
                     .Select("Id");
                var infos = Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
                if (infos != null && infos.Count > 0)
                    return true;
            }
            return false;
        }
    }
}
