using System;

namespace Beeant.Application.Dtos.Order
{
    public class OrderInsuranceDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

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
        /// 文件名
        /// </summary>
        public string AttachmentName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string AttachmentFileName { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public byte[] AttachmentFileByte { get; set; }
    }
  
}
