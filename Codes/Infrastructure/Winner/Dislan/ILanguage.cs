using System.Collections.Generic;

namespace Winner.Dislan
{
    public interface ILanguage
    {
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetName(string key,string name);

        /// <summary>
        /// 添加名称
        /// </summary>
        /// <param name="key"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        bool AddNames(string key, IList<LanguageInfo> infos);
        /// <summary>
        /// 移除名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveName(string key);
        /// <summary>
        /// 得到包含名称为name的所有集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<LanguageInfo> GetNames(string name);
    }
}
