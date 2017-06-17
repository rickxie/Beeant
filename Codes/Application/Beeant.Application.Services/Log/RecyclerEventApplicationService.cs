using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Log
{
    public class RecyclerJobApplicationService : IJobApplicationService
    {
        public IRepository Repository { get; set; }
        public IFileRepository FileRepository { get; set; }
        public static bool IsExecute { get; set; }
        public virtual bool Execute(object[] args)
        {
            if (IsExecute)
                return true;
            IsExecute = true;
            try
            {
                while (true)
                {
                    var query = new QueryInfo();
                    query.SetPageSize(100).Query<RecyclerEntity>().Select(it => new object[] { it.Id, it.FileName });
                    IList<RecyclerEntity> infos = Repository.GetEntities<RecyclerEntity>(query);
                    if (infos == null || infos.Count == 0)
                        break;
                    foreach (var info in infos)
                    {
                        if (!string.IsNullOrEmpty(info.FileName))
                            FileRepository.Remove(info.FileName);
                        info.SaveType = SaveType.Remove;
                    }
                    Winner.Creator.Get<IContext>().Commit(Repository.Save(infos));
                }
            }
            catch (Exception ex) 
            {
                
              
            }
            IsExecute = false;
            return true;
        }


    }
}
