using System;

namespace Beeant.Domain.Entities.Account
{
    [Serializable]
    public class AccountNumberEntity : BaseEntity<AccountNumberEntity>
    {
         
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        private string _number;

        /// <summary>
        /// OpenId
        /// </summary>
        public string Number
        {
            set
            {
                _number = value;
                
            }
            get
            {
                if (string.IsNullOrWhiteSpace(_number) && NumberEntity != null)
                    return NumberEntity.Id.ToString();
                return _number;
            }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 对应账户
        /// </summary>
        public AccountEntity Account { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public BaseEntity NumberEntity { get; set; }
     
    }
}
