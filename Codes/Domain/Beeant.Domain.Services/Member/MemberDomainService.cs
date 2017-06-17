using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Member
{
    public class MemberDomainService : RealizeDomainService<MemberEntity>
    {
        /// <summary>
        /// 账号信息
        /// </summary>
        public IDomainService AccountDomainService { get; set; }

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
                        {"Account", new UnitofworkHandle<AccountEntity>{DomainService= AccountDomainService} },
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "HeadUrl",BytePropertyName = "HeadUrlByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
    

        #region 重写验证

 

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(MemberEntity info)
        {
            var rev = ValidateAccount(info, null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(MemberEntity info)
        {
            var dataEntity = Repository.Get<MemberEntity>(info.Id);
            return ValidateAccount(info,dataEntity);
        }
      
        #endregion

        #region 删除验证

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(MemberEntity info)
        {
            var dataEntity = Repository.Get<MemberEntity>(info.Id);
            if (dataEntity.Account != null && dataEntity.Account.Id!=0)
            {
                info.AddError("OpenAccountNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(MemberEntity info ,MemberEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id) 
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
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
            var query = new QueryInfo();
            query.Query<MemberEntity>().Where(it => it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<MemberEntity>(query);
            if (infos != null && infos.Count(it => it.Id != info.Id) > 0)
            {
                info.AddError("AccountHasMember");
                return false;
            }
            return true;
        }

    
        #endregion

    }
}
