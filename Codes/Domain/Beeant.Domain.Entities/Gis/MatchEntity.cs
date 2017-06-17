using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Gis
{
    [Serializable]
    public class MatchEntity
    {
        /// <summary>
        /// 区域 
        /// </summary>
        public IList<AreaEntity> Areas { get; set; }
     
        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
