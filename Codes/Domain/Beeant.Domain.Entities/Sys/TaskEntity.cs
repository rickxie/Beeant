using System;
using System.Linq;
using Component.Extension;

namespace Beeant.Domain.Entities.Sys
{
    [Serializable]
    public class TaskEntity : BaseEntity<TaskEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 执行的类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 执行周期，单位分钟
        /// </summary>
        public int Recycle { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; }
   
        /// <summary>
        /// 参数
        /// </summary>
        public string Args { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 执行星期
        /// </summary>
        public string Weeks { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Months { get; set; }
        /// <summary>
        /// 日期数组
        /// </summary>
        public string[] MonthsArray
        {
            get
            {
                if (string.IsNullOrEmpty(Months)) return null;
                return Months.Split(',');
            }
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        public object[] ArgsArray
        {
            get
            {
                if (string.IsNullOrEmpty(Args)) return null;
                return Args.Split(',').Cast<object>().ToArray();
            }
        }

        /// <summary>
        /// 是否启动名称
        /// </summary>
        public string IsStartName
        {
            get { return this.GetServiceName(IsStart); }
        }
 
        /// <summary>
        /// 星期名称
        /// </summary>
        public string WeekName
        {
            get { return Weeks.GetEnums<DayOfWeek>().BuildeName(); }
        }
        /// <summary>
        /// 星期绑定
        /// </summary>
        public string BindWeeks
        {
            get
            {
                return Weeks.GetEnumComboStringValue<DayOfWeek>();
            }
            set { Weeks = value.GetEnumComboIntValue<DayOfWeek>(); }
        }
    }
}
