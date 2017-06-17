using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    public partial class List : ListPageBase<GoodsEntity>
    {

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTag.LoadData();
            }
            base.Page_Load(sender, e);
        }


        protected override void AddClientScript()
        {
            base.AddClientScript();
            this.ExecuteScript("Init();");
        }

   
        /// <summary>
        /// 生成映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnUnSales_Click(object sender, System.EventArgs e)
        {
            var infos = GetGoodses();
            if (infos == null) return;
            foreach (var goodEntity in infos)
            {
                if(goodEntity!=null)
                {
                    goodEntity.IsSales = false;
                    goodEntity.SaveType=SaveType.Modify;
                    goodEntity.SetProperty(it => it.IsSales);
                } 
            }
            SaveEntities(infos, "下架成功", "下架失败");
        }
        /// <summary>
        /// 生成映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnSales_Click(object sender, System.EventArgs e)
        {
            var infos = GetGoodses();
            if (infos == null) return;
            foreach (var goodEntity in infos)
            {
                if (goodEntity != null)
                {
                    goodEntity.IsSales = true;
                    goodEntity.SaveType = SaveType.Modify;
                    goodEntity.SetProperty(it => it.IsSales);
                }
            }
            SaveEntities(infos, "上架成功", "上架失败");
        }
        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<GoodsEntity> GetGoodses()
        {
            var infos = new List<GoodsEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect==null || !ckSelect.Checked)
                    continue;
                var info = new GoodsEntity
                {
                    Id = ckSelect.Value.Convert<long>()
                };
                infos.Add(info);
            }
            return infos;
        }
        /// <summary>
        /// 得到B2B地址
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        public virtual string GetDetailUrl(long goodId)
        {
            return string.Format("{0}/Goods/Detail?goodsid={1}&id=0&mark={2}", this.GetUrl("PresentationWebsiteItemUrl"), goodId,
                                 Winner.Creator.Get<Winner.Base.ISecurity>()
                                       .EncryptMd5(DateTime.Now.ToString("yyyy-MM-dd")));
        }
    }
}