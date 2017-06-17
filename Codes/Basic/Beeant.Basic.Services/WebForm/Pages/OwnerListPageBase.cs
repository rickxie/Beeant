using System.Text;
using Dependent;
using Beeant.Application.Services.Authority;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class OwnerListPageBase<T> : ListPageBase<T> where T : BaseEntity, new()
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
        /// 设置查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            var readCode = GetTeamReadCodes();
            if(string.IsNullOrWhiteSpace(readCode))
                return;
            var builder=new StringBuilder("(");
            var codes = readCode.Split(',');
            var i = 0;
            foreach (var code in codes)
            {
                query.SetParameter(string.Format("SysCodes{0}", i), code);
                builder.AppendFormat("@SysCodes{0}=={1}", i, OwnerPropertyName);
                if (i != codes.Length - 1)
                    builder.Append(" || ");
            }
            builder.Append(")");
            query.Where(builder.ToString());
            base.SetQueryWhere(query);
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTeamReadCodes()
        {
            var args=Ioc.Resolve<IAuthorityEngineApplicationService>().GetEngin();
            if (args == null)
                return "";
            var owner = args.GetOnwerHandle(Identity.Id, OwnerName);
            if (owner == null)
                return "";
            return owner.ReadCodes;
        }

       
    }
}
