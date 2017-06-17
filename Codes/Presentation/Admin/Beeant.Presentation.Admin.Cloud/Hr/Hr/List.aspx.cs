using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Hr;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cloud.Hr.Hr
{
    public partial class List : ListPageBase<HrEntity>
    {

        public virtual string GetBindName(AccountEntity account)
        {
            if (account == null || account.AccountNumbers == null || account.AccountNumbers.FirstOrDefault(it => it.Name == "HrId") == null)
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
            var infos = GetHrs();
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
                        Tag = "HrId",
                        Number = info.Id.ToString(),
                        Name = "人事系统编号",
                        SaveType = SaveType.Add
                    });
                }
            }
            SaveEntities(entities, "绑定成功", "绑定失败");
        }


        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<HrEntity> GetHrs()
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
            query.Query<HrEntity>().Where(it => ids.ToArray().Contains(it.Id)).Select(it => new object[] { it.Id, it.Account.Id });
            return Ioc.Resolve<IApplicationService, HrEntity>().GetEntities<HrEntity>(query);
        }

    }
}