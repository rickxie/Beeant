using Beeant.Domain.Entities.Api;

namespace Beeant.Application.Services.Api
{
    public interface IApiEngineApplicationService
    {
        /// <summary>
        /// 得到API
        /// </summary>
        /// <returns></returns>
        ApiEnginEntity GetEngin();

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        VerificationEntity Verify(ApiArgsEntity args);
    }
}
