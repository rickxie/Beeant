using System;
using System.Web;

namespace Winner.Persistence.ContextStorage
{

    public class HttpContextStorage :ThreadContextStorage
    {
        private static readonly string ItemName = Guid.NewGuid().ToString().Replace("-", "");
        /// <summary>
        /// 得到上下文
        /// </summary>
        /// <returns></returns>
        public override ContextInfo Get()
        {
            if (HttpContext.Current == null) return base.Get();
            return (ContextInfo)HttpContext.Current.Items[ItemName];
        }
        /// <summary>
        /// 设置上下文
        /// </summary>
        /// <param name="contexnt"></param>
        public override void Set(ContextInfo contexnt)
        {
            if (HttpContext.Current == null)
            {
                base.Set(contexnt);
                return;
            }
            if (contexnt != null)
            {
                HttpContext.Current.Items.Add(ItemName, contexnt);
            }
            else
            {
                HttpContext.Current.Items.Remove(ItemName);
            }
        }
    }

}
