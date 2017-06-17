using System;

namespace Beeant.Domain.Entities.Supplier
{
    [Serializable]
    public enum ContractBillType
    {
        /// <summary>
        /// 增值税发票
        /// </summary>
        AddValue=1,

        /// <summary>
        /// 普通发票
        /// </summary>
        Common=2,

        /// <summary>
        /// 服务性发票
        /// </summary>
        Service=3
    }
}
