using System.Collections.Generic;

namespace Component.Extension
{
    public static class MergeExtension
    {
        /// <summary>
        /// 添加集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="desList"></param>
        public static void AddList<T>(this IList<T> sender, IList<T> desList)
        {
            if (sender == null || desList == null) return;
            foreach (var des in desList)
            {
                sender.Add(des);
            }
        }
    }
}
