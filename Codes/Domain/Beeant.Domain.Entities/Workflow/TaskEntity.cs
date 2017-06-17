using System;
using System.Collections.Generic;
using System.Text;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Winner.Base;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public class TaskEntity : BaseEntity<TaskEntity>
    {
        /// <summary>
        /// 工作流类型
        /// </summary>
        public FlowEntity Flow { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public NodeEntity Node { get; set; }
        /// <summary>
        /// 数据编号
        /// </summary>
        public long DataId { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public BaseEntity Data { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string DataInfo { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public LevelEntity Level { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public DateTime OverTime { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public TaskStatusType Status { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleTime { get; set; } = "1990-01-01".Convert<DateTime>();
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// web方式显示业务信息
        /// </summary>
        public string WebDataEntity
        {
            get
            {
                if (string.IsNullOrEmpty(DataInfo)) return "";
                var rev = DataInfo.Replace("<", "&lt;")
                    .Replace(">&lt;", "><br/>&lt;")
                    .Replace("><br/>&lt;/", ">&lt;/");
                return rev;
            }
        }
     

        /// <summary>
        /// 填充属性
        /// </summary>
        /// <param name="properties"></param>
        public virtual void FillDataEntity(IList<PropertyEntity> properties)
        {
            if (properties == null || properties.Count == 0) return;
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                var value = Winner.Creator.Get<IProperty>().GetValue<object>(Data, property.Name);
                builder.AppendFormat("<{0}>{1}</{0}>",
                    property.Nickname, value);
            }
            DataInfo = builder.ToString();

        }
    }
}
