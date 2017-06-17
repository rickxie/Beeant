using System;
using System.Collections.Generic;
using System.Linq;
using Dependent;
using Beeant.Application.Services.Sys;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Sys;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Sys.Event
{
    public partial class List : MaintenPageBase<EventEntity>
    {
        protected void btnExcute_Click(object sender, System.EventArgs e)
        {
            Ioc.Resolve<IEventEngineApplicationService>().Trigger(ddlEventName.SelectedValue, GetArgs());
            this.ShowMessage("触发成功", "触发成功");
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEvents();
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 得到参数
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string,string> GetArgs()
        {
            if (string.IsNullOrWhiteSpace(txtEventArgs.Value))
                return null;
           var result=new Dictionary<string,string>();
            var values = txtEventArgs.Value.Split('&');
            foreach (var value in values)
            {
                var ps = value.Split('=');
                if(ps.Length!=2)
                    continue;
                result.Add(ps[0], ps[1]);
            }
            return result;
        }
        /// <summary>
        /// 加载事件
        /// </summary>

        protected virtual void LoadEvents()
        {
            var query=new QueryBasicInfo<EventEntity>();
            var infos = query.Query().Select(it => it.Name).ToList<EventEntity>();
            infos = infos?.Distinct().ToList();
            ddlEventName.DataBind(infos);
        }
    }
}