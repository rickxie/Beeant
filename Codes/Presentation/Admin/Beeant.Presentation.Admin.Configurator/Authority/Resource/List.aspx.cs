using System.Linq;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Dependent;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.Resource
{
    public partial class List : MaintenPageBase<ResourceEntity>
    {
        public long MenuId
        {
            get { return Request.QueryString["MenuId"].Convert<long>(); }
        }

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                var query = new QueryInfo();
                query.Query<AbilityEntity>().Where(it => it.Menu.Id == MenuId);
                ddlAbility.Query = query;
                ddlAbilitySearch.Query = query;
                ddlAbility.LoadData();
                ddlAbilitySearch.LoadData();
            }
            base.Page_Load(sender, e);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<ResourceEntity>().Where(it => it.Ability.Menu.Id == MenuId);
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
                var info = Ioc.Resolve<IApplicationService, MenuEntity>().GetEntity<MenuEntity>(MenuId);
                if (info != null) return string.Format("{0}-", info.Name);
            } 
            return "";
        }

      
    }
}