using System;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.Inventory
{
    public partial class Inventory : MaintenPageBase<InventoryEntity>
    {
        public long ProductId
        {
            get { return Request.Params["ProductId"].Convert<long>(); }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStorehouse.LoadData();
                ddlType.LoadData();
                ckMonths.LoadData();
                ckWeeks.LoadData();
                ckCities.LoadData();
            }
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<InventoryEntity>().Where(it =>  it.Product.Id==ProductId);
            base.SetQueryWhere(query);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        protected override InventoryEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                //var dataEntity = GetInventory(info.Product.Id, info.Storehouse.Id);
                if (info.Id==0)
                {
                    info.Product = new ProductEntity { Id = ProductId };
                    info.SaveType = SaveType.Add;
                }
                else
                {
                    info.Id = info.Id;
                    info.SaveType = SaveType.Modify;
                    info.SetProperty(it => it.Recycle).SetProperty(it => it.WarningCount);
                }
            }
            
            return info;
        }
        /// <summary>
        /// 得到库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="storehouseId"></param>
        /// <returns></returns>
        protected InventoryEntity GetInventory(long productId, long storehouseId)
        {
            var query = new QueryInfo();
            query.Query<InventoryEntity>().Where(it => it.Product.Id == productId && it.Storehouse.Id == storehouseId).Select(it=>new object[]{it.Id});
            var infos= Ioc.Resolve<IApplicationService, InventoryEntity>().GetEntities<InventoryEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
        /// <summary>
        /// 加载实体
        /// </summary>
        /// <returns></returns>
        protected override InventoryEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<InventoryEntity>().Where(it => it.Id==RequestId).Select(it => new object[] { it,it.Storehouse.Name });
            var infos = Ioc.Resolve<IApplicationService, InventoryEntity>().GetEntities<InventoryEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
    }
}