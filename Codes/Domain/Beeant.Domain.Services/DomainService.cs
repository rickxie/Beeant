using System;
using System.Collections.Generic;
using System.Linq;
using Winner.Filter;
using Winner.Persistence;
using Beeant.Domain.Entities;

namespace Beeant.Domain.Services
{
    public class DomainService: MarshalByRefObject, IDomainService
    {
        #region 属性
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository{ get; set; }

     
        #endregion

        #region 接口的实现
        /// <summary>
        /// 添加对象到上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Handle<T>(T info) where T : BaseEntity
        {
            if (info == null)
                return null;
            if(info.HandleResult.HasValue)
                return new IUnitofwork[0];
            IList<IUnitofwork> result = null;
            if (Validate(info))
            {
                result= Repository.Save(info);
            }
            info.HandleResult = result != null;
            return result;
        }

        /// <summary>
        /// 添加对象到上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Handle<T>(IList<T> infos) where T : BaseEntity
        {
            if (infos == null)
                return null;
            var result=new List<IUnitofwork>();
            foreach (var info in infos)
            {
                var unitofworks = Handle(info);
                if (unitofworks == null)
                    return null;
                result.AddRange(unitofworks);
            }
            return result;
        }
        /// <summary>
        /// 设置委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public virtual void SetItemLoaders<T>(T info) where T : BaseEntity
        {
           
        }
        #endregion

        #region 方法

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool Validate<T>(T info) where T : BaseEntity
        {
            return ValidateEntity(info);
        }
        /// <summary>
        /// 验证信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateEntity<T>(T info) where T : BaseEntity
        {
            if (info == null) return false;
            if (info.SaveType != SaveType.None)
            {
                IDictionary<SaveType, ValidationType> temp = new Dictionary<SaveType, ValidationType>
                    { 
                    { SaveType.Add,ValidationType.Add },{ SaveType.Modify,ValidationType.Modify },{ SaveType.Remove,ValidationType.Remove }
                };
                info.Errors = Winner.Creator.Get<IValidation>().ValidateInfo(info, temp[info.SaveType], info.Properties);
            }
            var rev = info.Errors == null || info.Errors.Count == 0;
            return rev;
        }



        #endregion


    }
}
