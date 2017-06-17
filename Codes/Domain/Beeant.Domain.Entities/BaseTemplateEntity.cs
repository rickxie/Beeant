using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Entities
{
    [Serializable]
    public class BaseEntity<T> : BaseEntity where T:BaseEntity
    {
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual BaseEntity SetProperty<TEntityType>(Expression<Func<TEntityType, object>> expression)
        {
            Winner.Persistence.Linq.EntityExtension.SetProperty(this, expression);
            return this;
        }
        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual BaseEntity Where<TEntityType>(Expression<Func<TEntityType, bool>> predicate)
        {
            Winner.Persistence.Linq.EntityExtension.Where(this, predicate);
            return this;
        }

        /// <summary>
        /// 是否存储该属性
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool HasSaveProperty<TEntityType>(Expression<Func<TEntityType, object>> expression)
        {
            return base.HasSaveProperty(ExpressionHelper.ReplaceExpressionParamter(expression.ToString()));
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual BaseEntity<T> SetProperty(Expression<Func<T, object>> expression)
        {
            Winner.Persistence.Linq.EntityExtension.SetProperty(this, expression);
            return this;
        }
        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual BaseEntity<T> Where(Expression<Func<T, bool>> predicate)
        {
            Winner.Persistence.Linq.EntityExtension.Where(this, predicate);
            return this;
        }

        /// <summary>
        /// 是否存储该属性
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool HasSaveProperty(Expression<Func<T, object>> expression)
        {
            return base.HasSaveProperty(ExpressionHelper.ReplaceExpressionParamter(expression.ToString()));
        }
        #region 重写业务

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="itemLoaders"></param>
        public override void SetItemLoaders<TEntity>(IDictionary<string, ItemLoader<TEntity>> itemLoaders)
        {
            ItemLoaders = itemLoaders as IDictionary<string, ItemLoader<T>>;
        }
        /// <summary>
        /// 重写事务
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="itemLoaders"></param>
        public override void SetBusiness<TEntity>( IDictionary<string, ItemLoader<TEntity>> itemLoaders)
        {
            ItemLoaders = itemLoaders as IDictionary<string, ItemLoader<T>>;
            SetBusiness();
        }
        /// <summary>
        /// 加载委托
        /// </summary>
        protected virtual IDictionary<string, ItemLoader<T>> ItemLoaders { get; set; }
       

        /// <summary>
        /// 重写业务
        /// </summary>
        protected virtual void SetBusiness()
        {
            switch (SaveType)
            {
                case SaveType.Add:
                    SetAddBusiness();
                    return;
                case SaveType.Modify:
                    SetModifyBusiness();
                    return;
                case SaveType.Remove:
                    SetRemoveBusiness();
                    return;
            }
        }
        /// <summary>
        /// 添加业务
        /// </summary>
        protected virtual void SetAddBusiness()
        {
            
        }
        /// <summary>
        /// 添加业务
        /// </summary>
        protected virtual void SetModifyBusiness()
        {
            
        }
        /// <summary>
        /// 添加业务
        /// </summary>
        protected virtual void SetRemoveBusiness()
        {

        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void InvokeItemLoader(string propertyName)
        {
            if (ItemLoaders != null && ItemLoaders.ContainsKey(propertyName))
                ItemLoaders[propertyName](this as T);
        }
        #endregion
    }
 
}
