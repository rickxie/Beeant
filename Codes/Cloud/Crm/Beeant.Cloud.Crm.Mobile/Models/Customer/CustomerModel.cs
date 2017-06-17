using System;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Cloud.Crm.Mobile.Models.Customer
{
    public class CustomerModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        ///联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string Weixin { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 提醒跟踪日期
        /// </summary>
        public string RemindNoteDate { get; set; }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual CustomerEntity CreateEntity(SaveType saveType)
        {
            var entity = new CustomerEntity
            {
                Name = string.IsNullOrWhiteSpace(Name) ? "" : Name,
                Type = new CustomerTypeEntity { Id= TypeId.Convert<long>() },
                Channel = new CustomerChannelEntity { Id = TypeId.Convert<long>() },
                Gender = string.IsNullOrWhiteSpace(Gender) ? "" : Gender,
                Qq = string.IsNullOrWhiteSpace(Qq) ? "" : Qq,
                Linkman = string.IsNullOrWhiteSpace(Linkman) ? "" : Linkman,
                Weixin = string.IsNullOrWhiteSpace(Weixin) ? "" : Weixin,
                Mobile = string.IsNullOrWhiteSpace(Mobile) ? "" : Mobile,
                Telephone = string.IsNullOrWhiteSpace(Telephone) ? "" : Telephone,
                Email = string.IsNullOrWhiteSpace(Email) ? "" : Email,
                Address = string.IsNullOrWhiteSpace(Address) ? "" : Address,
                Remark = string.IsNullOrWhiteSpace(Remark) ? "" : Remark,
                SaveType = saveType
            };
            entity.RemindNoteDate = string.IsNullOrWhiteSpace(RemindNoteDate)
                ? entity.GetMinDateTime()
                : RemindNoteDate.Convert<DateTime>();
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (TypeId != null)
                    entity.SetProperty(it => it.Type.Id);
                if (ChannelId != null)
                    entity.SetProperty(it => it.Channel.Id);
                if (Gender != null)
                    entity.SetProperty(it => it.Gender);
                if (Qq != null)
                    entity.SetProperty(it => it.Qq);
                if (Linkman != null)
                    entity.SetProperty(it => it.Linkman);
                if (Weixin != null)
                    entity.SetProperty(it => it.Weixin);
                if (Mobile != null)
                    entity.SetProperty(it => it.Mobile);
                if (Telephone != null)
                    entity.SetProperty(it => it.Telephone);
                if (Email != null)
                    entity.SetProperty(it => it.Email);
                if (Address != null)
                    entity.SetProperty(it => it.Address);
                if (Remark != null)
                    entity.SetProperty(it => it.Remark);
                if (RemindNoteDate != null)
                    entity.SetProperty(it => it.RemindNoteDate);
            }
            return entity;
        }
    }
}