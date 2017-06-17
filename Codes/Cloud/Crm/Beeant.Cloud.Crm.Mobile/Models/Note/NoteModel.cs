using System;
using Component.Extension;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Cloud.Crm.Mobile.Models.Note
{
    public class NoteModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///下次跟踪时间
        /// </summary>
        public DateTime? RemindNoteDate { get; set; }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public virtual NoteEntity CreateEntity()
        {
            var entity = new NoteEntity
            {
                Content = Content,
                Customer = new CustomerEntity { Id=CustomerId.Convert<long>()},
                SaveType = SaveType.Add
            };
             
            return entity;
        }
    }
}