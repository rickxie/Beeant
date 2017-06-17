using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm.Mobile.Models.Note
{
    public class NoteListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public IList<NoteEntity> Notes { get; set; } 
    }
}