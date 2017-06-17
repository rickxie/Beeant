using System;
using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Services.Utility;
using Winner.Persistence;

namespace Beeant.Application.Services.Member
{
    /// <summary>
    /// 会员商品浏览记录应用程序服务类。
    /// </summary>
    /// <remarks>
    /// <para>孙涛</para>
    /// <para>2015/8/14</para>
    /// </remarks>
    public class BrowseApplicationService : RealizeApplicationService<BrowseEntity>, IBrowseApplicationService, IJobApplicationService
    {
        #region 字段

        private static bool _isOpenQueue;

        #endregion

        #region 只读属性

        private const string QueueName = "GoodsBrowseQueue";

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
        /// <para>2015/8/14</para>
        /// </remarks>
        public virtual IQueueRepository QueueRepository { get; set; }

        #endregion

        #region 实例方法

        /// <summary>
        /// 确保打开队列。
        /// </summary>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/14</para>
        /// </remarks>
        private void EnsureOpenQueue()
        {
            if (!_isOpenQueue)
            {
                QueueRepository.Open(QueueName, int.MaxValue);
                _isOpenQueue = true;
            }
        }

        #endregion

        #region IBrowseApplicationService 实现

        /// <summary>
        /// 添加商品浏览记录。
        /// </summary>
        /// <param name="browseEntity">浏览信息对象。</param>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/14</para>
        /// </remarks>
        public virtual void Push(BrowseEntity browseEntity)
        {
            EnsureOpenQueue();
            QueueRepository.Push(QueueName, browseEntity);
        }


        #endregion

        #region IJobApplicationService 实现

        /// <summary>
        /// 执行事件。
        /// </summary>
        /// <param name="args">参数数组。</param>
        /// <returns>执行是否成功。</returns>
        /// <remarks>
        /// <para>孙涛</para>
        /// <para>2015/8/14</para>
        /// </remarks>
        public virtual bool Execute(object[] args)
        {
            try
            {
                int arg = (args == null || args.Length == 0) ? 500 : args[0].Convert<int>();
                int count = arg > 0 ? arg : 500;
                EnsureOpenQueue();
                IList<BrowseEntity> infos = new List<BrowseEntity>();
                while (count > 0)
                {
                    var info = QueueRepository.Pop<BrowseEntity>(QueueName);
                    if (info == null)
                        break;
                    info.SaveType = SaveType.Add;
                    info.IsBulkCopy = true;
                    count--;
                }
              return  Save(infos);
            }
            catch(Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
                return false;
            }
        }

        #endregion
    }
}
