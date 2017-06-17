using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Api;
using System;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Api.VoucherProtocol
{
    public partial class Add : ListPageBase<ProtocolEntity>
    {
        public long VoucherId
        {
            get { return Request["VoucherId"].Convert<long>(); }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<ProtocolEntity>().Where(
                it => it.VoucherProtocols.Count(s => s.Voucher.Id == VoucherId) == 0);
            base.SetQueryWhere(query);
        }

 
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var infos = GetSaveEntities<VoucherProtocolEntity>(SaveType.Add);
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
            var voucherProtocol = info as VoucherProtocolEntity;
            if (voucherProtocol == null) return info;
            var ckIsForbid = gridViewRow.FindControl("ckIsForbid") as HtmlInputCheckBox;
            if (ckIsForbid != null)
                voucherProtocol.IsForbid = ckIsForbid.Checked;
            var ckIsLog = gridViewRow.FindControl("ckIsLog") as HtmlInputCheckBox;
            if (ckIsLog != null)
                voucherProtocol.IsLog = ckIsLog.Checked;
            var txtSecondCount = gridViewRow.FindControl("txtSecondCount") as HtmlInputText;
            if (txtSecondCount != null)
                voucherProtocol.SecondCount = txtSecondCount.Value.Convert<int>();
            var txtDayCount = gridViewRow.FindControl("txtDayCount") as HtmlInputText;
            if (txtDayCount != null)
                voucherProtocol.DayCount = txtDayCount.Value.Convert<int>();
            var txtArgs = gridViewRow.FindControl("txtArgs") as HtmlInputText;
            if (txtArgs != null)
                voucherProtocol.Args = txtArgs.Value;
            var ckSelect = gridViewRow.FindControl(SelectName) as HtmlInputCheckBox;
            if (ckSelect != null && ckSelect.Checked)
                voucherProtocol.Protocol = new ProtocolEntity { Id = ckSelect.Value.Convert<long>() };
            var ckIsSign = gridViewRow.FindControl("ckIsSign") as HtmlInputCheckBox;
            if (ckIsSign != null)
                voucherProtocol.IsSign = ckIsSign.Checked;
            voucherProtocol.Voucher = new VoucherEntity { Id = VoucherId };
            return voucherProtocol as TEntityType;
        }

    }
}