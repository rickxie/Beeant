using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm.Mobile.Models.Staff
{
    public class StaffListModel:PagerModel
    {
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 员工
        /// </summary>
        public IList<StaffEntity> Staffs { get; set; } 
    }
}
