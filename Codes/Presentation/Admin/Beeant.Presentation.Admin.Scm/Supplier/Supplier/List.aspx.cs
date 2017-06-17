using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Supplier.Supplier
{
    public partial class List : ListPageBase<SupplierEntity>
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatus.LoadData();
            }
              
            base.OnInit(e);
        }
        protected override void LoadData()
        {

            base.LoadData();
            HideControls();

        }
        /// <summary>
        /// 批量报审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveEntities(GetSupplierEntities(SupplierStatusType.Auditting, true), "送审成功", "送审失败");
        }

        /// <summary>
        /// 批量设置可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEffective_Click(object sender, EventArgs e)
        {
            SaveEntities(GetSupplierEntities(SupplierStatusType.Effective, false), "设置有效成功", "设置有效失败");
        }



        /// <summary>
        /// 批量设置不可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInvalid_Click(object sender, EventArgs e)
        {
            SaveEntities(GetSupplierEntities(SupplierStatusType.Invalid, false), "设置为无效成功", "设置为无效失败");
        }

        /// <summary>
        /// 获取页面选定供应商信息，并进行状态变更
        /// </summary>
        /// <returns></returns>
        protected virtual IList<SupplierEntity> GetSupplierEntities(SupplierStatusType flag, bool checkCreated)
        {
            var infos = new List<SupplierEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked || GridView.DataKeys[gvr.RowIndex]==null)
                    continue;
                var info = new SupplierEntity
                {
                    Id = GridView.DataKeys[gvr.RowIndex]["Id"].Convert<long>(),
                    Status = flag,
                    SaveType = SaveType.Modify
                };
                info.SetProperty(it => it.Status);
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 根据登录的供应商Id和供应商状态判断是否显示编辑、资质、合同、证书
        /// </summary>
        /// <param name="statusFlag">状态</param>
        /// <returns></returns>
        protected bool GetDisplayUserAndStatus(SupplierStatusType statusFlag)
        {
            return statusFlag == SupplierStatusType.Invalid;
        }
     

        /// <summary>
        /// 批量修改所属用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModifyUser_Click(object sender, EventArgs e)
        {
            var infos = GetSaveEntities<SupplierEntity>(SaveType.Modify);
            SaveEntities(infos, "保存成功", "保存失败");
        }

 
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQuery(QueryInfo query)
        {
            switch (Request.QueryString["type"])
            {
                case "1":
                    query.Query<SupplierEntity>().Where(it => it.Service.Id == Identity.Id);
                    break;
                case "2":
                    query.Query<SupplierEntity>();
                    break;
            }

            base.SetQuery(query);
        }
        /// <summary>
        /// 隐藏控件
        /// </summary>
        protected virtual void HideControls()
        {
            if (Request.QueryString["type"]=="1")
            {
                btnEffective.Visible = false;
                btnInvalid.Visible = false;
            }
            else if(Request.QueryString["type"]=="2")
            {
                btnEffective.Visible = false;
                btnInvalid.Visible = false;
                GridView.Columns[2].Visible = false;
                GridView.Columns[3].Visible = false;
                GridView.Columns[4].Visible = false;
                GridView.Columns[5].Visible = false;
                btnSubmit.Visible = false;

            }
            else 
            {
                GridView.Columns[2].Visible = false;
                GridView.Columns[3].Visible = false;
                GridView.Columns[4].Visible = false;
                GridView.Columns[5].Visible = false;
                btnSubmit.Visible = false;
            }
        }

    }
}