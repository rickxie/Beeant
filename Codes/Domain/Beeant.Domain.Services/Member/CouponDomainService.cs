using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Member
{
    public class CouponDomainService : RealizeDomainService<CouponEntity>, ICouponDomainService
    {
       
        /// <summary>
        /// 优惠券模板实例
        /// </summary>
        public IDomainService CouponerDomainService { get; set; }


        #region 重写事务

        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"Couponer", new UnitofworkHandle<CouponerEntity>{DomainService= CouponerDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<CouponEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CouponEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CouponEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"Couponer", LoadCouponer}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

     

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(CouponEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<CouponEntity>(info.Id);
        }

      
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadCouponer(CouponEntity info)
        {
            if (info.SaveType == SaveType.Add && info.Couponer != null && info.Couponer.Id != 0)
            {
                info.Couponer = info.Couponer.SaveType == SaveType.Add
                                    ? info.Couponer
                                    : Repository.Get<CouponerEntity>(info.Couponer.Id);
            }
            else
            {
                LoadDataEntity(info);
                if (info.DataEntity != null)
                    info.Couponer = Repository.Get<CouponerEntity>(info.DataEntity.Couponer.Id);
            }
        }
   
      
        #endregion

        #region 重写验证


        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<CouponEntity> infos)
        {
            var temps =
                infos.Where(it => it.Account != null && it.Account.Id != 0 && it.HasSaveProperty(s => s.Account.Id))
                     .ToList();
            if (temps.Count > 1)
            {
                foreach (var temp in temps)
                {
                    temp.AddErrorByName(typeof(BaseEntity).FullName, "NoAllowBatchSave");
                }
                return false;
            }
            return true;
        }
 

        #region 验证添加

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CouponEntity info)
        {
            return ValidateAccount(info, null) && ValidateCouponer(info)
                   && ValidateCollect(info, null) && ValidateUsed(info,null);
        }
        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCouponer(CouponEntity info)
        {
            if (!info.HasSaveProperty(it => it.Couponer.Id))
                return true;
            if (info.Couponer == null || info.Couponer.Id == 0)
                return true;
            var couponer = info.Couponer.SaveType == SaveType.Add
                             ? info.Couponer
                             : Repository.Get<CouponerEntity>(info.Couponer.Id);
            if (couponer == null)
            {
                info.AddErrorByName(typeof(CouponerEntity).FullName, "NoExist");
                return false;
            }
            if (!couponer.IsUsed)
            {
                info.AddErrorByName(typeof(CouponerEntity).FullName, "UnUsed");
                return false;
            }
            if (couponer.CollectEndDate < DateTime.Now)
            {
                info.AddErrorByName(typeof(CouponerEntity).FullName, "CollectDateOut");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateAccount(CouponEntity info,CouponEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null 
                || info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            var account = Repository.Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof (AccountEntity).FullName, "NoExist");
                return false;
            }
            if (!account.IsUsed)
            {
                info.AddErrorByName(typeof (AccountEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }

       
        /// <summary>
        /// 验证领取
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCollect(CouponEntity info, CouponEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account != null && info.Account.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            return ValidateCode(info) && ValidateCollectCount(info, dataEntity);
        }

     

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCode(CouponEntity info)
        {
            if (info.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<CouponEntity>().Where(it => it.Id == info.Id && it.Code == info.Code && it.Account.Id==0)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<CouponEntity>(query);
            if (infos != null && infos.Count > 0)
                return true;
            info.AddError("CodeError");
            return false;
        }

        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCollectCount(CouponEntity info, CouponEntity dataEntity)
        {
            CouponerEntity couponer=null;
            if (info.SaveType == SaveType.Add)
            {
                couponer = info.Couponer.SaveType == SaveType.Add
                             ? info.Couponer
                             : Repository.Get<CouponerEntity>(info.Couponer.Id);
            }
            else if (dataEntity != null && dataEntity.Couponer != null && dataEntity.Couponer.Id != 0)
            {
                couponer = Repository.Get<CouponerEntity>(dataEntity.Couponer.Id);
            }
            if (couponer == null || couponer.Id==0)
                return true;
            var query = new QueryInfo();
            query.Query<CouponEntity>().Where(it => it.Couponer.Id == couponer.Id && 
                it.Account.Id == info.Account.Id).Select(it=>it.Id);
            var infos = Repository.GetEntities<CouponEntity>(query);
            if (infos != null &&  infos.Count < couponer.CollectCount)
                return true;
            info.AddErrorByName(typeof(CouponerEntity).FullName, "CollectCountOut", couponer.CollectCount);
            return false;
        }

        #endregion

        #region 修改验证

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(CouponEntity info)
        {
            var dataEntity = Repository.Get<CouponEntity>(info.Id);
            return ValidateUsed(info, dataEntity)
                   && ValidateAccount(info, dataEntity) && ValidateCollect(info, dataEntity)
                   && ValidateChangeAccount(info,dataEntity);
        }

        /// <summary>
        /// 验证是否使用过
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateUsed(CouponEntity info, CouponEntity dataEntity)
        {
            if (info.SaveType == SaveType.Add && !info.IsUsed)
                return true;
            if (!info.HasSaveProperty(it => it.IsUsed))
                return true;
            if (dataEntity!=null && info.IsUsed == dataEntity.IsUsed)
                return true;
            return ValidateCouponUsed(info, dataEntity);
        }
        /// <summary>
        /// 验证是否使用过
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCouponUsed(CouponEntity info, CouponEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.EndDate) && dataEntity != null)
                info.EndDate = dataEntity.EndDate;
            if (dataEntity!=null && dataEntity.IsUsed)
            {
                info.AddError("AlreadyUsed");
                return false;
            }
            if (info.EndDate < DateTime.Now.Date)
            {
                info.AddError("EndDateOut");
                return false;
            }
            return true;
        }

  
        /// <summary>
        /// 验证优惠券
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateChangeAccount(CouponEntity info, CouponEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (dataEntity.Account == null || dataEntity.Account.Id == 0)
                return true;
            if (dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            info.AddError("NotAllowChangeAccount");
            return false;
        }

        #endregion




        #endregion

        #region 接口的实现

        /// <summary>
        /// 优惠券生成
        /// </summary>
        /// <param name="couponerId"></param>
        /// <returns></returns>
        public virtual IList<CouponEntity> CreateCoupon(long couponerId)
        {
            var couponer = Repository.Get<CouponerEntity>(couponerId);
            if (couponer == null)
                return null;
            var infos = new List<CouponEntity>();
            for (int i = 0; i < couponer.Count; i++)
            {
                var code = "";
                if (couponer.IsCode)
                {
                    var rd=new Random(Guid.NewGuid().GetHashCode());
                    code = rd.Next(100000, 999999).ToString();
                }
                infos.Add(new CouponEntity
                    {
                        Amount = couponer.Amount,
                        Account = new AccountEntity {Id = 0},
                        CollectTime = DateTime.Now,
                        Couponer = couponer,
                        EndDate = couponer.EndDate,
                        UsedTime=DateTime.Now,
                        Remark=couponer.Remark,
                        Name=couponer.Name,
                        Code = code,
                        SaveType=SaveType.Add,
                        
                    });
            }
            return infos;
        }

      
        #endregion
    }
}
