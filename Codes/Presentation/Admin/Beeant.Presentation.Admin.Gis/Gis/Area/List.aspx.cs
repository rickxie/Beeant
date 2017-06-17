using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;
using Configuration;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Gis.Gis.Area
{
    public partial class List : ListPageBase<AreaEntity>
    {
        public string City
        {
            get
            {
                return string.IsNullOrEmpty(Request.QueryString["City"])
                    ? "上海"
                    : Server.UrlDecode(Request.QueryString["City"]);
            }
        }
        public string Address
        {
            get
            {
                return Server.UrlDecode(Request.QueryString["address"]);
            }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCity.LoadData();
                ddlCity.DropDownList.SelectedIndex = ddlCity.DropDownList.Items.IndexOf(ddlCity.DropDownList.Items.FindByValue(City));
                if (Request.QueryString["ispublish"] == "true")
                {
                    btnImport.Visible = false;
                    btnPublish.Visible = false;
                }

            }
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// Url
        /// </summary>
        /// <returns></returns>
        public virtual string GetKey()
        {
            return ConfigurationManager.GetSetting<string>("BaiduMap").DeserializeJson<dynamic>().Ak.ToString();
        }
        /// <summary>
        /// 导入生产地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCity.DropDownList.SelectedValue))
            {
                this.ShowMessage("操作提示","请选择城市");
                return;
            }
            IList<AreaEntity> infos = GetAreas();
            if(infos==null)
                return;
            foreach (var info in infos)
            {
                info.Import();
                info.SetProperty(it => it.Origin);
                info.SaveType = SaveType.Modify;
            }
            var rev= Ioc.Resolve<IApplicationService, AreaEntity>().Save(infos);
            this.ShowMessage("操作提示", rev?"导入成功":"导入失败");
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCity.DropDownList.SelectedValue))
            {
                this.ShowMessage("操作提示", "请选择城市");
                return;
            }
            IList<AreaEntity> infos = GetAreas();
            if (infos == null)
                return;
            foreach (var info in infos)
            {
                info.Publish();
            }
            var rev = Ioc.Resolve<IApplicationService, AreaEntity>().Save(infos);
            this.ShowMessage("操作提示", rev ? "发布成功" : "发布失败");
        }
        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AreaEntity> GetAreas()
        {
            var query=new QueryInfo();
            query.Query<AreaEntity>().Where(it => it.City == ddlCity.DropDownList.SelectedValue);
            return Ioc.Resolve<IApplicationService, AreaEntity>().GetEntities<AreaEntity>(query);
        } 
    }

}