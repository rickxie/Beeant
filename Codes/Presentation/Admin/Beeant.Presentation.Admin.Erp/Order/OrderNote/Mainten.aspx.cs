using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Dependent;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Erp.Order.OrderNote
{
    public partial class Mainten :AuthorizePageBase
    {
        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
        }
   
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }

        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status == OrderStatusType.Cancel || order.Status == OrderStatusType.Finish)
                ((AuthorizePageBase)Page).InvalidateData("不能操作维护记录");
           
            LoadEntities();
        }
        /// <summary>
        /// 加载实体
        /// </summary>
        protected virtual void LoadEntities()
        {
            var infos = GetEntities();
            BindEntities(infos);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <returns></returns>
        protected virtual IList<OrderNoteEntity> GetEntities()
        {
            if (string.IsNullOrEmpty(Request.QueryString["OrderId"]))
                return null;
            var query = Pager1.Query;
            query.Query<OrderNoteEntity>().Where(it => it.Order.Id == Request.QueryString["OrderId"].Convert<long>()
                )
                 .OrderBy(it => it.InsertTime);
           return Ioc.Resolve<IApplicationService, OrderNoteEntity>().GetEntities<OrderNoteEntity>(query);
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void BindEntities(IList<OrderNoteEntity> infos)
        {
            if(infos==null)return;
            Repeater1.DataSource = infos;
            Repeater1.DataBind();
            Pager1.DataBind();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public virtual void Save()
        {
            var info = new OrderNoteEntity
                {
                    Content = txtContent.Value.Trim(),
                    Order = new OrderEntity { Id = Request.QueryString["OrderId"].Convert<long>() },
                    Account = new AccountEntity {Id = Identity.Id },
                    SaveType = SaveType.Add
                };
            var rev = Ioc.Resolve<IApplicationService, OrderNoteEntity>().Save(info);
            Message1.ShowMessage(info.Errors);
            if (rev)
            {
                LoadEntities();
                txtContent.Value = "";
            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="id"></param>
        public virtual void Remove(long id)
        {
            var info = new OrderNoteEntity {Id = id, SaveType = SaveType.Remove};
            Ioc.Resolve<IApplicationService, OrderNoteEntity>().Save(info);
            LoadEntities();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void Pager1_PagerChanged(object source, EventArgs e)
        {
            LoadEntities();
        }

        protected void Repeater1_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if(e.CommandName.Equals("Remove"))
                Remove(e.CommandArgument.Convert<long>());
        }

        protected void Repeater1_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            var lkbtnRemove = e.Item.FindControl("lkbtnRemove");
            if(lkbtnRemove==null)return;
            lkbtnRemove.Visible = DataBinder.Eval(e.Item.DataItem, "Account.Id").Convert<long>() == Identity.Id;
        }
    }
}