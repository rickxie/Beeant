using System;
using System.Collections.Generic;
using System.Text;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Dependent;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Configurator.Management.User
{
    public partial class Update : Basic.Services.WebForm.Pages.UpdatePageBase<UserEntity>
    {
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
        protected override void SetValidScriptString()
        {
            ValidScriptString = this.SetEntity(SaveButton, new Dictionary<Type, string> { { typeof(UserEntity), "ValidateName1" }, { typeof(AccountEntity), "ValidateName" } }, ValidationType.Modify);
           

        }
        protected override void OnPreLoad(EventArgs e)
        {
            SaveButton = btnSave;
            base.OnPreLoad(e);
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override UserEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null && info.Account != null)
            {
                var dataInfo = Ioc.Resolve<IApplicationService>().GetEntity<UserEntity>(info.Id);
                if (dataInfo != null && dataInfo.Account != null)
                {
                    info.Account.Id = dataInfo.Account.Id;
                }
                info.Account.IsUsed = info.IsUsed;
                info.Account.SaveType=SaveType.Modify;
                info.Account.SetProperty(it => it.Name)
                    .SetProperty(it => it.Mobile)
                    .SetProperty(it => it.Email)
                    .SetProperty(it => it.RealName)
                    .SetProperty(it => it.IsUsed);
            }
            return info;
        }

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <returns></returns>
        protected override UserEntity GetEntity()
        {
            var info= base.GetEntity();
            if (info != null && info.Account != null)
                info.Account = Ioc.Resolve<IApplicationService>().GetEntity<AccountEntity>(info.Account.Id);
            return info;
        }
    }
}