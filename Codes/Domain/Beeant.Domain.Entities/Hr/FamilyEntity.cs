using System;


namespace Beeant.Domain.Entities.Hr
{

    [Serializable]
    public class FamilyEntity : BaseEntity<FamilyEntity>
    {
    
        /// <summary>
        /// 
        /// </summary>
        public HrEntity Hr { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public StaffEntity Staff { get; set; }
       
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        ///身份证
        /// </summary>
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 既往病史
        /// </summary>
        public string MedicalHistory { get; set; }

        /// <summary>
        /// 添加处理
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Staff");
            if (Staff == null)
                return;
            Hr = Staff.Hr;
            base.SetAddBusiness();
        }

        /// <summary>
        /// 生日
        /// </summary>
        public string BirthdayName
        {
            get { return Birthday == this.GetMinDateTime() ? "" : Birthday.ToString("yyyy-MM-dd"); }
        }
    }
    
}
