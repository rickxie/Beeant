using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Api
{
    public class VoucherEntity : BaseEntity<VoucherEntity>
    {
        /// <summary>
        /// 凭证 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 帐户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否禁止 
        /// </summary>
        public VoucherType Type { get; set; }
        /// <summary>
        /// IP白名单
        /// </summary>
        public string Ips { get; set; }
        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog { get; set; }
        /// <summary>
        /// 是否禁止名称
        /// </summary>
        public string IsLogName
        {
            get { return this.GetStatusName(IsLog); }
        }

        private IDictionary<string, string> _ipsArray;

        public IDictionary<string, string> IpsArray
        {
            get
            {
                if (_ipsArray != null)
                    return _ipsArray;
                if (string.IsNullOrWhiteSpace(Ips))
                    return null;
                _ipsArray= Ips.Split(',').GroupBy(it => it).ToDictionary(it => it.Key, it=>"");
                return _ipsArray;
            }
        }
        /// <summary>
        /// 限制协议
        /// </summary>
        public IList<VoucherProtocolEntity> VoucherProtocols { get; set; }
        /// <summary>
        /// 协议验证
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }

        protected override void SetAddBusiness()
        {
            if (string.IsNullOrWhiteSpace(Token))
                Token = Guid.NewGuid().ToString();
            base.SetAddBusiness();
        }

        /// <summary>
        /// 保护协议
        /// </summary>
        public IDictionary<string, VoucherProtocolEntity> ProtocolDictionaries { get; set; }
        /// <summary>
        /// 设置字典
        /// </summary>
        public virtual void SetProtocolDictionaries()
        {
            if(VoucherProtocols==null)
                return;
            ProtocolDictionaries=new Dictionary<string, VoucherProtocolEntity>();
            foreach (var info in VoucherProtocols)
            {
                if(info.Protocol==null)
                    continue;
                ProtocolDictionaries.Add(info.Protocol.Name,info);
            }
        }
        /// <summary>
        /// 检查协议
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool CheckProtocol(string name)
        {
            if (ProtocolDictionaries == null)
                return false;
            return ProtocolDictionaries.ContainsKey(name);
        }
    }
}
