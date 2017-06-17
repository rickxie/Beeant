using System.Collections.Generic;
using Beeant.Domain.Entities.Purchase;

namespace Beeant.Application.Services.Purchase
{
    public class PayApplicationService : RealizeApplicationService<PurchasePayEntity>
    {
        static protected readonly object Locker=new object();

        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<PurchasePayEntity> infos)
        {
            lock (Locker)
            {
               return base.Save(infos);
            }
        }
    }
}
