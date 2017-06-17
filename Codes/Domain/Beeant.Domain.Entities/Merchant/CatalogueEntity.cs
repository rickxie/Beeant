using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Merchant
{
    [Serializable]
    public class CatalogueEntity : BaseEntity<CatalogueEntity>
    {
        /// <summary>
        /// 父类
        /// </summary>
        public CatalogueEntity Parent { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 子类
        /// </summary>
        public IList<CatalogueEntity> Children { get; set; }

        /// <summary>
        /// 子类
        /// </summary>
        public IList<CatalogueGoodsEntity> CatalogueGoodses { get; set; }
        /// <summary>
        /// 添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetAccount();
        }
        /// <summary>
        /// 修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            SetAccount();
        }
        /// <summary>
        /// 设置平台类型
        /// </summary>
        public virtual void SetAccount()
        {
            InvokeItemLoader("Parent");
            if (Parent == null || SaveType != SaveType.Add || Parent.Id==0) return;
            Account = Parent.Account;
            if (Properties == null) return;
            SetProperty(it => it.Account.Id);
        }
    }

}
