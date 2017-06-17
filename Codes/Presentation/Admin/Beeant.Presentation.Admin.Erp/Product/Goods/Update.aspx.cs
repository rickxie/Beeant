using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    public partial class Update : UpdatePageBase<GoodsEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }



        protected override GoodsEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Category != null)
                    info.Category =
                        Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntity<CategoryEntity>(info.Category.Id);
                if (info.Account != null)
                    info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);                
            }
            if (info != null && info.IsSales)
            {
                if (info.Account != null && info.Account.Id > 0)
                {
                    InvalidateData("商品不是平台自营商品不能编辑");
                }
                else if (info.IsSales)
                {
                    InvalidateData("上架的商品不能编辑");
                }
            }

            return info;
        }

        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override GoodsEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                var values = Edit1.TagCheckBoxList.GetSelectedValues();
                if (values.Length > 0)
                {
                    info.Tag = values;
                }
                info.IsSales = false;
                info.Products = Edit1.GetProducts(info);
                info.GoodsImages = Edit1.GetGoodsImages(info);
                info.GoodsProperties = Edit1.GetGoodsProperties(info);
                info.GoodsDetails = Edit1.GetGoodsDetails(info);
                info.PublishTime = DateTime.Now;
                info.Category = new CategoryEntity { Id = Edit1.CategoryId };
    
            }
            return info;
        }
    
      
    }
}