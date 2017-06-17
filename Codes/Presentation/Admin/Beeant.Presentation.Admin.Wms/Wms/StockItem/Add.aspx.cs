using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.StockItem
{
    public partial class Add : AddPageBase<StockItemEntity>
    {
        public long StockId
        {
            get { return Request.QueryString["StockId"].Convert<long>(); }
        }
   
        protected override StockItemEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Stock = new StockEntity { Id = StockId };
                info.User = new UserEntity { Id = Identity.Id };


            }
            return info;
        }
        protected override void LoadEntity()
        {
            var order = this.GetEntity<StockEntity>(StockId);
            if (order.Status != StockStatusType.WaitHandle)
                ((AuthorizePageBase)Page).InvalidateData("您没有权限在该状态下添加出入库明细");
            
                base.LoadEntity();
        }
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <returns></returns>
        protected override StockItemEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<StockItemEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Storehouse.Name, it });
            var infos = Ioc.Resolve<IApplicationService, StockItemEntity>().GetEntities<StockItemEntity>(query);
            return infos==null?null:infos.FirstOrDefault();
        }

        protected override void SetResult(bool rev, System.Collections.Generic.IList<Winner.Filter.ErrorInfo> errors)
        {
            base.SetResult(rev,errors);
            if (errors == null || errors.Count == 0)
            {
                hfProduct.Visible = true;
                hfProduct.HRef = string.Format("{0}{1}", hfProduct.HRef, StockId);
            }
              
        }
    }
}