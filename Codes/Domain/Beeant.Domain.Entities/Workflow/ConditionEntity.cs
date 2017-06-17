using System;
using System.Linq;

namespace Beeant.Domain.Entities.Workflow
{

    [Serializable]
    public class ConditionEntity : BaseEntity<ConditionEntity>
    {
     
        /// <summary>
        /// 节点
        /// </summary>
        public NodeEntity Node { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public string InspectExp { get; set; }
        /// <summary>
        /// 表达式属性
        /// </summary>
        public string SelectExp { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
  
        /// <summary>
        /// 参数集合
        /// </summary>
        public string[] SelectExpArray
        {
            get
            {
                if (string.IsNullOrEmpty(SelectExp)) return null;
                return SelectExp.Split(',');
                
            }
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        public object[] ArgumentArray
        {
            get
            {
                if (string.IsNullOrEmpty(Argument)) return null;
                var arr = Argument.Split(',');
                return arr.Cast<object>().ToArray();
            }
        }
    }
}
