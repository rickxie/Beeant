using System;


namespace Beeant.Domain.Entities.Supplier
{

        [Serializable]
        public enum QualificationType
        {
            /// <summary>
            /// 厂家
            /// </summary>
            Factory = 1,
            /// <summary>
            /// 总代
            /// </summary>
            GeneralAgency = 2,
            /// <summary>
            /// 分代理
            /// </summary>
            SubAgent = 3,
            /// <summary>
            /// 经销商
            /// </summary>
            Agency = 4
        }
    
}
