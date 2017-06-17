using System;
using System.Collections.Generic;

namespace Beeant.Basic.Services.Mvc.Paging
{
    public class PagerModel
    {
        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualPath { get; set; }
        /// <summary>
        /// 首页地址
        /// </summary>
        public string FirstUrl { get; set; }
        /// <summary>
        /// 上一页地址
        /// </summary>
        public string PreviousUrl { get; set; }
        /// <summary>
        /// 数字连接地址
        /// </summary>
        public IDictionary<int, string> Links { get; set; }
        /// <summary>
        /// 下一页地址
        /// </summary>
        public string NextUrl { get; set; }
        /// <summary>
        /// 最后一页地址
        /// </summary>
        public string LastUrl { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 是否Ajax
        /// </summary>
        public bool IsAjax { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string AjaxFunction { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        private int _pageCount = -1;

        /// <summary>
        /// 页大小
        /// </summary>
        public virtual int PageCount
        {
            get
            {
                if (DataCount == 0)
                    return 0;
                if (_pageCount == -1)
                    _pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(DataCount)/PageSize));
                return _pageCount;
            }
            set { _pageCount = value; }
        }

        private int _pageSize = 10;
        /// <summary>
        /// 分页大小
        /// </summary>
        public virtual int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public int DataCount { get; set; }
    }
}
