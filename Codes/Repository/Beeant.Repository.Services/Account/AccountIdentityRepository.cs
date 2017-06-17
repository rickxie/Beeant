using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Wcf;

namespace Beeant.Repository.Services.Account
{
    public class AccountIdentityRepository : Repository
    {
        public IWcfService WcfService { get; set; }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public override IList<IUnitofwork> Save<T>(T info)
        {
            return GetUnitofworks(info);
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override IList<IUnitofwork> Save<T>(IList<T> infos)
        {
            var result = new List<IUnitofwork>();
            foreach (var info in infos)
            {
                var unitofworks = GetUnitofworks(info);
                result.AddList(unitofworks);
            }
            return result;
        }

        /// <summary>
        /// 得到工作单元
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> GetUnitofworks<T>(T info) where T : BaseEntity
        {
            if (typeof (T) != typeof (AccountIdentityEntity))
                return base.Save(info);
            if (info.SaveType == SaveType.Add)
            {
                return new List<IUnitofwork> {new AccountIdentityUnitofwork(info as AccountIdentityEntity, WcfService) };
            }
            return base.Save(info);
        }
    }
}
