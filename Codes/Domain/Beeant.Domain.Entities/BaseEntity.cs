using System;
using System.Collections.Generic;
using Winner;
using Winner.Dislan;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Domain.Entities
{
    public delegate void ItemLoader<in T>(T info);
    [Serializable]
    public class BaseEntity : EntityInfo
    {

        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 验证是否通过
        /// </summary>
        public virtual bool? HandleResult { get;set; }

        /// <summary>
        /// ID
        /// </summary>
        public virtual long Id{ get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        public virtual DateTime InsertTime { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        ///删除时间
        /// </summary>
        public virtual DateTime DeleteTime { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public virtual int Mark { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version { get; set; }
        /// <summary>
        /// 存储顺序
        /// </summary>
        public virtual int SaveSequence { get; set; }
        /// <summary>
        /// 是否启用批量插入
        /// </summary>
        public virtual bool IsBulkCopy { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public virtual string EventName { get; set; }
        
        /// <summary>
        /// 重写SaveType
        /// </summary>
        public override SaveType SaveType
        {
            get
            {
                return base.SaveType;
            }
            set
            {
                switch (value)
                {
                    case SaveType.Add: Mark = 1; 
                        InsertTime = DateTime.Now;

                        UpdateTime = DateTime.Now;
                        DeleteTime = DateTime.Now;
                        if (Properties != null)
                            SetProperty("Mark").SetProperty("InsertTime")
                                               .SetProperty("UpdateTime").SetProperty("DeleteTime");
                        break;
                    case SaveType.Modify: Mark = 2;
                        UpdateTime = DateTime.Now;
                        if (Properties != null)
                            SetProperty("Mark").SetProperty("UpdateTime");
                            break;
                    case SaveType.Remove: Mark = 0; 
                        DeleteTime = DateTime.Now; 
                        if (Properties != null)
                            SetProperty("Mark").SetProperty("DeleteTime");
                            break;
                }
                base.SaveType = value;
            }
        }

        #region 通用方法
        /// <summary>
        /// 添加错误信息
        /// </summary>
        public virtual void AddError(ErrorInfo error)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            Errors.Add(error);
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddError(string propertyName, params object[] paramters)
        {
            AddErrorByName(GetType().FullName, propertyName, paramters);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddErrorByName(string name, string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            Errors.Add(GetErrorByName(name, propertyName, paramters));
        }
        /// <summary>
        /// 得到错误
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual ErrorInfo GetErrorByName(string name, string propertyName, params object[] paramters)
        {
            var error = Creator.Get<IValidation>().GetErrorInfo(name, propertyName);
            error.Message = string.Format(error.Message, paramters);
            return error;
        }

        /// <summary>
        /// 得到错误
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual ErrorInfo GetError(string propertyName, params object[] paramters)
        {
            return GetErrorByName(GetType().FullName, propertyName, paramters);
        }
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string GetLanguage(string propertyName, object value)
        {
            return
                Creator.Get<ILanguage>()
                             .GetName(string.Format("{0}.{1}", GetType().FullName, propertyName), value.ToString());
        }

        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string GetLanguage(string entityName, string propertyName, object value)
        {
            return
                Creator.Get<ILanguage>()
                             .GetName(string.Format("{0}.{1}", entityName, propertyName), value.ToString());
        }

        /// <summary>
        /// 是否存储该属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool HasSaveProperty(string name)
        {
            if (SaveType != SaveType.Modify && SaveType!=SaveType.Add) return false;
            if (Properties == null || Properties.Contains(name)) return true;
            return false;
        }


        /// <summary>
        /// 设置业务
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="itemLoaders"></param>
        public virtual void SetBusiness<TEntity>(IDictionary<string, ItemLoader<TEntity>> itemLoaders) where TEntity : BaseEntity
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="itemLoaders"></param>
        public virtual void SetItemLoaders<TEntity>(IDictionary<string, ItemLoader<TEntity>> itemLoaders)
        {
            
        }
        #endregion
    }
 
}
