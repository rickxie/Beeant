using System;

namespace Beeant.Domain.Entities
{
    [Serializable]
    public enum ChannelType
    {
        /// <summary>
        /// Admin
        /// </summary>
        Admin = 1,
        /// <summary>
        /// Mobile
        /// </summary>
        Mobile = 2,
        /// <summary>
        /// Website
        /// </summary>
        Website = 3,
        /// <summary>
        /// API
        /// </summary>
        Api=4,
        /// <summary>
        /// 
        /// </summary>
        Ios=5,
        /// <summary>
        /// 
        /// </summary>
        Android=6
    }

}
