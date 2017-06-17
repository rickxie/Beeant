using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cloud.Cms.Cms
{
    public partial class List : ListPageBase<CmsEntity>
    {


        public virtual string GetBindName(AccountEntity account)
        {
            if (account == null || account.AccountNumbers == null || account.AccountNumbers.FirstOrDefault(it => it.Name == "CmsId") == null)
                return "未绑定";
            return "已绑定";
        }

        /// <summary>
        /// 生成映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnBindAccountNumber_Click(object sender, System.EventArgs e)
        {
            var infos = GetCmss();
            if (infos == null)
                return;
            var entities = new List<AccountNumberEntity>();
            foreach (var info in infos)
            {
                if (info != null && info.Account != null)
                {
                    entities.Add(new AccountNumberEntity
                    {
                        Account = info.Account,
                        Tag = "CmsId",
                        Number = info.Id.ToString(),
                        Name = "CMS系统编号",
                        SaveType = SaveType.Add
                    });
                }
            }
            SaveEntities(entities, "绑定成功", "绑定失败");
        }


        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<CmsEntity> GetCmss()
        {
            var ids = new List<long>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                ids.Add(ckSelect.Value.Convert<long>());
            }
            var query = new QueryInfo();
            query.Query<CmsEntity>().Where(it => ids.ToArray().Contains(it.Id)).Select(it => new object[] { it.Id, it.Account.Id });
            return Ioc.Resolve<IApplicationService, CmsEntity>().GetEntities<CmsEntity>(query);
        }


    }
}