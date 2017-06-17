namespace Beeant.Domain.Entities.Log
{
    public class RecyclerEntity : BaseEntity
    {
 
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
