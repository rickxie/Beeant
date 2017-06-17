using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm
{
    public class CrmAuthorizeBaseController : BaseController
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual long CrmId
        {
            get
            {
                long crmId = ViewBag.CrmId??0;
                return crmId;
            }
        }
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual long CrmAccountId
        {
            get
            {
                long crmAccountId = ViewBag.CrmAccountId??0;
                return crmAccountId;
            }
        }
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual StaffEntity Staff
        {
            get
            {
                return ViewBag.Staff;
            }
        }
       
        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual bool IsMainAccount
        {
            get
            {
                bool isMainAccount = ViewBag.IsMainAccount??false;
                return isMainAccount;
            }
        }
   
    }
}
