using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class Detail : WorkflowPageBase<OrderEntity>
    {
      


        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();

               
            }
        }
  

        /// <summary>
        ///  是否显示订单明细checkbox
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isreturn"></param>
        /// <returns></returns>
        protected bool GetIsReturn(long id ,bool isreturn)
        {
            return id>0 && isreturn;
        }

        protected override OrderEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);

            }
            return info;
        }
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("Return();");
        }
    }
}