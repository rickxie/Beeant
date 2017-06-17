using System;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.FilterAttribute
{
    public class DataFilterAttribute: ActionFilterAttribute
    {

        private string _idParamterName = "id";
        public Type EntityType { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public virtual string IdParamterName
        {
            get { return _idParamterName; }
            set { _idParamterName = value; }
        }

        private string _identityName = "Account.Id";
        /// <summary>
        /// 值名称
        /// </summary>
        public virtual string IdentityName
        {
            get { return _identityName; }
            set { _identityName = value; }
        }

        private IdentityEntity _identity;
        /// <summary>
        /// 登录服务
        /// </summary>
        public virtual IdentityEntity Identity
        {
            get
            {
                if (_identity == null)
                    _identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>();
                return _identity;
            }
            set { _identity = value; }
        }

        private long? _identityId;

        public virtual long IdentityId
        {
            get
            {
                if (_identityId.HasValue)
                    return _identityId.Value;
                _identityId = Identity == null ? 0 : Identity.Id;
                return _identityId.Value;
            }
            set { _identityId = value; }
        }

      

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Check(filterContext))
                HidePage(filterContext);
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        protected virtual bool Check(ActionExecutingContext filterContext)
        {
            if (IdentityId == 0)
                return false;
            var reuslt = filterContext.Controller.ValueProvider.GetValue(IdParamterName);
            if (reuslt == null || reuslt.RawValue == null)
                return false;
            var rawValue = reuslt.RawValue;
            if (typeof (string[]) == rawValue.GetType())
            {
                var values = rawValue.Convert<string[]>();
                if (values == null)
                    return false;
                var ids = values.Select(value => value.Convert<long>()).ToArray();
                if (ids.Length == 0)
                    return false;
                var query = new QueryInfo { IsReturnCount = false };
                query.SetPageSize(1).From(EntityType.FullName)
                     .Where(string.Format("@IdArray.Contains(Id) && {0}==@IdentityId", IdentityName))
                     .SetParameter("IdArray", ids)
                     .SetParameter("IdentityId", IdentityId)
                     .Select("Id");
                var infos= Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
                if (infos!=null && infos.Count == ids.Length)
                    return true;
            }
            else
            {
                var id = rawValue.Convert<long>();
                if (id == 0)
                    return false;
                var query = new QueryInfo {IsReturnCount=false};
                query.From(EntityType.FullName)
                     .Where(string.Format("Id==@Id && {0}==@AccountId", IdentityName))
                     .SetParameter("Id", id)
                     .SetParameter("IdentityId", IdentityId)
                     .Select("Id");
                var infos = Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
                if (infos != null && infos.Count >0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        protected virtual void HidePage(ActionExecutingContext filterContext)
        {
            filterContext.Result = new ContentResult();
        }
    }
}
