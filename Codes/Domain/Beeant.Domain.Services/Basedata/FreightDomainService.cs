using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Basedata
{
    public class FreightDomainService : RealizeDomainService<FreightEntity>
    {
        public IDomainService CaaryDomainService { get; set; }
        public IDomainService PostageDomainService { get; set; }

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
                        {"Caaries", new UnitofworkHandle<CarryEntity>{DomainService= CaaryDomainService}},
                        {"Postages", new UnitofworkHandle<PostageEntity>{DomainService= PostageDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        #region 重写验证

        /// <summary>
        /// 重载验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Validate(FreightEntity info)
        {
            var rev = base.Validate(info);
            if (rev)
                rev = ValidateOperator(info);
            return rev;
        }
        /// <summary>
        /// 验证操作
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOperator(FreightEntity info)
        {
            switch (info.SaveType)
            {
                case SaveType.Add:
                    return ValidateAdd(info);
                case SaveType.Modify:
                    return ValidateModify(info);
                case SaveType.Remove:
                    return ValidateRemove(info);
            }
            return true;
        }

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(FreightEntity info)
        {
            return ValidateAccount(info, null); 
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(FreightEntity info, FreightEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                    return true;
                var account = Repository.Get<AccountEntity>(info.Account.Id);
                if (account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!account.IsUsed)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }

        #endregion

        #region 修改验证

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(FreightEntity info)
        {
            var dataEntity = Repository.Get<FreightEntity>(info.Id);
            return ValidateAccount(info, dataEntity);
        }


        #endregion

        #region 删除验证

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(FreightEntity info)
        {
            return ValidateGoods(info);
        }
        /// <summary>
        /// 验证是否有商品绑定
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateGoods(FreightEntity info)
        {
            var query = new QueryInfo();
            query.SetPageIndex(0).SetPageSize(1).Query<GoodsEntity>().Where(it => it.Freight.Id == info.Id);
            var infos = Repository.GetEntities<GoodsEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("HasGoodsNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion



        #endregion


    }
}
