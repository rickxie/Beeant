using System;

namespace Beeant.Domain.Entities.Api
{
    public class ApiEnginEntity
    {
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<string,string, VoucherProtocolEntity> GetVoucherProtocolHandle { get; set; }
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<string, VoucherEntity> GetVoucherHandle { get; set; }
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<string, ProtocolEntity> GetProtocolHandle { get; set; }

        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="token"></param>
        /// <param name="protocolName"></param>
        /// <returns></returns>
        public virtual VoucherProtocolEntity GetVoucherProtocol(string token,string protocolName)
        {
            if (GetVoucherProtocolHandle == null)
                return null;
            return GetVoucherProtocolHandle(token,protocolName);
        }

        /// <summary>
        /// 得到Voucher
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual VoucherEntity GetVoucher(string token)
        {
            if (GetVoucherHandle == null)
                return null;
            return GetVoucherHandle(token);
        }

        /// <summary>
        /// 得到Protocol
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ProtocolEntity GetProtocol(string name)
        {
            if (GetProtocolHandle == null)
                return null;
            return GetProtocolHandle(name);
        }
    }
}
