
using System;

namespace Beeant.Domain.Entities.Utility
{
    [Serializable]
    public class EmailEntity
    {
        private bool _isLog = true;
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog
        {
            get { return _isLog; }
            set { _isLog = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public Winner.Mail.MailInfo Mail { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
    }
}
