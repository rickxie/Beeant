using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Basedata.Brand
{
    public partial class List : ListPageBase<BrandEntity>
    {
        /// <summary>
        /// 批量启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            SaveEntities(GetBrandEntity(true), "启用成功", "启用失败");
        }

        /// <summary>
        /// 批量设置禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            SaveEntities(GetBrandEntity(false), "禁用成功", "禁用失败");
        }

        protected virtual IList<BrandEntity> GetBrandEntity( bool isUsed)
        {
            var infos = new List<BrandEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (!ckSelect.Checked)
                    continue;
                var info = new BrandEntity
                {
                    Id = GridView.DataKeys[gvr.RowIndex]["Id"].Convert<long>(),
                    IsUsed = isUsed,
                    SaveType = SaveType.Modify
                };
                info.SetProperty(it => it.IsUsed);
                infos.Add(info);
            }
            return infos;
        }

    }
}