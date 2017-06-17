using System;
using System.Linq;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Home.Desktop.User
{
    public partial class Update : UpdatePageBase<UserEntity>
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
        public override long RequestId
        {
            get
            {
                return GetUserId();
            }
            set
            {
                base.RequestId = value;
            }
        }
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        protected override void OnPreLoad(EventArgs e)
        {
            SaveButton = btnSave;
            base.OnPreLoad(e);
        }
        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <returns></returns>
        protected virtual long GetUserId()
        {
            var query=new QueryBasicInfo<UserEntity>();
            var user=query.Query()
                .Where(it => it.Account.Id == Identity.Id)
                .Select(it => it.Id)
                .ToList<UserEntity>()?
                .FirstOrDefault();
            return user == null ? 0 : user.Id;
        }
    }
}