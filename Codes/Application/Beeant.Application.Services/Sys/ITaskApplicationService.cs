using Beeant.Domain.Entities.Sys;

namespace Beeant.Application.Services.Sys
{
    public interface ITaskApplicationService
    {
        bool Execute(TaskEntity info, IJobApplicationService eventInstance);
    }
}
