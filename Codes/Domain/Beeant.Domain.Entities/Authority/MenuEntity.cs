using System;
using System.Collections.Generic;
namespace Beeant.Domain.Entities.Authority
{
   
    [Serializable]
    public class MenuEntity : BaseEntity<MenuEntity>
    {

        /// <summary>
        ///父菜单
        /// </summary>
        public MenuEntity Parent { get; set; }


        /// <summary>
        ///名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        public int Sequence { get; set; }


        /// <summary>
        ///是否新窗口打开
        /// </summary>
        public bool IsBlank { get; set; }

        /// <summary>
        ///是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        ///URL 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///菜单类型
        /// </summary>
        public SubsystemEntity Subsystem{ get; set; }

        /// <summary>
        ///子类
        /// </summary>
        public IList<MenuEntity> Children { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        public IList<AbilityEntity> Abilities { get; set; }
       
    }
 
}
