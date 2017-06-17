using System;


namespace Beeant.Domain.Entities.Authority
{

    [Serializable]
    public class ResourceEntity : BaseEntity<ResourceEntity>
    {
        
      
        /// <summary>
        ///名称
        /// </summary>
        public string Name{get;set;}
     
        /// <summary>
        ///Url 
        /// </summary>
        public string Url{ get; set; }
        /// <summary>
        /// 是否验证参数
        /// </summary>
        public bool IsValidateParamter { get; set; }
        /// <summary>
        /// 是否正则验证
        /// </summary>
        public bool IsRegexValidate { get; set; }
        /// <summary>
        ///控件 
        /// </summary>
        public string Controls { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否参数验证
        /// </summary>
        public string IsValidateParamterName
        {
            get { return this.GetStatusName(IsValidateParamter); }
        }
        /// <summary>
        /// 是否正则表达式验证
        /// </summary>
        public string IsRegexValidateName
        {
            get { return this.GetStatusName(IsRegexValidate); }
        }
        /// <summary>
        ///所属功能
        /// </summary>
        public AbilityEntity Ability { get; set; }
        /// <summary>
        /// 是否记录
        /// </summary>
        public bool IsExcude { get; set; }

        /// <summary>
        /// 是否记录
        /// </summary>
        public string IsExcudeName
        {
            get { return this.GetStatusName(IsExcude); }
        }
    }
 
}
