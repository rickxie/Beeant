using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.StockItem
{
    public partial class Update : UpdatePageBase<StockItemEntity>
    {
     
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

    
        protected override StockItemEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.User = new UserEntity { Id = Identity.Id };
                info.SetProperty(it => it.User.Id);
           
            }
            return info;
        }
        protected override StockItemEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<StockItemEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Storehouse.Name, it});
            var infos = Ioc.Resolve<IApplicationService, StockItemEntity>().GetEntities<StockItemEntity>(query);
            var info = infos==null ? null : infos.FirstOrDefault();
            if (info != null)
            {
                info.Stock = Ioc.Resolve<IApplicationService, StockEntity>().GetEntity<StockEntity>(info.Stock.Id);
                if (info.Stock == null || info.Stock.Status != StockStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("您没有权限在该状态下修改出入库明细");
            }   
            return info;
        }
 
    }
}