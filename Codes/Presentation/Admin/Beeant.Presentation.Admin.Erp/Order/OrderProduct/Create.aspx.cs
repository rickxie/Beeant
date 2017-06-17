using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Order.OrderProduct
{
    public partial class Create : AddPageBase<OrderProductEntity>
    {
        public override long RequestId
        {
            get { return Request["orderid"].Convert<long>(); }
            set { base.RequestId = value; }
        }
        public int Index = 0;
        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }
        protected override OrderProductEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Order = new OrderEntity { Id = Request.QueryString["OrderId"].Convert<long>() };
            }
            return info;
        }
        protected override void LoadEntity()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status != OrderStatusType.WaitHandle)
                ((AuthorizePageBase)Page).InvalidateData("不能新增订单明细");
            base.LoadEntity();
        }
        /// <summary>
        /// 重写保存
        /// </summary>
        protected override void Save()
        {
            var infos = GetOrderProducts();
            var rev = Ioc.Resolve<IApplicationService, OrderProductEntity>().Save(infos);
            var erros = new List<ErrorInfo>();
            if (!rev)
            {
                foreach (var info in infos)
                {
                    erros.AddList(info.Errors);
                }
            }
            SetResult(rev, erros);
        }

        /// <summary>
        /// 得到采购明细
        /// </summary>
        protected virtual IList<OrderProductEntity> GetOrderProducts()
        {
            var infos = new List<OrderProductEntity>();
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var txtCost = gvr.FindControl("txtCost") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                var hfName = gvr.FindControl("hfName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var hfIsReturn = gvr.FindControl("hfIsReturn") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var hfDescription = gvr.FindControl("hfDescription") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (txtCost == null || txtCount == null || hfName == null || hfIsReturn == null || hfDescription == null)
                    continue;
                var orderItem = new OrderProductEntity
                {
                    Product = new ProductEntity { Id = ckSelect.Value.Convert<long>() },
                    Price = txtCost.Value.Convert<decimal>(),
                    Count = txtCount.Value.Convert<int>(),
                    Name = hfName.Value,
                    Promotion=new PromotionEntity{Id=0},
                    Order = new OrderEntity { Id = Request.QueryString["OrderId"].Convert<long>() },
                    IsReturn = hfIsReturn.Convert<bool>(),
                    Description = hfDescription.Value,
                    SaveType = SaveType.Add
                };
                infos.Add(orderItem);
            }
            return infos;
        }
        /// <summary>
        /// 得到包装清单
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual string GetDescription(GoodsEntity goods, long productId)
        {
            if (goods == null || goods.GoodsDetails == null)
                return "";
            var goodsDetail = goods.GoodsDetails.FirstOrDefault(it =>it.Name== "PackageDescription" &&  it.Product != null && it.Product.Id == productId);
            if (goodsDetail != null)
                return goodsDetail.Detail;
            goodsDetail = goods.GoodsDetails.FirstOrDefault(it => it.Name == "PackageDescription" && it.Product != null && it.Product.Id == 0);
            if (goodsDetail != null)
                return goodsDetail.Detail;
            return "";
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override IList<BaseEntity> GetItemEntities(QueryInfo query)
        {
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(query);
        }
    }
}