using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Search;
using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Search
{
    public class KeyQueueSaveEventApplicationService : KeyApplicationService, IKeyQueueSaveApplicationService, IJobApplicationService
    {
        #region 字段

        private bool IsOpenQueue = false;

        #endregion

        #region 只读属性

        private static readonly string QueueName = "SearchKeyQueue";

        #endregion

        #region 实例属性

        /// <summary>
        /// 获取或设置队列仓储。
        /// </summary>
        /// <value>
        /// 队列仓储。
        /// </value>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        public virtual IQueueRepository QueueRepository { get; set; }

        #endregion

        #region 实例方法

        /// <summary>
        /// 确保打开队列。
        /// </summary>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        private void EnsureOpenQueue()
        {
            if (!IsOpenQueue)
            {
                QueueRepository.Open(QueueName, int.MaxValue);
                IsOpenQueue = true;
            }
        }

        #endregion

        #region IKeyQueueSaveApplicationService 实现

        /// <summary>
        /// 添加搜索关键词记录。
        /// </summary>
        /// <param name="KeyEntity">搜索关键词信息。</param>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        public virtual void Add(KeyEntity keyEntity)
        {
            EnsureOpenQueue();
            QueueRepository.Push<KeyEntity>(QueueName, keyEntity);
        }

        #endregion

        #region IEventApplicationService 实现

        /// <summary>
        /// 执行事件。
        /// </summary>
        /// <param name="args">参数数组。</param>
        /// <returns>执行是否成功。</returns>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/17</para>
        /// </remarks>
        public virtual bool Execute(object[] args)
        {
            try
            {
                int arg = (args == null || args.Length == 0) ? 100 : Convert.ToInt32(args[0]);
                int count = arg > 0 ? arg : 100;
                EnsureOpenQueue();
                var infos = new List<KeyEntity>();

                KeyEntity info = null;
                while ((info = QueueRepository.Pop<KeyEntity>(QueueName)) != null)
                {
                    infos.Add(info);
                    if (infos.Count > count)
                    {
                        break;
                    }
                }

                var unitofworks = DomainService.Handle<KeyEntity>(infos);
                return Commit(unitofworks);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
