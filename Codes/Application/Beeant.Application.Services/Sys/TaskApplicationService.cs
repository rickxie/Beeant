using System;
using System.Globalization;
using Beeant.Domain.Entities.Sys;
using System.Linq;
namespace Beeant.Application.Services.Sys
{
    public class TaskApplicationService :RealizeApplicationService<TaskEntity>, ITaskApplicationService
    {
        public virtual bool Execute(TaskEntity info, IJobApplicationService eventInstance)
        {
            if (info == null) return false;
            if (!info.IsStart) return false;
            if (string.IsNullOrEmpty(info.Weeks) || !info.Weeks.Contains(((int)DateTime.Now.DayOfWeek).ToString(CultureInfo.InvariantCulture)))
                return false;
            if (info.MonthsArray == null || info.MonthsArray.Count(it => it.Equals((DateTime.Now.Day).ToString(CultureInfo.InvariantCulture))) == 0)
                return false;
            if ( info.Recycle != 0&& (DateTime.Now - info.BeginTime).TotalSeconds % info.Recycle == 0)
                return false;
            var rev = eventInstance.Execute(info.ArgsArray);
            return rev;
        }



    }
}
