using Winner.Persistence;

namespace Beeant.Domain.Entities.Wms
{
    public class ShiftEntity : BaseEntity<ShiftEntity>
    {
        /// <summary>
        /// 支架
        /// </summary>
        public ShelfEntity Shelf { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 重写
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Shelf");
            if(Shelf==null)
                return;
            Shelf.Count += Count;
            if (Shelf.SaveType == SaveType.None)
            {
                Shelf.SetProperty(it => it.Count);
                Shelf.SaveType=SaveType.Modify;
            }
        }
    }
}
