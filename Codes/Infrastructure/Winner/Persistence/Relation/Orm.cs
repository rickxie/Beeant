using System.Collections.Generic;

namespace Winner.Persistence.Relation
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class Orm:IOrm
    {
        #region 属性
        public IDictionary<string,OrmObjectInfo> Orms=new Dictionary<string, OrmObjectInfo>();
        #endregion

        #region 接口实现
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="info"></param>
        public virtual void AddOrm(OrmObjectInfo info)
        {
            Orms.Add(info.ObjectName, info);
            if(!Orms.ContainsKey(info.NickObjectName))
                Orms.Add(info.NickObjectName, info);
            if (!Orms.ContainsKey(info.ShortObjectName))
                Orms.Add(info.ShortObjectName, info);
        }
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual OrmObjectInfo GetOrm(string name)
        {
        
            return Orms[name];
        }
        /// <summary>
        /// 得到所有对象
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, OrmObjectInfo> GetOrms()
        {
            return Orms;
        }


        #endregion

       
    }
}
