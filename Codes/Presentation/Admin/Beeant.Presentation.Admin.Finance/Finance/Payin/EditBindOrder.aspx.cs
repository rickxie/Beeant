using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Payin
{
    public partial class EditBindOrder : ListPageBase<PayinItemEntity>
    {
        public long PayinId
        {
            get { return Request.QueryString["id"].Convert<long>(); }
        }
        protected override void LoadData()
        {
            var entity = Ioc.Resolve<IApplicationService, PayinEntity>().GetEntity<PayinEntity>(PayinId);
            if (entity == null || entity.Status != PayinStatusType.WaitHandle)
                ((AuthorizePageBase)Page).InvalidateData("您没有权限在该状态下操作收款核销");
 
                base.LoadData();
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<PayinItemEntity>().Where(it => it.Payin.Id == Request.QueryString["id"].Convert<long>());
            base.SetQueryWhere(query);
        }

        protected void btnModifyAmount_Click(object sender, EventArgs e)
        {
            SaveEntities(GetChangeAmountEntities(), "修改金额成功", "修改金额失败");
        }

        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<OrderPayEntity> GetChangeAmountEntities()
        {
            var infos = new List<OrderPayEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect != null && !ckSelect.Checked)
                    continue;
                var txtAmount = gvr.FindControl("txtAmount") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtAmount == null || txtRemark==null)
                    continue;
                var info = new OrderPayEntity
                {
                    Id = ckSelect.Value.Convert<long>(),
                    Amount = txtAmount.Value.Convert<decimal>(),
                    Remark=txtRemark.Value,
                    SaveType = SaveType.Modify,
                };
                info.SetProperty(it => it.Amount);
                info.SetProperty(it => it.Remark);
                infos.Add(info);
            }
            return infos;
        }
    }
}