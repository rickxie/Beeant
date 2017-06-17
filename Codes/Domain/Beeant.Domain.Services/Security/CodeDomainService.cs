using System;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Security;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Security
{
    public class CodeDomainService : RealizeDomainService<CodeEntity>, ICodeDomainService
    {
       

        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CodeEntity info)
        {
            var rev = ValidateExist(info);
            return rev;
        }



        /// <summary>
        /// 验证是否重复发送
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(CodeEntity info)
        {
           var json = Configuration.ConfigurationManager.GetSetting<string>("SecurityCode").DeserializeJson<dynamic>();
           int stepTime = json == null || json.SendStep==null ? 100 : json.SendStep;
            var query = new QueryInfo() { IsReturnCount = false };
            query.SetPageIndex(0)
                       .SetPageSize(1)
                       .Query<CodeEntity>()
                       .Where(
                           it =>
                           it.Name==info.Name && it.Type == info.Type && it.Tag == info.Tag &&
                           it.InsertTime >= DateTime.Now.AddSeconds(-stepTime));
            var infos = Repository.GetEntities<CodeEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("StepTimeError",stepTime);
                return false;
            }
            return true;
        }

        #endregion




        #endregion

        #region 验证码

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ValidateCode(string tag,string name,CodeType type,string value)
        {
            if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(value))
                return false;
            var query = new QueryInfo() {IsReturnCount=false};
            query.SetPageIndex(0)
                 .SetPageSize(1)
                 .Query<CodeEntity>()
                 .Where(
                     it =>
                     it.Tag==tag && it.Type == type && it.Name == name && it.Value == value &&
                     it.EffectiveTime >= DateTime.Now)
                 .OrderByDescending(it => it.InsertTime);
            var infos = Repository.GetEntities<CodeEntity>(query);
            if (infos != null && infos.Count > 0)
                return true;
            return false;
        }

        #endregion
    }
}
