using System;
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Application.Services
{
    /// <summary>
    ///
    /// </summary>
    public enum EventHandleSequenceType
    {
        /// <summary>
        /// 存储前
        /// </summary>
        BeforeSave=1,
        /// <summary>
        /// 提交前
        /// </summary>
        BeforeCommit=2,
        /// <summary>
        /// 保存后
        /// </summary>
        AfterCommit = 3
    }
    /// <summary>
    /// 事件参数
    /// </summary>
    public class EventHandleArgs<TEntityType> where TEntityType:BaseEntity
    {
        /// <summary>
        /// 事件源
        /// </summary>
        public EventHandle<TEntityType> Sender { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        public TEntityType Entity { get; set; }
        /// <summary>
        /// 工作单元
        /// </summary>
        public IList<IUnitofwork> Unitofworks { get; set; } 
    }
    public class EventHandle<TEntityType> where TEntityType : BaseEntity
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 处理
        /// </summary>
        public Action<EventHandleArgs<TEntityType>> Handle { get; set; }

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool IsAsynchronization { get; set; }
        /// <summary>
        /// 顺序类型
        /// </summary>
        public EventHandleSequenceType SequenceType { get; set; }
    
    }

    /// <summary>
    /// 事件集合
    /// </summary>
    /// <typeparam name="TEntityType"></typeparam>
    public class EventHandleCollection<TEntityType> where TEntityType : BaseEntity
    {
        /// <summary>
        /// 对象集合
        /// </summary>
        protected  IDictionary<EventHandleSequenceType, IDictionary<string, IList<EventHandle<TEntityType>>>> EventHandles { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="handles"></param>
        public EventHandleCollection(IList<EventHandle<TEntityType>> handles)
        {
            if(handles==null)
                return;
            EventHandles = EventHandles ??
                         new Dictionary
                             <EventHandleSequenceType, IDictionary<string, IList<EventHandle<TEntityType>>>>();
            foreach (var handle in handles)
            {
                if(!EventHandles.ContainsKey(handle.SequenceType))
                    EventHandles.Add(handle.SequenceType,new Dictionary<string, IList<EventHandle<TEntityType>>>());
                if(!EventHandles[handle.SequenceType].ContainsKey(handle.EventName))
                    EventHandles[handle.SequenceType].Add(handle.EventName,new List<EventHandle<TEntityType>>());
                EventHandles[handle.SequenceType][handle.EventName].Add(handle);
            }
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <param name="sequenceType"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public virtual IList<EventHandle<TEntityType>> GetHandles(EventHandleSequenceType sequenceType,string eventName)
        {
            
            if (string.IsNullOrEmpty(eventName) || EventHandles == null)
                return null;
            if (!EventHandles.ContainsKey(sequenceType) || !EventHandles[sequenceType].ContainsKey(eventName))
                return null;
            return EventHandles[sequenceType][eventName];
        }

    }
    public class RealizeApplicationService<TEntityType> : ApplicationService where TEntityType : BaseEntity
    {
        /// <summary>
        /// 事件对象
        /// </summary>
        protected virtual EventHandleCollection<TEntityType> EventHandles { get; set; }


        #region 重写方法
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T GetEntity<T>(long id)
        {
            return GetEntity(id) as T;
        }
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save<T>(IList<T> infos)
        {
            return Save(infos as IList<TEntityType>);
        }
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Save<T>(T info)
        {
            return Save(info as TEntityType);
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool HandleException<T>(Exception ex, T info)
        {
            return HandleException(ex, info as TEntityType);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<T> GetEntities<T>(QueryInfo query)
        {
            return GetEntities(query) as IList<T>;
        }
        /// <summary>
        /// 得到验证成功的事务处理对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override IList<IUnitofwork> Handle<T>(IList<T> infos)
        {
            return Handle(infos as IList<TEntityType>);
 
        }
 
        #endregion

        #region 接口的实现
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<TEntityType> GetEntities(QueryInfo query)
        {
            return base.GetEntities<TEntityType>(query);
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Save(TEntityType info)
        {
            var infos=new  List<TEntityType>();
            infos.Add(info);
            ExecutEvent(null,EventHandleSequenceType.BeforeSave, info);
            var unitofworks = Handle(infos);
            var rev = false;
            if (unitofworks != null)
            {
                ExecutEvent(unitofworks,EventHandleSequenceType.BeforeCommit, info);
                rev = Commit(unitofworks);
            }
            ExecutEvent(null,EventHandleSequenceType.AfterCommit, info);
            return rev;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual bool Save(IList<TEntityType> infos)
        {
            ExecutEvent(null,EventHandleSequenceType.BeforeSave, infos);
            var unitofworks = Handle(infos);
            var rev = false;
            if (unitofworks != null)
            {
                ExecutEvent(unitofworks,EventHandleSequenceType.BeforeCommit, infos);
                rev= Commit(unitofworks);
            }
            ExecutEvent(null,EventHandleSequenceType.AfterCommit, infos);
            return rev;
        }
      
        /// <summary>
        /// 根据ID得到对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntityType GetEntity(long id) 
        {
            return base.GetEntity<TEntityType>(id);
        }

       
        #endregion

        #region 方法

        /// <summary>
        /// 得到验证成功的事务处理对象
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> Handle(IList<TEntityType> infos)
        {
            return base.Handle(infos);
        }


        #endregion

        #region 执行事件
        /// <summary>
        /// 执行事件
        /// </summary>
        protected virtual void ExecutEvent(IList<IUnitofwork> unitofworks, EventHandleSequenceType sequenceType,IList<TEntityType> infos)
        {
            if(EventHandles==null ||　infos==null)
                return;
            foreach (var info in infos)
            {
                ExecutEvent(unitofworks,sequenceType, info);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        protected virtual void ExecutEvent(IList<IUnitofwork> unitofworks, EventHandleSequenceType sequenceType, TEntityType info)
        {
            if (EventHandles == null || info == null)
                return;
            DomainService.SetItemLoaders(info);
            var handles = EventHandles.GetHandles(sequenceType, info.EventName);
            if (handles==null)
                return;
            foreach (var eventHandle in handles)
            {
                var args=new EventHandleArgs<TEntityType>
                {
                    Entity=info,
                    Unitofworks=unitofworks,
                    Sender= eventHandle
                };
                if (eventHandle.IsAsynchronization)
                    eventHandle.Handle.BeginInvoke(args, null, null);
                else
                    eventHandle.Handle(args);
            }
        }
        #endregion


        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool HandleException(Exception ex, TEntityType info)
        {
            return base.HandleException(ex, info);
        }
    }
}
