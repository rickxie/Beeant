
using System;

namespace Beeant.Domain.Entities.Utility
{
    [Serializable]
    public class MobileEntity
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
        /// 发送的手机地址
        /// </summary>
        public string[] ToMobiles{ get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
    }
}
