using Winner.Creation;

namespace Dependent
{
    static public class Ioc
    {

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T Resolve<T>(string name = null)
        {
            return Winner.Creator.Get<IFactory>().Get<T>(name);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEntityType"></typeparam>
        /// <returns></returns>
        static public T Resolve<T, TEntityType>(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? string.Format("{0},{1}",typeof(T).FullName,typeof(TEntityType).FullName) : name;
            var rev = Winner.Creator.Get<IFactory>().Get<T>(name);
            var t = rev as object;
            if (t == null) return Resolve<T>();
            return rev;
        }
    }


}
