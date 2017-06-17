using System.Collections.Generic;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Product.Inquery
{
    public partial class List : ListPageBase<InqueryEntity>
    {

        /// <summary>
        /// 生成映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnUnShow_Click(object sender, System.EventArgs e)
        {
            var infos = GetInqueries();
            if (infos == null) return;
            foreach (var inquery in infos)
            {
                if (inquery != null)
                {
                    inquery.IsShow = false;
                    inquery.SaveType = SaveType.Modify;
                    inquery.SetProperty(it => it.IsShow);
                }
            }
            SaveEntities(infos, "操作成功", "操作失败");
        }
        /// <summary>
        /// 生成映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnShow_Click(object sender, System.EventArgs e)
        {
            var infos = GetInqueries();
            if (infos == null) return;
            foreach (var inquery in infos)
            {
                if (inquery != null)
                {
                    inquery.IsShow = true;
                    inquery.SaveType = SaveType.Modify;
                    inquery.SetProperty(it => it.IsShow);
                }
            }
            SaveEntities(infos, "操作成功", "操作失败");
        }
        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<InqueryEntity> GetInqueries()
        {
            var infos = new List<InqueryEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var info = new InqueryEntity
                {
                    Id = ckSelect.Value.Convert<long>()
                };
                infos.Add(info);
            }
            return infos;
        }
    }
}