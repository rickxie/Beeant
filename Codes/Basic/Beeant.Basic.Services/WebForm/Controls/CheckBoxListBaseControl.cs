using System.Web.UI;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public abstract class CheckBoxListBaseControl : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public virtual QueryInfo Query { get; set; }
        /// <summary>
        /// 绑定控件
        /// </summary>
        public virtual System.Web.UI.WebControls.CheckBoxList CheckBoxList { get; set; }
        /// <summary>
        /// 存储名称
        /// </summary>
        public virtual string SaveName
        {
            get { return CheckBoxList.Attributes["SaveName"]; }
            set { CheckBoxList.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public virtual string BindName
        {
            get { return CheckBoxList.Attributes["BindName"]; }
            set { CheckBoxList.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public virtual string ValidateName
        {
            get { return CheckBoxList.Attributes["ValidateName"]; }
            set { CheckBoxList.Attributes.Add("ValidateName", value); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public virtual string SearchWhere
        {
            get { return CheckBoxList.Attributes["SearchWhere"]; }
            set { CheckBoxList.Attributes.Add("SearchWhere", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public virtual string SearchParamterName
        {
            get { return CheckBoxList.Attributes["SearchParamterName"]; }
            set { CheckBoxList.Attributes.Add("SearchParamterName", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public virtual string SearchPropertyTypeName
        {
            get { return CheckBoxList.Attributes["SearchPropertyTypeName"]; }
            set { CheckBoxList.Attributes.Add("SearchPropertyTypeName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public virtual string DataTextField
        {
            get { return CheckBoxList.DataTextField; }
            set { CheckBoxList.DataTextField = value; }
        }

        /// <summary>
        /// 绑定值
        /// </summary>
        public virtual string DataValueField
        {
            get { return CheckBoxList.DataValueField; }
            set { CheckBoxList.DataValueField = value; }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
            CheckBoxList.DataSource = GetSource();
            CheckBoxList.DataBind();
        }

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
        /// 是否枚举
        /// </summary>
        public virtual bool IsEnum { get; set; }
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
    }
}
