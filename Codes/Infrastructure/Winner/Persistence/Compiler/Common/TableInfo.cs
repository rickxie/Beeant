using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winner.Persistence.Compiler.Common
{
    public  class TableInfo
    {
        private string _asName;
        /// <summary>
        /// 别名
        /// </summary>
        public string AsName
        {
            set { _asName = value; }
            get
            {
                if (string.IsNullOrEmpty(_asName))
                    _asName = CreateAsName();
                return _asName;
            }
        }
        /// <summary>
        /// 连接对象
        /// </summary>
        public IDictionary<string, JoinInfo> Joins { get; set; }
        /// <summary>
        /// 得到别名
        /// </summary>
        /// <returns></returns>
        public virtual string CreateAsName()
        {
            return string.Format("_{0}", Guid.NewGuid().ToString().Replace("-", ""));
        }

    }
}
