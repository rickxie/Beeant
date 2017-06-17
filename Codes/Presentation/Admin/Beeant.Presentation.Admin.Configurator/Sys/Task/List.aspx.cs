using System;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Sys;
using Dependent;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Configurator.Sys.Task
{
    public partial class List : MaintenPageBase<TaskEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ckMonths.LoadData();
                ckWeeks.LoadData();
            }
            base.Page_Load(sender, e);
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                Execute();
                this.ShowMessage("执行成功", "执行成功");
            }
            catch (Exception ex)
            {
                this.ShowMessage("执行错误", string.Format("错误原因：{0}", ex.Message));
            }
            this.ShowMessage("执行成功", "执行成功");
        }

        protected virtual void Execute()
        {
            var infos = GetSaveEntities<TaskEntity>(SaveType.Remove);
            if (CheckSaveEntities(infos))
            {
                foreach (var info in infos)
                {
                    Ioc.Resolve<IJobApplicationService>(info.ClassName).Execute(info.ArgsArray);
                }
            }
          
        }

    }
}