using System.Linq;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Dependent;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Authority.Ability
{
    public partial class List : MaintenPageBase<AbilityEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<AbilityEntity>().Where(it => it.Menu.Id == Request.QueryString["MenuId"].Convert<long>());
            base.SetQueryWhere(query);
           
        }
        /// <summary>
        /// 得到菜单名称
        /// </summary>
        /// <returns></returns>
        public string GetMenuName()
        {
            if (!string.IsNullOrEmpty(Request["menuid"]))
            {
                var info = Ioc.Resolve<IApplicationService, MenuEntity>().GetEntity<MenuEntity>(Request["menuid"].Convert<long>());
                if (info != null) return string.Format("{0}-", info.Name);
            }
            return "";
        }

        protected override AbilityEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info.SaveType == SaveType.Add)
                info.Menu = new MenuEntity { Id = Request["menuid"].Convert<long>() };
            return info;
        }
    }
}