using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Hr;

namespace Beeant.Domain.Entities.Crm
{
    [Serializable]
    public class CustomerEntity : BaseEntity<CustomerEntity>
    {

        /// <summary>
        /// 客户信息
        /// </summary>
        public CrmEntity Crm { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerTypeEntity Type { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerChannelEntity Channel { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public StaffEntity Staff { get; set; }
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
        public DateTime RemindNoteDate { get; set; }
        /// <summary>
        /// 维护记录
        /// </summary>
        public IList<CustomerNoteEntity> CustomerNotes { get; set; }
 
    }
}
