using System.Collections.Generic;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Supplier;

namespace Beeant.Application.Services.Finance
{
    public class BankApplicationService : RealizeApplicationService<BankEntity>
    {
        static protected readonly object Locker=new object();

        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<BankEntity> infos)
        {
            lock (Locker)
            {
                return base.Save(infos);
            }
        }
    }
}
