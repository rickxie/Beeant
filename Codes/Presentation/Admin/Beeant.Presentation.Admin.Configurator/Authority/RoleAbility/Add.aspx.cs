using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Authority;
using System;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Authority.RoleAbility
{
    public partial class Add : ListPageBase<AbilityEntity>
    {

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSubsystem.LoadData();
                LoadData();
            }
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<AbilityEntity>().Where(
                it => it.RoleAbilities.Count(s => s.Role.Id == Request["roleid"].Convert<long>()) == 0 && it.Menu.Subsystem.Url.StartsWith("PresentationAdmin"));
            base.SetQueryWhere(query);
        }

 
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var infos = GetSaveEntities<RoleAbilityEntity>(SaveType.Add);
            SaveEntities(infos, "授权成功", "授权失败");

        }

        /// <summary>
        /// 得到存储对象
        /// </summary>
        /// <param name="gridViewRow"></param>
        /// <param name="saveType"></param>
        /// <param name="isBindDataKey"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected override TEntityType GetSaveEntity<TEntityType>(GridViewRow gridViewRow, SaveType saveType,
                                                                bool isBindDataKey = true,
                                                                DropDownList dropDownList = null)

        {
            var info = base.GetSaveEntity<TEntityType>(gridViewRow, saveType, isBindDataKey, dropDownList);
            var roleAbility = info as RoleAbilityEntity;
            if (roleAbility == null) return info;
            var ckSelect = gridViewRow.FindControl(SelectName) as HtmlInputCheckBox;
            if (ckSelect != null && ckSelect.Checked)
                roleAbility.Ability = new AbilityEntity { Id = ckSelect.Value.Convert<long>() };
            roleAbility.Role = new RoleEntity { Id = Request.QueryString["roleid"].Convert<long>() };
            return roleAbility as TEntityType ;
        }

    }
}