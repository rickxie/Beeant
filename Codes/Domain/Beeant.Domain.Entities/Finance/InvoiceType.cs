using System;

namespace Beeant.Domain.Entities.Finance
{
    [Serializable]
    public enum InvoiceType
    {
        /// <summary>
        /// 普通发票
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 增值税发票
        /// </summary>
        Vat = 2
    }
}