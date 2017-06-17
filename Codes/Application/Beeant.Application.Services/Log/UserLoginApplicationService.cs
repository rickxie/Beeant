using System;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Services;
using Winner.Persistence;

namespace Beeant.Application.Services.Log
{
    public class UserLoginApplicationService : IJobApplicationService
    {
      
        public IRepository Repository { get; set; }

        public virtual bool Execute(object[] args)
        {
            var info = new LoginEntity
                {
                    WhereExp = "InsertTime<@InsertTime",
                    SaveType = SaveType.Remove
                };
            info.SetParameter("InsertTime", DateTime.Now.AddDays(0 - Convert.ToInt32(args[0])));
            return Winner.Creator.Get<IContext>().Commit(Repository.Save(info));
        }


    }
}
