using System;
using System.Web.UI;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;
using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public abstract class DropDownListBaseControl : UserControl
    {

        public bool AutoPostBack
        {
            get { return DropDownList.AutoPostBack; }
            set
            {
                DropDownList.AutoPostBack = value;
                DropDownList.SelectedIndexChanged += SelectedIndexChanged_Click;
            }
        }

   
        
        public event SelectedIndexChangedEventHandler SelectedIndexChanged;
        public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// 查询条件
        /// </summary>
        public virtual QueryInfo Query { get; set; }
        /// <summary>
        /// 绑定控件
        /// </summary>
        public virtual DropDownList DropDownList { get; set; }
        /// <summary>
        /// 存储名称
        /// </summary>
        public virtual string SaveName
        {
            get { return DropDownList.Attributes["SaveName"]; }
            set { DropDownList.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public virtual string BindName
        {
            get { return DropDownList.Attributes["BindName"]; }
            set { DropDownList.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public virtual string ValidateName
        {
            get { return DropDownList.Attributes["ValidateName"]; }
            set { DropDownList.Attributes.Add("ValidateName", value); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public virtual string SearchWhere
        {
            get { return DropDownList.Attributes["SearchWhere"]; }
            set { DropDownList.Attributes.Add("SearchWhere", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public virtual string SearchParamterName
        {
            get { return DropDownList.Attributes["SearchParamterName"]; }
            set { DropDownList.Attributes.Add("SearchParamterName", value); }
        }
        /// <summary>
        /// 搜索实体对象字段名称
        /// </summary>
        public virtual string SearchPropertyTypeName
        {
            get { return DropDownList.Attributes["SearchPropertyTypeName"]; }
            set { DropDownList.Attributes.Add("SearchPropertyTypeName", value); }
        }
        /// <summary>
        /// 不搜索的值
        /// </summary>
        public virtual string UnSearchValue
        {
            get { return DropDownList.Attributes["UnSearchValue"]; }
            set { DropDownList.Attributes.Add("UnSearchValue", value); }
        }
        /// <summary>
        /// 绑定名称
        /// </summary>
        public virtual string DataTextField
        {
            get { return DropDownList.DataTextField; }
            set { DropDownList.DataTextField = value; }
        }

        /// <summary>
        /// 绑定值
        /// </summary>
        public virtual string DataValueField
        {
            get { return DropDownList.DataValueField; }
            set { DropDownList.DataValueField = value; }
        }
        /// <summary>
        /// 是否枚举
        /// </summary>
        public virtual bool IsEnum { get; set; }
        /// <summary>
        /// 实体名称
        /// </summary>
        public virtual string ObjectName { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public virtual string WhereExp { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual string OrderByExp { get; set; }
        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
            DataBind(GetSource());
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="source"></param>
        public virtual void DataBind(object source)
        {
            DropDownList.DataBind(source);
        }
        /// <summary>
        /// 重写数据源
        /// </summary>
        /// <returns></returns>
        public virtual object GetSource()
        {
            if (IsEnum)
                return GetEnumSource();
            return GetEntitySource();
        }

        /// <summary>
        /// 重写数据源
        /// </summary>
        /// <returns></returns>
        protected virtual object GetEntitySource()
        {
            if (Query == null) Query = new QueryInfo();
            if (string.IsNullOrEmpty(Query.SelectExp))
                Query.SelectExp = string.Format("{0},{1}", DataTextField, DataValueField);
            if (string.IsNullOrEmpty(Query.FromExp))
                Query.From(ObjectName);
            if (string.IsNullOrEmpty(Query.WhereExp))
                Query.WhereExp = WhereExp;
            if (string.IsNullOrEmpty(Query.OrderByExp))
                Query.OrderByExp = OrderByExp;
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(Query);
        }

        /// <summary>
        /// 得到数据源
        /// </summary>
        /// <returns></returns>
        protected virtual object GetEnumSource()
        {
            DataTextField = "Message";
            DataValueField = "Name";
            return EnumExtension.GetNames(ObjectName);
        }
        
        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SelectedIndexChanged_Click(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(sender, e);
        }

       
    }
}
