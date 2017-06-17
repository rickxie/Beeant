using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Api;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Api
{
    public class VoucherProtocolDomainService : RealizeDomainService<VoucherProtocolEntity>
    {
        #region 验证
    

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<VoucherProtocolEntity> infos)
        {
            bool rev = true;
            for (int i = 0; i < infos.Count; i++)
                for (int k = i + 1; k < infos.Count; k++)
                {
                    rev &= CompareEntities(infos[i], infos[k]);
                }
            return rev;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="info"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        protected virtual bool CompareEntities(VoucherProtocolEntity info, VoucherProtocolEntity compare)
        {
            string errorName = null;
            if (info.Protocol.Id.Equals(compare.Protocol.Id) && info.Voucher.Id.Equals(compare.Voucher.Id))
                errorName = "RepeatInList";
            if (string.IsNullOrEmpty(errorName)) return true;
            info.AddErrorByName(typeof(BaseEntity).FullName, errorName);
            return false;
        }
       
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(VoucherProtocolEntity info)
        {
            return ValidateProtocolExist(info) && ValidateVoucherExist(info) && ValidateVoucherProtocolExist(info);
        }



        /// <summary>
        /// 验证VoucherProtocol是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateVoucherProtocolExist(VoucherProtocolEntity info)
        {
            if (info.Voucher != null && info.Voucher.SaveType == SaveType.Add ||
                info.Protocol != null && info.Protocol.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<VoucherProtocolEntity>().Where(it => it.Protocol.Id == info.Protocol.Id
                                                      && it.Voucher.Id == info.Voucher.Id);
            var infos = Repository.GetEntities<VoucherProtocolEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistVoucherProtocol");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateVoucherExist(VoucherProtocolEntity info)
        {
            if (!info.HasSaveProperty(it => it.Voucher.Id))
                return true;
            if (info.Voucher != null && info.Voucher.SaveType == SaveType.Add)
                return true;
            if (info.Voucher != null && info.Voucher.Id!=0)
            {
                if (Repository.Get<VoucherEntity>(info.Voucher.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(VoucherEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProtocolExist(VoucherProtocolEntity info)
        {
            if (!info.HasSaveProperty(it => it.Protocol.Id))
                return true;
            if (info.Protocol != null && info.Protocol.SaveType == SaveType.Add)
                return true;
            if (info.Protocol != null && info.Protocol.Id!=0)
            {
                if (Repository.Get<ProtocolEntity>(info.Protocol.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(ProtocolEntity).FullName, "NoExist");
            return false;
        }
        #endregion

    }
}
