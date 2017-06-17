using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;

using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Controls.Basedata
{
    public partial class DistrictDropDownList : System.Web.UI.UserControl
    {
        #region 使用端配置
        /// <summary>
        /// 省存储名称
        /// </summary>
        public virtual string ProvinceSaveName
        {
            get { return ddlProvince.Attributes["SaveName"]; }
            set { ddlProvince.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 省绑定名称
        /// </summary>
        public virtual string ProvinceBindName
        {
            get { return ddlProvince.Attributes["BindName"]; }
            set { ddlProvince.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 省省验证名称
        /// </summary>
        public virtual string ProvinceValidateName
        {
            get { return ddlProvince.Attributes["ValidateName"]; }
            set { ddlProvince.Attributes.Add("ValidateName", value); }
        }


        /// <summary>
        /// 市存储名称
        /// </summary>
        public virtual string CitySaveName
        {
            get { return ddlCity.Attributes["SaveName"]; }
            set { ddlCity.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 市绑定名称
        /// </summary>
        public virtual string CityBindName
        {
            get { return ddlCity.Attributes["BindName"]; }
            set { ddlCity.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 市验证名称
        /// </summary>
        public virtual string CityValidateName
        {
            get { return ddlCity.Attributes["ValidateName"]; }
            set { ddlCity.Attributes.Add("ValidateName", value); }
        }

        /// <summary>
        /// 镇存储名称
        /// </summary>
        public virtual string CountySaveName
        {
            get { return ddlCounty.Attributes["SaveName"]; }
            set { ddlCounty.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 镇绑定名称
        /// </summary>
        public virtual string CountyBindName
        {
            get { return ddlCounty.Attributes["BindName"]; }
            set { ddlCounty.Attributes.Add("BindName", value); }
        }


        /// <summary>
        /// 镇验证名称
        /// </summary>
        public virtual string CountyValidateName
        {
            get { return ddlCounty.Attributes["ValidateName"]; }
            set { ddlCounty.Attributes.Add("ValidateName", value); }
        }
        #endregion
       
        /// <summary>
        /// 省下拉框
        /// </summary>
        public DropDownList ProvinceDropDownList
        {
            get { return ddlProvince; }
        }
        
        /// <summary>
        /// 市下拉框
        /// </summary>
        public DropDownList CityDropDownList
        {
            get { return ddlCity; }
        }
        
        /// <summary>
        /// 镇下拉框
        /// </summary>
        public DropDownList CountyDropDownList
        {
            get { return ddlCounty; }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
            var proviceList = GetDistrictEntityByParent(0);
            foreach (var districtEntity in proviceList)
                ProvinceDropDownList.Items.Add(new ListItem(districtEntity.Name, districtEntity.Id.ToString()));
            InitCity();
            InitCounty();
        }

        /// <summary>
        /// 按值重置下拉框
        /// </summary>
        public virtual void ResetByValue(string val)
        {
            var query = new QueryInfo();
            query.Query<DistrictEntity>()
                 .Where(it => it.Id == val.Convert<int>()&&it.IsUsed)
                 .Select(it => new object[] {it, it.Parent});
            var district =
                Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntities<DistrictEntity>(query).FirstOrDefault();
            if (district == null) return;
            // ReSharper disable PossibleNullReferenceException
            if (district.Parent != null)
            {
                if (district.Parent.Id > 0)
                {
                    var provinceDist =
                        Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntity<DistrictEntity>(district.Parent.Id);
                    ResetProvinceByValue(provinceDist.Id.ToString());
                    ResetCityByValue(district.Parent.Id.ToString());
                    ResetCountyByValue(val);
                }
                else
                {
                    ResetProvinceByValue(district.Id.ToString());
                    ResetCityByValue(val);
                }
            }
            else
            {
                ResetProvinceByValue(val);
            }
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// 按值重置省
        /// </summary>
        /// <param name="province"></param>
        protected virtual void ResetProvinceByValue(string province)
        {
            ProvinceDropDownList.SelectedValue = province;
            Province_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// 按值重置市
        /// </summary>
        /// <param name="city"></param>
        protected virtual void ResetCityByValue(string city)
        {
            CityDropDownList.SelectedValue = city;
            City_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// 按值重置镇
        /// </summary>
        /// <param name="county"></param>
        protected virtual void ResetCountyByValue(string county)
        {
            CountyDropDownList.SelectedValue = county;
        }

        /// <summary>
        /// 获取客户端控件值
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        protected virtual string GetClientDropDownListValue(string controlId)
        {
            return Request.Form[controlId.Replace("_", "$")] ?? string.Empty;
        }

        /// <summary>
        /// 获取选定District值
        /// </summary>
        /// <returns></returns>
        public virtual string GetDistrictSelectedValue()
        {
            if (GetClientDropDownListValue(CountyDropDownList.ClientID).Length > 0)
            {
                ProvinceDropDownList.SelectedValue = GetClientDropDownListValue(ProvinceDropDownList.ClientID);
                Province_SelectedIndexChanged(this, new EventArgs());
                CityDropDownList.SelectedValue = GetClientDropDownListValue(CityDropDownList.ClientID);
                City_SelectedIndexChanged(this, new EventArgs());
                CountyDropDownList.SelectedValue = GetClientDropDownListValue(CountyDropDownList.ClientID);
                return CountyDropDownList.SelectedValue;
            }
            if (GetClientDropDownListValue(CityDropDownList.ClientID).Length > 0)
            {
                Province_SelectedIndexChanged(this, new EventArgs());
                CityDropDownList.SelectedValue = GetClientDropDownListValue(CityDropDownList.ClientID);
                return CityDropDownList.SelectedValue;
            }
            ProvinceDropDownList.SelectedValue = GetClientDropDownListValue(ProvinceDropDownList.ClientID);
            return ProvinceDropDownList.SelectedValue;
        }

        /// <summary>
        /// 按名称重置下拉框
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <param name="county">镇</param>
        public virtual void ResetByText(string province, string city, string county)
        {
            ResetProvinceByText(province);
            ResetCityByText(city);
            ResetCountyByText(county);
        }
       
        /// <summary>
        /// 按名称重置省
        /// </summary>
        /// <param name="province"></param>
        protected virtual void ResetProvinceByText(string province)
        {
            foreach (ListItem p in ProvinceDropDownList.Items)
            {
                if (p.Text == province)
                {
                    ProvinceDropDownList.SelectedValue = p.Value;
                    Province_SelectedIndexChanged(this,new EventArgs());
                    break;
                }
            }
        }

        /// <summary>
        /// 按名称重置市
        /// </summary>
        /// <param name="city"></param>
        protected virtual void ResetCityByText(string city)
        {
            foreach (ListItem c in CityDropDownList.Items)
            {
                if (c.Text == city)
                {
                    CityDropDownList.SelectedValue = c.Value;
                    City_SelectedIndexChanged(this, new EventArgs());
                    break;
                }
            }
        }
        
        /// <summary>
        /// 按名称重置镇
        /// </summary>
        /// <param name="county"></param>
        protected virtual void ResetCountyByText(string county)
        {
            foreach (ListItem d in CountyDropDownList.Items)
            {
                if (d.Text == county)
                {
                    CountyDropDownList.SelectedValue = d.Value;
                    break;
                }
            }
        }

        /// <summary>
        /// 省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Province_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitCity();
            InitCounty();
            if (ProvinceDropDownList.SelectedValue.Length > 0)
            {
                var cityList = GetDistrictEntityByParent(Convert.ToInt32(ProvinceDropDownList.SelectedValue));
                foreach (var districtEntity in cityList)
                    CityDropDownList.Items.Add(new ListItem(districtEntity.Name, districtEntity.Id.ToString()));
                City_SelectedIndexChanged(this, new EventArgs());
            }
        }

        protected virtual void InitCity()
        {
            CityDropDownList.Items.Clear();
            //CityDropDownList.Items.Add(new ListItem("请选择", ""));
        }

        protected virtual void InitCounty()
        {
            CountyDropDownList.Items.Clear();
            //CountyDropDownList.Items.Add(new ListItem("请选择", ""));
        }

        protected virtual IEnumerable<DistrictEntity> GetDistrictEntityByParent(long parentId)
        {
            var query = new QueryInfo();
            query.Query<DistrictEntity>().Where(it => it.Parent.Id == parentId && it.IsUsed);
            return Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntities<DistrictEntity>(query);
        }

        protected void City_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitCounty();
            if (CityDropDownList.SelectedValue.Length > 0)
            {
                var districtList = GetDistrictEntityByParent(Convert.ToInt32(CityDropDownList.SelectedValue));
                foreach (var districtEntity in districtList)
                    CountyDropDownList.Items.Add(new ListItem(districtEntity.Name, districtEntity.Id.ToString()));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateClientScript();
        }

        protected virtual string CreateAjaxClientScript(string parentCtrlId, string ctrlId)
        {
            return string.Format("   $(\"#{2}\").empty();" + Environment.NewLine +
                                    "$(\"#{2}\").append('<option  value=\"\">请选择</option>');" + Environment.NewLine +
                                    "$(\"#{2}\").trigger(\"change\");" + Environment.NewLine +
                                    "$.ajax(" + Environment.NewLine +
                                    "{{" + Environment.NewLine +
                                    "    url: '{0}?DistrictId=' + $(\"#{1}\").val()," + Environment.NewLine +
                                    "    type: 'post'," + Environment.NewLine +
                                    "    dataType: 'text'," + Environment.NewLine +
                                    "    success: function (res) " + Environment.NewLine +
                                    "        {{  " + Environment.NewLine +
                                    "            if(res.length > 0) {{ " + Environment.NewLine +
                                    "                var json = eval(res);" + Environment.NewLine +
                                    "                $(json).each(function()" + Environment.NewLine +
                                    "                {{" + Environment.NewLine +
                                    "                    $(\"#{2}\").append('<option  value=' + this.Value + '>' + this.Text + '</option>');" + Environment.NewLine +
                                    "                }});" + Environment.NewLine +
                                    "            }}" + Environment.NewLine +
                                    "        }}," + Environment.NewLine +
                                    "error: function (request, status, errorTxt) {{ alert(errorTxt); }}" + Environment.NewLine +
                                    "}});", "/ajax/basedata/District.aspx", parentCtrlId, ctrlId);
        }

        protected virtual void CreateClientScript()
        {
            var script = new StringBuilder();            
            script.Append(string.Format("$(\"#{0}\").change(function () {{{1}}});", ProvinceDropDownList.ClientID, CreateAjaxClientScript(ProvinceDropDownList.ClientID, CityDropDownList.ClientID)));
            script.Append(string.Format("$(\"#{0}\").change(function () {{{1}}});", CityDropDownList.ClientID, CreateAjaxClientScript(CityDropDownList.ClientID, CountyDropDownList.ClientID)));
            Page.ExecuteScript(script.ToString());
        }
    }
}