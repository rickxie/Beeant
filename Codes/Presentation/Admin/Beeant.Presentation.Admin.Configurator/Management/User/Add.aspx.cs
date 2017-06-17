using System;
using System.Collections.Generic;
using System.Text;
using Beeant.Domain.Entities.Authority;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Workflow;
using Component.Extension;
using Dependent;
using Winner.Filter;

namespace Beeant.Presentation.Admin.Configurator.Management.User
{
    public partial class Add : Basic.Services.WebForm.Pages.AddPageBase<UserEntity>
    {
  
       
        protected override void SetValidScriptString()
        {
            ValidScriptString =this.SetEntity(SaveButton, new Dictionary<Type, string> { { typeof(UserEntity),"ValidateName1" } , { typeof(AccountEntity), "ValidateName" }  }, ValidationType.Add);
            var script = new StringBuilder();
            script.Append(ValidScriptString);
            script.Append("validator.AddValidateInfo({");
            script.AppendFormat("Control:document.getElementById('{0}'),Message:'两次输入密码不一致',ValidateEvent:'blur'", txtSurePassword.ClientID);
            script.Append("}).Handles.push({Function:function(){");
            script.AppendFormat("return document.getElementById('{0}').value==document.getElementById('{1}').value;", txtSurePassword.ClientID, txtPassword.ClientID);
            script.Append("},Message:'两次输入密码不一致'});");
            ValidScriptString = script.ToString();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LoadRoles();
            LoadOwners();
            LoadGroups();
            LoadAuditors();
        }

        /// <summary>
        /// 加载角色
        /// </summary>
        protected virtual void LoadRoles()
        {
            var query = new QueryInfo();
            query.Query<RoleEntity>().Select(it => new object[] { it.Id, it.Name, it.InsertTime });
            var roleEntities = Ioc.Resolve<IApplicationService>().GetEntities<RoleEntity>(query);
            gvRole.DataSource = roleEntities;
            gvRole.DataBind();
        }
        /// <summary>
        /// 加载组
        /// </summary>
        protected virtual void LoadGroups()
        {
            var query = new QueryInfo();
            query.Query<GroupEntity>().Select(it => new object[] { it.Id, it.Name, it.InsertTime });
            var groupEntities = Ioc.Resolve<IApplicationService>().GetEntities<GroupEntity>(query);
            gvGroup.DataSource = groupEntities;
            gvGroup.DataBind();
        }
        /// <summary>
        /// 加载组
        /// </summary>
        protected virtual void LoadOwners()
        {
            var query = new QueryInfo();
            query.Query<OwnerEntity>().Select(it => new object[] { it.Id, it.Name, it.InsertTime });
            var groupEntities = Ioc.Resolve<IApplicationService>().GetEntities<OwnerEntity>(query);
            gvOnwer.DataSource = groupEntities;
            gvOnwer.DataBind();
        }
        /// <summary>
        /// 加载组
        /// </summary>
        protected virtual void LoadAuditors()
        {
            var query = new QueryInfo();
            query.Query<AuditorEntity>().Select(it => new object[] { it.Id, it.Name, it.InsertTime });
            var groupEntities = Ioc.Resolve<IApplicationService>().GetEntities<AuditorEntity>(query);
            gvAuditor.DataSource = groupEntities;
            gvAuditor.DataBind();
        }

        protected override void Save()
        {
            if (!txtPassword.Value.Equals(txtSurePassword.Value))
            {
                this.ShowMessage("验证错误", "两次输入密码不一致");
                return;
            }
            base.Save();
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override UserEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                FillRoleAccounts(info);
                FillOwnerAccounts(info);
                FillAuditorAccounts(info);
                FillAuditorAccounts(info);
                if (info.Account != null)
                {
                    info.Account.SaveType= SaveType.Add;
                    info.Account.IsUsed = info.IsUsed;
                    info.Account.AccountIdentites = new List<AccountIdentityEntity>();
                    info.Account.AccountIdentites.Add(new AccountIdentityEntity
                    {
                        Account = info.Account,
                        Name = "Name",
                        Number = info.Account.Name,
                        SaveType = SaveType.Add
                    });
                }
            }
            
            return info;
        }
        /// <summary>
        /// 填充用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillRoleAccounts(UserEntity info)
        {
            info.RoleAccounts = new List<RoleAccountEntity>();
            foreach (GridViewRow gvr in gvRole.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var roleAccount = new RoleAccountEntity
                {
                    Role = new RoleEntity { Id = ckSelect.Value.Convert<long>() },
                    Account =info.Account,
                    SaveType = SaveType.Add,
                };
                info.RoleAccounts.Add(roleAccount);
            }
        }
        /// <summary>
        /// 填充用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillOwnerAccounts(UserEntity info)
        {
            info.OwnerAccounts = new List<OwnerAccountEntity>();
            foreach (GridViewRow gvr in gvRole.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var ownerAccount = new OwnerAccountEntity
                {
                    Owner = new OwnerEntity { Id = ckSelect.Value.Convert<long>() },
                    Account = info.Account,
                    SaveType = SaveType.Add,
                };
                info.OwnerAccounts.Add(ownerAccount);
            }
        }
        /// <summary>
        /// 填充用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillGroupAccounts(UserEntity info)
        {
            info.GroupAccounts = new List<GroupAccountEntity>();
            foreach (GridViewRow gvr in gvRole.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var groupAccount = new GroupAccountEntity
                {
                    Group = new GroupEntity { Id = ckSelect.Value.Convert<long>() },
                    Account = info.Account,
                    SaveType = SaveType.Add,
                };
                info.GroupAccounts.Add(groupAccount);
            }
        }
        /// <summary>
        /// 填充用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillAuditorAccounts(UserEntity info)
        {
            info.AuditorAccounts = new List<AuditorAccountEntity>();
            foreach (GridViewRow gvr in gvRole.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var auditorAccount = new AuditorAccountEntity
                {
                    Auditor = new AuditorEntity { Id = ckSelect.Value.Convert<long>() },
                    Account = info.Account,
                    SaveType = SaveType.Add,
                };
                info.AuditorAccounts.Add(auditorAccount);
            }
        }
        protected override void OnPreLoad(EventArgs e)
        {
            SaveButton = btnSave;
            base.OnPreLoad(e);
        }
    }
}