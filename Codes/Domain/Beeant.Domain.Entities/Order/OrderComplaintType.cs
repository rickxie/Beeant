using System;

namespace Beeant.Domain.Entities.Order
{


    [Serializable]
    public enum OrderComplaintType
    {
        /// <summary>
        /// 未回复
        /// </summary>
        None=1,
        /// <summary>
        /// 好评
        /// </summary>
        Good = 2,
        /// <summary>
        /// 一般
        /// </summary>
        General = 3,
        /// <summary>
        /// 差评
        /// </summary>
        Bad = 4
    
    }

}
