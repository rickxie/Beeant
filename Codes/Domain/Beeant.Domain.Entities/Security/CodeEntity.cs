using System;
using System.Text;
using Component.Extension;

namespace Beeant.Domain.Entities.Security
{
    [Serializable]
    public class CodeEntity : BaseEntity<CodeEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public CodeType Type { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 有效时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }
        /// <summary>
        /// 发送地址
        /// </summary>
        public string ToAddress { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 步长频率
        /// </summary>
        public int SendStep { get; set; }
        /// <summary>
        /// 有效步长
        /// </summary>
        public int EffectiveStep { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
 
        /// <summary>
        /// 设置业务
        /// </summary>
        protected override void SetBusiness()
        {
            SetValue();
            SetEffectiveTime();
            Subject = GetSubject();
            Body = GetBody();
        }
        /// <summary>
        /// 设置有效时间
        /// </summary>
        protected virtual void SetEffectiveTime()
        {
            var json = Configuration.ConfigurationManager.GetSetting<string>("SecurityCode").DeserializeJson<dynamic>();
            int effectiveStep=EffectiveStep!=0? EffectiveStep: json == null || json.EffectiveStep == null ? 300 : json.EffectiveStep;
            SendStep=SendStep!=0?SendStep: json == null || json.SendStep == null ? 100 : json.SendStep;
            EffectiveTime = DateTime.Now.AddSeconds(effectiveStep);
            if (Properties == null) return;
            SetProperty(it => it.EffectiveTime);
        }


        /// <summary>
        /// 设置值
        /// </summary>
        protected virtual void SetValue()
        {
            var code = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                code.Append(random.Next(0, 9));
            }
            Value = code.ToString();
            if(Properties==null)return;
            SetProperty(it => it.Value);

        }
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <returns></returns>
        public virtual string GetSubject()
        {
            if(string.IsNullOrEmpty(Subject))
                Subject= Winner.Creator.Get<Winner.Dislan.ILanguage>().GetName(string.Format("{0}.{1}", GetType().FullName,Tag),string.Format("{0}Subject", Type));
            if (string.IsNullOrEmpty(Subject))
                return "";
            return string.Format(Subject, Value);
        }
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <returns></returns>
        public virtual string GetBody()
        {
            if (string.IsNullOrEmpty(Body))
                Body= Winner.Creator.Get<Winner.Dislan.ILanguage>().GetName(string.Format("{0}.{1}", GetType().FullName, Tag), string.Format("{0}Body", Type));
            if (string.IsNullOrEmpty(Body))
                return "";
            return string.Format(Body, Value);
        }
    }
}
