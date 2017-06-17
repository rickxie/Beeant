using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Erp.Product.Product
{
    public partial class List : ListPageBase<ProductEntity>
    {
       
       
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            
            if (!string.IsNullOrEmpty(Request.QueryString["CategoryId"]))
            {
                if (string.IsNullOrEmpty(query.WhereExp))
                {
                    query.Query<ProductEntity>()
                     .Where(it => it.Goods.Category.Parent.Parent.Id == Request.QueryString["CategoryId"].Convert<long>());
                }
                else
                {
                    query.Where(string.Format("{0} && Goods.Category.Id==@CategoryId", query.WhereExp))
                         .SetParameter("CategoryId", Request.QueryString["CategoryId"].Convert<long>());
                }
            }
            base.SetQueryWhere(query);
        }


        #region 上架
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSales_Click(object sender, EventArgs e)
        {
            SaveEntities(GetSaleProducts(), "上架成功", "上架失败");
        }
        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<ProductEntity> GetSaleProducts()
        {
            var infos = new List<ProductEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var keys = GridView.DataKeys[gvr.RowIndex];
                if (keys == null) continue;
                var info = new ProductEntity
                {
                    Id = keys["Id"].Convert<long>(),
                    IsSales = true,
                    SaveType = SaveType.Modify
                };
                info.SetProperty(it => it.IsSales);
                infos.Add(info);
            }
            return infos;
        }
        #endregion


        #region 下架
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnSales_Click(object sender, EventArgs e)
        {
            SaveEntities(GetUnSaleProducts(), "下架成功", "下架失败");
        }
        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<ProductEntity> GetUnSaleProducts()
        {
            var infos = new List<ProductEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect==null || !ckSelect.Checked)
                    continue;
                var keys = GridView.DataKeys[gvr.RowIndex];
                if (keys == null ) continue;
                var info = new ProductEntity
                {
                    Id = keys["Id"].Convert<long>(),
                    IsSales = false,
                    SaveType = SaveType.Modify
                };
                info.SetProperty(it => it.IsSales);
                infos.Add(info);
            }
            return infos;
        }
        #endregion

 
        /// <summary>
        /// 得到库存数量
        /// </summary>
        /// <param name="inventories"></param>
        /// <returns></returns>
        public virtual bool IsWarning(InventoryEntity[] inventories)
        {
            if (inventories == null)
                return false;
            return
                inventories.Count(it => it.WarningCount >= it.TransitCount + it.Count + it.LockCount) > 0;

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                    var ckSelect = e.Row.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                    if (ckSelect != null && ckSelect.Attributes["IsWarning"].Convert<bool>())
                {
                    e.Row.Attributes.Add("style", "color:#FF0000");
                }
            }

        }
        /// <summary>
        /// 得到B2B地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetProductDetailUrl(long id)
        {
            return string.Format("{0}/Goods/Detail?id={1}&mark={2}", Page.GetUrl("PresentationWebsiteItemUrl"), id,
                                 Winner.Creator.Get<Winner.Base.ISecurity>()
                                       .EncryptMd5(DateTime.Now.ToString("yyyy-MM-dd")));
        }
     
    }
}