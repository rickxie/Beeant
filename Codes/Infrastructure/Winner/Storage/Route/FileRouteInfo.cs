using System;

namespace Winner.Storage.Route
{
    [Serializable]
    public class FileRouteInfo
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        private int _step = 10*24*60;
        /// <summary>
        /// 步长
        /// </summary>
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 全路径
        /// </summary>
        public string GetFullPath()
        {
            var dt = DateTime.Now;
            var timeSpan = dt - StartTime;
            var modValue = timeSpan.TotalMinutes%Step;
            return string.Format("{0}{1}/", Path, dt.AddMinutes(0 - modValue).ToString("yyyy/MM/dd/HH/mm"));
        
        }

        public FileRouteInfo()
        {
            StartTime = DateTime.Now;
        }
    }
}
