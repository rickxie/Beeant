using System;
using Beeant.Domain.Entities.Log;
using Winner;
using Winner.Persistence;

namespace Beeant.Repository.Services
{
    public static class LogHelper
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        public static void AddEcho(EchoEntity entity)
        {
            entity.SaveType=SaveType.Add;
            Action<EchoEntity> action = BeginAddEcho;
            action.BeginInvoke(entity, null, null);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="entity"></param>
        private static void BeginAddEcho(EchoEntity entity)
        {
            Creator.Get<IContext>().Set(entity, entity, entity.SaveSequence);
            entity.SaveType = entity.SaveType;
            var unitofworks= Creator.Get<IContext>().Save();
            Creator.Get<IContext>().Commit(unitofworks);
        }
    }
}
