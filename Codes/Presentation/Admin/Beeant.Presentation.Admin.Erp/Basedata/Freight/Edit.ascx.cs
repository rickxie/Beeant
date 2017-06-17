using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Basedata.Freight
{
    public partial class Edit : System.Web.UI.UserControl
    {
        public SaveType SaveType { get; set; }

        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var builder = new StringBuilder();
            builder.AppendFormat("InitFreight();");
            Page.ExecuteScript(builder.ToString());
        }

        #region 得到存储
        /// <summary>
        /// 得到存储运价
        /// </summary>
        /// <returns></returns>
        public virtual IList<CarryEntity> GetSaveCarries()
        {
            var infos = new List<CarryEntity>();
            var idArrays = Request.Form["CarryId"].Split(',');
            var defaultCounts = Request.Form["CarryDefaultCount"].Split(',');
            var defaultPrices = Request.Form["CarryDefaultPrice"].Split(',');
            var continueCounts = Request.Form["CarryContinueCount"].Split(',');
            var continuePrices = Request.Form["CarryContinuePrice"].Split(',');
            var names = Request.Form["CarryName"].Split(',');
            var region = Request.Form["CarryRegion"].Split(',');
            for (var i = 0; i < idArrays.Length; i++)
            {
                var info = new CarryEntity
                {
                    Id = idArrays.Length > i ? idArrays[i].Convert<long>() : 0,
                    DefaultCount = defaultCounts[i].Convert<int>(),
                    DefaultPrice = defaultPrices[i].Convert<decimal>(),
                    ContinueCount = continueCounts[i].Convert<int>(),
                    ContinuePrice = continuePrices[i].Convert<decimal>(),
                    Name = names[i],
                    Region = region[i].Replace("-", ",")
                };
                infos.Add(info);
            }
            return infos;
        }
 
        #endregion

        #region 得到运价
        /// <summary>
        /// 得到运价Html
        /// </summary>
        /// <returns></returns>
        public virtual string GetCarryEntities()
        {
            var infos = GetCarries();
            var json = new List<IDictionary<string, object>>();
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    json.Add(new Dictionary<string, object>
                    {
                        {"Id", info.Id},
                        {"DefaultCount", info.DefaultCount},
                        {"DefaultPrice", info.DefaultPrice},
                        {"ContinueCount", info.ContinueCount},
                        {"ContinuePrice", info.ContinuePrice},
                        {"Type", info.Name},
                        {"Region", info.Region}
                    });
                }
            }
            return json.SerializeJson();
        }


        /// <summary>
        /// 得到运价
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CarryEntity> GetCarries()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"]))
                return null;
            var query = new QueryInfo();
            query.Query<CarryEntity>().Where(it => it.Freight.Id == Request.QueryString["Id"].Convert<long>())
                 .Select(
                     it =>
                     new object[]
                         {
                             it.Id, it.DefaultCount, it.DefaultPrice, it.ContinueCount, it.ContinuePrice, it.Name,it.Region
                             
                         });
            return Ioc.Resolve<IApplicationService, CarryEntity>().GetEntities<CarryEntity>(query);
            
        }


        #endregion

        #region 得到区域
        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        public virtual string GetDistrictHtml()
        {
            var builder = new StringBuilder();
            var infos = GetDistricts();
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    builder.AppendFormat("<div class=\"province\">");
                    builder.AppendFormat("<input id=\"{0}\" value=\"{0}\" name=\"Province\" type=\"checkbox\" AllCheckName=\"{0}\"/>",info.Id);
                    builder.AppendFormat("<a href=\"javascript:void(0);\"><label name=\"Province\">{0}</label>", info.Name);
                    builder.AppendFormat("<span name=\"spanCount\" class=\"count\"></span></a>");
                    if (info.Children != null)
                    {
                        builder.AppendFormat("<div class=\"city\" style=\"display: none;\">");
                        info.Children = info.Children.OrderBy(it => it.Sequence).ToList();
                        foreach (var child in info.Children)
                        {
                            builder.AppendFormat("<span><input id=\"{0}\" value=\"{0}\" name=\"City\" type=\"checkbox\" SubCheckName=\"{1}\"/>",child.Id,info.Id);
                            builder.AppendFormat("<label for=\"{0}\" name=\"City\">{1}</label></span>",child.Id,child.Name);
                        }
                        builder.AppendFormat("</div>");
                    }
                    builder.AppendFormat("</div>");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        protected virtual IList<DistrictEntity> GetDistricts()
        {
            var query = new QueryInfo();
            query.Query<DistrictEntity>().Where(it => it.Parent.Id == 0).OrderBy(it => it.Sequence)
                 .Select(
                     it =>
                     new object[] {it.Id, it.Name, it.Children.Select(s => new object[] {s.Id, s.Name, s.Sequence})});
            return Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntities<DistrictEntity>(query);
        }
        #endregion
    }
}