using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.UI.WebControls;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Extension;
using Component.Extension;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Compiler.Common;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class ChartPageBase<T> : ReportPageBase<T>
    {
        protected virtual bool IsShowSumHtml { get; } = false;
        public string SumHtml;
        public override bool IsImageRecover
        {
            get { return false; }
            set { base.IsImageRecover = value; }
        }

        /// <summary>
        /// 输出脚步
        /// </summary>
        public virtual string Option { get; set; }


        /// <summary>
        /// 重写排序
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryOrderby(QueryInfo query)
        {
            if (TypeRadioButtonList != null)
                query.OrderByExp = TypeRadioButtonList.SelectedValue;
        }

        /// <summary>
        /// 重写加载
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                LoadEntities();
            }
            catch (Exception ex)
            {
                this.ShowMessage("查询异常", string.Format("异常信息为：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 重写加载
        /// </summary>
        protected override void LoadEntities()
        {
            var xAxis = GetXAxis();
            var datas = new Dictionary<string, IList<decimal>>();
            var reuslt = new Dictionary<string, IDictionary<string,decimal>>();
            var infos = GetEntities();
            var temp = GetEntityDictionaries(infos);
            xAxis = xAxis ?? temp.Keys.ToList();
            LoadDatas(datas, temp, xAxis);
            LoadPieDatas(reuslt, infos, xAxis);
            if (infos != null && IsShowSumHtml)
                InitSumHtml(reuslt);
            BindOption(datas, reuslt, xAxis);
        }

        protected void InitSumHtml(IDictionary<string, IDictionary<string, decimal>> reuslt)
        {
            StringBuilder res = new StringBuilder();
            foreach (var sumKey in reuslt)
            {
                res.Append($"<span>{sumKey.Key}:</span><br>");
                res.Append("<table class=\"tb\" cellspacing=\"0\" border=\"1\" style=\"width:100%\">");
                res.Append("<tr>");
                foreach (var kv in sumKey.Value)
                {
                    res.Append($"<td class=\"font\" width=\"{Math.Round(1.0M/sumKey.Value.Count.Convert<decimal>(), 4) * 100}%\">{kv.Key}</td>");
                }
                res.Append("</tr>");
                res.Append("<tr class=\"out\">");
                foreach (var kv in sumKey.Value)
                {
                    res.Append($"<td class=\"left\">{kv.Value}</td>");
                }
                res.Append("</tr>");
                res.Append("</table>");
                res.Append("<br>");
            }
            SumHtml = res.ToString();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="reuslt"></param>
        /// <param name="infos"></param>
        /// <param name="xAxis"></param>
        protected virtual void LoadPieDatas(IDictionary<string, IDictionary<string, decimal>> reuslt, IList<T> infos,
            IList<string> xAxis)
        {
            foreach (ListItem selectItem in SelectList.Items)
            {
                if (!selectItem.Selected)
                    continue;
                reuslt.Add(selectItem.Text, new Dictionary<string, decimal>());
                foreach (var info in infos)
                {
                    var showValue = TypeRadioButtonList.SelectedItem.Attributes["ShowValue"] ?? TypeRadioButtonList.SelectedValue;
                    var xName = info.GetProperty(showValue).Convert<string>();
                    if(string.IsNullOrEmpty(xName))
                        continue;
                    if (reuslt[selectItem.Text].ContainsKey(xName))
                    {
                        reuslt[selectItem.Text][xName] += info.GetProperty(selectItem.Value).Convert<decimal>();
                    }
                    else
                    {
                        reuslt[selectItem.Text].Add(xName, info.GetProperty(selectItem.Value).Convert<decimal>());
                    }
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="temp"></param>
        /// <param name="xAxis"></param>
        protected virtual void LoadDatas(IDictionary<string, IList<decimal>> datas, IDictionary<string, T> temp, IList<string> xAxis)
        {
            foreach (var name in xAxis)
            {
                if (!datas.ContainsKey(name))
                {
                    var values=new List<decimal>();
                    var info = temp.ContainsKey(name) ? temp[name] : default(T);
                    foreach (ListItem selectItem in SelectList.Items)
                    {
                        if (!selectItem.Selected)
                            continue;
                        var value = info == null ? 0 : info.GetProperty(selectItem.Value).Convert<decimal>();
                        values.Add(value);
                    }
                    datas.Add(name, values);
                }
            }
           
        }
        /// <summary>
        /// 得到字典
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IDictionary<string,T> GetEntityDictionaries(IList<T> infos)
        {
            var temp = new Dictionary<string, T>();
            foreach (var info in infos)
            {
                var showValue = TypeRadioButtonList.SelectedItem.Attributes["ShowValue"] ?? TypeRadioButtonList.SelectedValue;
                var xName = info.GetProperty(showValue).Convert<string>();
                if (!string.IsNullOrEmpty(xName) && !temp.ContainsKey(xName))
                    temp.Add(xName, info);
            }
            return temp;
        }
        /// <summary>
        /// 得到横坐标名称
        /// </summary>
        /// <returns></returns>
        protected virtual IList<string> GetXAxis()
        {
            return null;
        }
       
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <returns></returns>
        protected override IList<T> GetEntities()
        {
            QueryInfo query = new QueryInfo();
            SetQuery(query);
            var infos = Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
            return infos;
        }

       

        #region 输出脚步

        /// <summary>
        /// 重写绑定
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="result"></param>
        /// <param name="xAxis"></param>
        protected virtual void BindOption(IDictionary<string, IList<decimal>> datas, IDictionary<string, IDictionary<string, decimal>> result,IList<string> xAxis)
        {
            var builder = new StringBuilder("option = {");
            AppendToolTip(builder);
            AppendToolBox(builder);
            AppendCalculable(builder);
            AppendLegend(builder);
            AppendxAxis(builder, xAxis);
            AppendyAxis(builder, xAxis);
            AppendDataZoom(builder);
            AppendGrid(builder);
            AppendSeries(builder, datas, result);
            builder.Append("};");
            Option = builder.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendToolTip(StringBuilder builder)
        {
            builder.Append("tooltip: {trigger: 'axis' }");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendToolBox(StringBuilder builder)
        {
            builder.Append(",toolbox: { show: true, y: 'top', feature: { mark: { show: true },  dataView: { show: true, readOnly: false }, magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },restore: { show: true },saveAsImage: { show: true }  }}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendCalculable(StringBuilder builder)
        {
            builder.Append(",calculable: true");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendLegend(StringBuilder builder)
        {
            builder.Append(",legend: { data:[");
            var infos = new List<string>();
            foreach (ListItem selectItem in SelectList.Items)
            {
                if (!selectItem.Selected)
                    continue;
                infos.Add(string.Format("'{0}'",  selectItem.Text));
            }
            builder.Append(string.Join(",", infos.ToArray()));
            builder.Append("]}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="infos"></param>
        protected virtual void AppendxAxis(StringBuilder builder, IList<string>  infos)
        {
            var count = SelectList.Items.Cast<ListItem>().Count(selectItem =>selectItem.Attributes["ChartType"] == "none");
            if (count == SelectList.Items.Count)
                return;
            var type = string.IsNullOrEmpty(TypeRadioButtonList.Attributes["xAxisType"])
                ? "category"
                : TypeRadioButtonList.Attributes["xAxisType"];
            builder.Append(",xAxis: [{type: '");
            builder.AppendFormat("{0}'", type);
            if (TypeRadioButtonList.Attributes["xAxis"]==null)
            {
                builder.Append(",splitLine: { show: false }");
            }
            else
            {
                builder.Append(TypeRadioButtonList.Attributes["xAxis"]);
            }
            if (type == "category")
            {
                builder.Append(",data: [");
                if (TypeRadioButtonList.Attributes["IsShowCategory"] == "false")
                {
                    builder.AppendFormat("''");
                }
                else
                {
                    foreach (var info in infos)
                    {
                        builder.AppendFormat("'{0}',", info);
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("]");
            }
            builder.Append("}]"); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="infos"></param>
        protected virtual void AppendyAxis(StringBuilder builder, IList<string> infos)
        {
            var count = SelectList.Items.Cast<ListItem>().Count(selectItem => selectItem.Attributes["ChartType"] == "none");
            if(count==SelectList.Items.Count)
                return;
            var type = string.IsNullOrEmpty(TypeRadioButtonList.Attributes["yAxisType"])
                ? "value"
                : TypeRadioButtonList.Attributes["yAxisType"];
            builder.Append(",yAxis: [{type: '");
            builder.AppendFormat("{0}'", type);
            if (TypeRadioButtonList.Attributes["yAxis"]==null)
            {
                builder.Append(",position:'right'");
            }
            else
            {
                builder.Append(TypeRadioButtonList.Attributes["yAxis"]);
            }
            if (type == "category")
            {
                builder.Append(",data: [");
                if (TypeRadioButtonList.Attributes["IsShowCategory"] == "false")
                {
                    builder.AppendFormat("''");
                }
                else
                {
                    foreach (var info in infos)
                    {
                        builder.AppendFormat("'{0}',", info);
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("]");
            }
            builder.Append("}]");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendDataZoom(StringBuilder builder)
        {
            builder.Append(",dataZoom:{");
            if (TypeRadioButtonList.Attributes["DataZoom"]==null)
            {
                builder.Append("show: true, start: 0");
            }
            else
            {
                builder.Append(TypeRadioButtonList.Attributes["DataZoom"]);
            }
            builder.Append("}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AppendGrid(StringBuilder builder)
        {
            builder.Append(",grid:{y2: 80}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="datas"></param>
        /// <param name="result"></param>
        protected virtual void AppendSeries(StringBuilder builder, IDictionary<string, IList<decimal>> datas, IDictionary<string, IDictionary<string, decimal>> result)
        {
            builder.Append(",series: [");
            AppendDatas(builder, datas);
            AppendPies(builder, result);
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
        }

        /// <summary>
        /// 图形
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="datas"></param>
        /// <param></param>
        /// <param></param>
        protected virtual void AppendDatas(StringBuilder builder, IDictionary<string, IList<decimal>> datas)
        {
            var count = SelectList.Items.Cast<ListItem>().Count(selectItem => selectItem.Attributes["ChartType"] == "none");
            if (count == SelectList.Items.Count)
                return;
            var i = 0;
            foreach (ListItem selectItem in SelectList.Items)
            {
                if (!selectItem.Selected)
                    continue;
                var type = !string.IsNullOrEmpty(TypeRadioButtonList.SelectedItem.Attributes["ChartType"]) ? TypeRadioButtonList.SelectedItem.Attributes["ChartType"] : "bar";
                type = !string.IsNullOrEmpty(selectItem.Attributes["ChartType"]) && selectItem.Attributes["ChartType"] !="none"? selectItem.Attributes["ChartType"] : type;
                builder.Append("{name:");
                builder.AppendFormat(" '{0}',type: '{1}',data:[", selectItem.Text, type);
                if (selectItem.Attributes["ChartType"] != "none")
                {
                    foreach (var data in datas.Values)
                    {
                        builder.AppendFormat("{0},", data[i]);
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
                builder.Append("]},");
                i++;
            }
        }

        /// <summary>
        /// 拼接饼图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="result"></param>
        /// <param></param>
        protected virtual void AppendPies(StringBuilder builder, IDictionary<string, IDictionary<string, decimal>> result)
        {
            var count = SelectList.Items.Cast<ListItem>().Count(selectItem => selectItem.Selected && selectItem.Attributes["IsPie"] == "true");
            var index = 0;
            foreach (ListItem selectItem in SelectList.Items)
            {
                if (!selectItem.Selected)
                    continue;
                if (selectItem.Attributes["IsPie"] == "true")
                {
                    var center = string.Format("[{0}, 130]", index*160 + 320);
                    if (!string.IsNullOrEmpty(selectItem.Attributes["PieCenter"]) && count>0)
                    {
                        var centers = selectItem.Attributes["PieCenter"].Split('|');
                        if (centers.Length >= count && !string.IsNullOrEmpty(centers[count - 1]))
                        {
                            center = centers[count - 1];
                        }
                    }
                    var radius = "[0, 50]";
                    if (!string.IsNullOrEmpty(selectItem.Attributes["PieCenter"]) && count > 0)
                    {
                        var radiuses = selectItem.Attributes["PieRadius"].Split('|');
                        if (radiuses.Length >= count && !string.IsNullOrEmpty(radiuses[count - 1]))
                        {
                            radius = radiuses[count - 1];
                        }
                    }
                    builder.Append("{name:");
                    builder.AppendFormat(" '{0}',type: 'pie',", selectItem.Text);
                    builder.Append("tooltip:{ trigger:'item',formatter: '{a} <br/>{b} : {c} ({d}%)'},");
                    builder.AppendFormat(" center:{0} ,radius:{1} ,", center,radius);
                    builder.Append("itemStyle: { normal: {labelLine: {  length: 20 }}},data:[");
                    foreach (var data in result[selectItem.Text])
                    {
                        builder.Append("{");
                        builder.AppendFormat("name:'{0}', value:{1}", data.Key, data.Value);
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("]},");
                    index++;
                }
            }
         
        }

      

        #endregion

    }
}
