using System.Collections.Generic;
using System.Linq;

namespace Winner.Dislan
{
    public class Language : ILanguage
    {
        #region 属性

        private IDictionary<string, IDictionary<string,LanguageInfo>> _names = new Dictionary<string, IDictionary<string,LanguageInfo>>();
        /// <summary>
        /// 名称集合
        /// </summary>
        protected IDictionary<string, IDictionary<string, LanguageInfo>> Names 
        { 
            get { return _names; }
            set { _names = value; }
        }

        #endregion
        #region 接口的实现

        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetName(string key, string name)
        {
            if (Names.ContainsKey(key) && Names[key]!=null && Names[key].ContainsKey(name) && Names[key][name]!=null)
            {
                return Names[key][name].Message;
            }
            return null;
        }
        /// <summary>
        /// 添加名称
        /// </summary>
        /// <param name="key"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual bool AddNames(string key, IList<LanguageInfo> infos)
        {
            if (Names.ContainsKey(key))
                return false;
            var dis=new Dictionary<string, LanguageInfo>();
            foreach (var info in infos)
            {
                if(!dis.ContainsKey(info.Name))
                    dis.Add(info.Name,info);
            }
            Names.Add(key, dis);
            return true;
        }

      
     
        /// <summary>
        /// 移除名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveName(string key)
        {
            if (!Names.ContainsKey(key))
                return false;
            Names.Remove(key);
            return true;
        }

        public virtual IList<LanguageInfo> GetNames(string key)
        {
            if (!Names.ContainsKey(key))
                return null;
            return Names[key].Values.ToList();
        }

 

        #endregion 
    }
}
