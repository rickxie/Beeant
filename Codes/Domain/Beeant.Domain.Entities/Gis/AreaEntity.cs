using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Gis
{
    [Serializable]
    public class AreaEntity : BaseEntity<AreaEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 边界
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 原始
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// 图形路径
        /// </summary>
        public GraphicsPath GraphicsPath { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public Region Region { get; set; }
        private static readonly object Locker = new object();
        /// <summary>
        /// 检查点是否在区域内
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public virtual bool CheckPoint(double lng, double lat)
        {
            if (string.IsNullOrEmpty(Path))
                return false;
            if (GraphicsPath == null)
            {
                lock (Locker)
                {
                    if (GraphicsPath == null)
                    {
                        GraphicsPath = new GraphicsPath();
                        GraphicsPath.Reset();
                        var points = new List<Point>();
                        var arrs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, double>>>(Path);
                        foreach (var arr in arrs)
                        {
                            var point = new Point(ParsePoint(arr["lng"]), ParsePoint(arr["lat"]));
                            points.Add(point);
                        }
                        GraphicsPath.AddPolygon(points.ToArray());
                        Region = new Region();
                        Region.MakeEmpty();
                        Region.Union(GraphicsPath);
                    }
                }
            }
            var inputponint = new Point(ParsePoint(lng), ParsePoint(lat));
            return Region.IsVisible(inputponint);
        }


        /// <summary>
        /// 转换点
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual int ParsePoint(double value)
        {
            value = value*1000000;
            return (int)Math.Round(value);
        }
        /// <summary>
        /// 设置更新原始
        /// </summary>
        /// <returns></returns>
        public virtual void Import()
        {
            var dis = new Dictionary<string, object>
            {
                {"Id", Id},
                {"Name", Name},
                {"City", City},
                {"Type", Type},
                {"Color", Color},
                {"Tag", Tag},
                {"Path", Path},
                {"Value", Value},
                {"IsUsed", IsUsed},
                {"SaveType",SaveType }
            };
            Origin = dis.SerializeJson();
        }
        /// <summary>
        /// 得到原始
        /// </summary>
        /// <returns></returns>
        public virtual AreaEntity GetOrigin()
        {
            return Origin.DeserializeJson<AreaEntity>();
        }
        /// <summary>
        /// 发布
        /// </summary>
        public virtual void Publish()
        {
            var data= Origin.DeserializeJson<AreaEntity>();
            if(data==null)
                return;
            Id = data.Id;
            Name = data.Name;
            City = data.City;
            Type= data.Type;
            Color = data.Color;
            Tag = data.Tag;
            Path = data.Path;
            Value = data.Value;
            IsUsed = data.IsUsed;
            SaveType = data.SaveType;
            if (SaveType != SaveType.Remove)
            {
                SaveType = SaveType.Modify;
                SetProperty(it => it.Path)
                    .SetProperty(it => it.Tag)
                    .SetProperty(it => it.Color)
                    .SetProperty(it => it.Value)
                    .SetProperty(it => it.IsUsed);
            }
        
        }
      
    }
}
