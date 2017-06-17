using System.Collections.Generic;
using Beeant.Domain.Entities.Member;
using Winner.Persistence;

namespace Beeant.Presentation.Mobile.Member.Models.Member
{
    public class AddressModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<AddressEntity> Addresses { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 是否默认地址
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址标签
        /// </summary>
        public string Tag { get; set; }

        public AddressEntity AddressEntity { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public virtual AddressEntity CreateEntity(SaveType saveType)
        {
            var entity = new AddressEntity
            {
                Country= "中国",
                Province = Province,
                City = City,
                County = County,
                Recipient = Recipient,
                Mobile = Mobile,
                Postcode = Postcode,
                Address = Address,
                Telephone = Telephone,
                Company = Company,
                IsDefault = IsDefault,
                Email = Email,
                Tag = Tag,
                SaveType=saveType
            };
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id;
                entity.SetProperty(it=>it.Country).SetProperty(it => it.Province).SetProperty(it => it.City)
                    .SetProperty(it => it.County).SetProperty(it => it.Recipient)
                    .SetProperty(it => it.Mobile).SetProperty(it => it.Postcode)
                    .SetProperty(it => it.Address).SetProperty(it => it.Telephone)
                    .SetProperty(it => it.Company).SetProperty(it => it.IsDefault)
                    .SetProperty(it => it.Email).SetProperty(it => it.Tag);
            }
            return entity;
        }
    }
}
