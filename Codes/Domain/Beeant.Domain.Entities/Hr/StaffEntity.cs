using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;


namespace Beeant.Domain.Entities.Hr
{

    [Serializable]
    public class StaffEntity : BaseEntity<StaffEntity>
    {
    
        /// <summary>
        /// 
        /// </summary>
        public HrEntity Hr { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
       /// <summary>
       /// 性别
       /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 员工类型
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string WorkAddress { get; set; }

        /// <summary>
        /// 开始工作日期
        /// </summary>
        public DateTime StartWorkDate { get; set; }
        /// <summary>
        /// 入司日期
        /// </summary>
        public DateTime EnrollmentDate { get; set; }
        /// <summary>
        /// 社保所在地
        /// </summary>
        public string SocialSecurity { get; set; }
        /// <summary>
        /// 往来病史
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Classify { get; set; }
        /// <summary>
        /// 备注1
        /// </summary>
        public string Remark1 { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        public string Remark2 { get; set; }
        /// <summary>
        /// 备注3
        /// </summary>
        public string Remark3 { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company{ get; set; }
        /// <summary>
        /// 启用状态 
        /// </summary>
        public string IsUsedName
        {
            get {return this.GetStatusName(IsUsed); }
        }
        /// <summary>
        /// 家庭成员
        /// </summary>
        public IList<FamilyEntity> Families { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string BirthdayName
        {
            get { return Birthday == this.GetMinDateTime() ? "" : Birthday.ToString("yyyy-MM-dd"); }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public string StartWorkDateName
        {
            get { return StartWorkDate == this.GetMinDateTime() ? "" : StartWorkDate.ToString("yyyy-MM-dd"); }
        }
 
    }
    
}
