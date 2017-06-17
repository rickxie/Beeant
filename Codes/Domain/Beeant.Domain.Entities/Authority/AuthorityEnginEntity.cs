using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace Beeant.Domain.Entities.Authority
{
    public class AuthorityEnginEntity
    {
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<long, long[]> GetRolesHandle { get; set; }
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<long,string, OwnerEntity> GetOnwerHandle { get; set; }
        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual long[] GetRoles(long accountId)
        {
            if (GetRolesHandle == null)
                return null;
            return GetRolesHandle(accountId);
        }

        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual OwnerEntity GetOnwers(long accountId,string name)
        {
            if (GetOnwerHandle == null)
                return null;
            return GetOnwerHandle(accountId, name);
           
        }
    }
}
