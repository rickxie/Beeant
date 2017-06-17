using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Security;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Security
{
    public class LockerDomainService :RealizeDomainService<LockerEntity>, ILockerDomainService
    {

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="locker"></param>
        /// <returns></returns>
        public virtual bool Check(LockerEntity locker)
        {
            return CheckLocker(locker);
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="locker"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Release(LockerEntity locker)
        {
            var info = GetLoginLocker(locker);
            if (info != null && info.IsUsed)
            {
                info.IsUsed = false;
                info.ErrorCount = 0;
                info.SaveType=SaveType.Modify;
                info.SetProperty(it => it.IsUsed).SetProperty(it=>it.ErrorCount);
                return Handle(info);
            }
            return null;
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="locker"></param>
        public virtual IList<IUnitofwork> Set(LockerEntity locker)
        {
            if(string.IsNullOrEmpty(locker.Tag))
                return null;
            var json = Configuration.ConfigurationManager.GetSetting<string>("SecurityLocker").DeserializeJson<dynamic>();
            int lockCount = json == null || json.LockCount == 0 ? 20 : json.LockCount;
            int lockTime = json == null || json.LockTime == 0 ? 300 : json.LockTime;
            var info = GetLoginLocker(locker);
            if (info == null)
            {
                locker.ErrorCount = 1;
                locker.IsUsed = lockCount <= 1;
                locker.LockTime=DateTime.Now.AddSeconds(lockTime);
                locker.SaveType=SaveType.Add;
            }
            else
            {
                locker.Id = info.Id;
                locker.ErrorCount = info.ErrorCount+1;
                locker.IsUsed = lockCount <= locker.ErrorCount;
                locker.LockTime = DateTime.Now.AddSeconds(lockTime);
                locker.SaveType = SaveType.Modify;
                locker.SetProperty(it => it.ErrorCount).SetProperty(it => it.IsUsed).SetProperty(it => it.LockTime);
            }
            return Handle(locker);

        }
        /// <summary>
        /// 检查错误次数
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        protected virtual bool CheckLocker(LockerEntity login)
        {
            var info = GetLoginLocker(login);
            if (info != null && info.IsUsed)
            {
                login.AddError("LoginLocker", info.LockTime);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 得到登录锁
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        protected virtual LockerEntity GetLoginLocker(LockerEntity login)
        {
            if (string.IsNullOrEmpty(login.Tag))
                return null;
            var query = new QueryInfo();
            query.Query<LockerEntity>().Where(it => it.Name == login.Name && it.Tag == login.Tag && it.LockTime > DateTime.Now).Select(it => new object[] { it.Id ,it.ErrorCount,it.IsUsed});
            var infos = Repository.GetEntities<LockerEntity>(query);
            return infos?.FirstOrDefault();
        }
    }
}
