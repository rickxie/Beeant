using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;

namespace Beeant.Domain.Entities.Supplier
{
    [Serializable]
    public class SupplierEntity : BaseEntity<SupplierEntity>
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long ServiceId
        {
            get { return Service == null ? 0 : Service.Id; }
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { set; get; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// 县
        /// </summary>
        public string County { set; get; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 所在区县
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// 官网首页
        /// </summary>
        public string WebUrl { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessRange { get; set; }
        /// <summary>
        /// 经营品牌
        /// </summary>
        public string BusinessBrand { get; set; }
        /// <summary>
        /// 销售范围
        /// </summary>
        public string SalesRange { get; set; }
        /// <summary>
        /// 售后咨询电话
        /// </summary>
        public string ServiceTelephone { get; set; }
        /// <summary>
        /// 返修地址
        /// </summary>
        public string ServiceAddress { get; set; }
        /// <summary>
        /// 返修收货人
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 返修收货人联系方式
        /// </summary>
        public string ReceiverTelephone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>  
        public string Remark { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity Service { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SupplierStatusType Status { set; get; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get { return Status.GetName(); } }

        /// <summary>
        /// 其他证书
        /// </summary>
        public IList<CertificationEntity> Certifications { set; get; }

        /// <summary>
        /// 合同
        /// </summary>
        public IList<ContractEntity> Contracts { set; get; }

        /// <summary>
        /// 资质
        /// </summary>
        public IList<QualificationEntity> Qualifications { set; get; }
  
        
    }
}
