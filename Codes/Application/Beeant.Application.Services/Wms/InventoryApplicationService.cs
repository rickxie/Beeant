using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Wms
{
    public class InventoryEventApplicationService : RealizeApplicationService<InventoryEntity>, IJobApplicationService
    {


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Execute(object[] args)
        {
           
            var i = 0;
            while (true)
            {
                var infos = GetInventories(i);
                if(infos==null || infos.Count==0)
                    break;
                UpdateProducts(infos);
                i++;
            }
            return true;
        }

        /// <summary>
        /// 得到产品
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual void UpdateProducts(IList<InventoryEntity> infos)
        {
            IList<ProductEntity> products = new List<ProductEntity>();
            foreach (var info in infos)
            {
                if (info.Product == null)
                    continue;
                if (string.IsNullOrEmpty(info.Weeks) ||
                    !info.Weeks.Contains(((int) DateTime.Now.DayOfWeek).ToString(CultureInfo.InvariantCulture)))
                    continue;
                if (info.MonthsArray == null ||
                    info.MonthsArray.Count(it => it.Equals((DateTime.Now.Day).ToString(CultureInfo.InvariantCulture))) ==
                    0)
                    continue;
                if (info.Type == InvertoryType.Reset && info.Recycle != 0
                    && (DateTime.Now - info.StartTime).TotalMinutes%info.Recycle == 0)
                    continue;
                
                var product = new ProductEntity
                {
                    Id = info.Product.Id,
                    Count= info.Count,
                    SaveType = SaveType.Modify
                };
                product.SetProperty(it => it.Count);
                products.Add(product);
            }
            var unitofworks = Repository.Save(products);
            Commit(unitofworks);
        }

        /// <summary>
        /// 得到库存
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected virtual  IList<InventoryEntity> GetInventories(int pageIndex)
        {
            var query=new QueryInfo();
            query.SetPageIndex(pageIndex).SetPageSize(100).Query<InventoryEntity>()
                .Where(it=>it.Type==InvertoryType.Reset)
                .Select(it=>new object[] {it.Product.Id,it.Type,it.Months,it.Weeks,it.TimingTime,it.Recycle});
            var infos = Repository.GetEntities<InventoryEntity>(query);
            return infos;
        }
    }
}
