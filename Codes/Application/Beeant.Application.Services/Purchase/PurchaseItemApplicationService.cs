using System.Collections.Generic;
using Beeant.Domain.Entities.Purchase;

namespace Beeant.Application.Services.Purchase
{
    public class PurchaseItemApplicationService : RealizeApplicationService<PurchaseItemEntity>
    {
        static protected readonly object Locker=new object();
     
        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<PurchaseItemEntity> infos)
        {
            lock (Locker)
            {
                var rev = Handle(infos);
                if (rev != null)
                    return Commit(rev);
                return false;
            }
        }
   
     
        
    }
}
