using Dependent;
using Beeant.Application.Services.Authority;
using Beeant.Domain.Entities;
using Winner.Persistence;
using Winner.Persistence.Compiler.Common;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class OwnerSavePageBase<T> : DatumPageBase<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public  virtual string OwnerName { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        public virtual string OwnerPropertyName { get; set; } = "Code";
        
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOwnerSubmitCode()
        {
            var args = Ioc.Resolve<IAuthorityEngineApplicationService>().GetEngin();
            if (args == null)
                return "";
            var owner = args.GetOnwerHandle(Identity.Id, OwnerName);
            if (owner == null)
                return "";
            return owner.SubmitCode;
        }
        /// <summary>
        /// 重写填充
        /// </summary>
        /// <returns></returns>
        protected override T FillEntity()
        {
            var info = base.FillEntity();
            if (SaveType == SaveType.Add)
            {
                info.SetProperty(OwnerPropertyName, GetOwnerSubmitCode());
            }
            return info;
        }
    }
}
