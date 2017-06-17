using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public class ZTreeBaseControl : UserControl
    {
        /// <summary>
        /// Query
        /// </summary>
        public virtual QueryInfo Query { get; set; }
        /// <summary>
        /// 对象名称
        /// </summary>
        public virtual string ObjectName { get; set; }


        /// <summary>
        /// 输出数据对象
        /// </summary>
        public virtual string ObjectFields
        {
            set { _objectFields = value; }
            get
            {
                return string.IsNullOrEmpty(_objectFields)
                           ? string.Format("{0},{1},{2}", DataTextField, DataValueField, DataParentField)
                           : _objectFields;
            }
        }
        private string _objectFields;

        /// <summary>
        /// 默认宽度
        /// </summary>
        private string _width = "100%";
        /// <summary>
        /// 宽度
        /// </summary>
        public virtual string Width
        {
            set { _width = value; }
            get { return _width; }
        }

        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        public virtual bool ShowDel { set; get; }
        /// <summary>
        /// TreeNodeStr
        /// </summary>
        protected virtual string TreeNodeStr { set; get; }
        /// <summary>
        /// 全部展开
        /// </summary>
        public virtual bool ExpandAll { set; get; }
        /// <summary>
        /// 数据显示名称
        /// </summary>
        public virtual string DataTextField { set; get; }
        /// <summary>
        /// 数据显示值
        /// </summary>
        public virtual string DataValueField { set; get; }
        /// <summary>
        /// 数据父对象
        /// </summary>
        public virtual string DataParentField { set; get; }
        /// <summary>
        /// 单击Node节点事件
        /// </summary>
        public virtual string ClickNode { set; get; }
        /// <summary>
        /// 默认加载方式
        /// </summary>
        protected bool isAsync = false;
        /// <summary>
        /// 加载方式
        /// </summary>
        public virtual bool IsAsync
        {
            get { return isAsync; }
            set { isAsync = value; }
        }
        /// <summary>
        /// 默认异步加载页面
        /// </summary>
        protected string asyncUrl = "/Ajax/Ztree/AsyncTree.aspx";
        /// <summary>
        /// 异步加载页面
        /// </summary>
        public virtual string AsyncUrl
        {
            get { return asyncUrl; }
            set { asyncUrl = value; }
        }
        /// <summary>
        /// 异步加载参数
        /// </summary>
        /// <returns></returns>
        public virtual string GetAsyncParams { set; get; }
       
        /// <summary>
        /// 启用异步加载后默认第一层父节点Key
        /// </summary>
        protected string defaultParentKey = "0";
        /// <summary>
        /// 启用异步加载后第一层父节点Key
        /// </summary>
        public string DefaultParentKey
        {
            get { return defaultParentKey; }
            set { defaultParentKey = value; }
        }

        /// <summary>
        /// 是否以弹出框显示
        /// </summary>
        public bool ShowDialog { get; set; }

        /// <summary>
        /// Tree容器
        /// </summary>
        public virtual string TreeContainer { set; get; }

        /// <summary>
        /// 其他参数
        /// </summary>
        public virtual string OtherParams { set; get; }

        /// <summary>
        /// 对话框默认标题
        /// </summary>
        protected string defaultDialogTitle = "请选择";
        /// <summary>
        /// 对话框标题
        /// </summary>
        public virtual string DialogTitle
        {
            get { return defaultDialogTitle; }
            set { defaultDialogTitle = value; }
        }

        /// <summary>
        /// 点击对话框确定按钮
        /// </summary>
        public virtual string SaveDialog { set; get; }

        /// <summary>
        /// 重写Render
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsAsync)
                LoadTreeContent();
            RenderStyle(writer);
            RenderScript(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "borderPanel");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("overflow-y: auto; overflow-x:hidden;width:{0}px;", Width));
            writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}Main", ID));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ztree");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        /// <summary>
        /// 输出样式
        /// </summary>
        /// <param name="writer"></param>
        private void RenderStyle(HtmlTextWriter writer)
        {
            writer.WriteLine("<style>");
            writer.WriteLine("    ul.ztree");
            writer.WriteLine("    {");
            writer.WriteLine(string.Format("        width: {0};  ", Width));
            writer.WriteLine("    }");
            writer.WriteLine("    .ztree li span.button.ico_open {");
            writer.WriteLine("        margin-right: 2px;");
            writer.WriteLine("        vertical-align: top;");
            writer.WriteLine("    }");
            writer.WriteLine("");
            writer.WriteLine("    .ztree li span.button.ico_close {");
            writer.WriteLine("        margin-right: 2px;");
            writer.WriteLine("        vertical-align: top;");
            writer.WriteLine("    }");
            writer.WriteLine("    #dt_list td");
            writer.WriteLine("    {");
            writer.WriteLine("    ");
            writer.WriteLine("    }");
            writer.WriteLine("</style>");
        }

        private void RenderScriptHeader(HtmlTextWriter writer)
        {
            writer.WriteLine("<script language=\"javascript\" type=\"text/javascript\">");
        }

        private void RenderScriptEnd(HtmlTextWriter writer)
        {
            writer.WriteLine("</script>");
        }

        private void RenderTreeScript(HtmlTextWriter writer)
        {
            #region 初始化Tree控件
            writer.WriteLine("$(document).ready(function () {");
            writer.WriteLine(string.Format("  {0}setting = {{", ID));
            if (IsAsync)
            {
                writer.WriteLine("        async: {");
                writer.WriteLine("        enable: true,");
                if (!string.IsNullOrEmpty(GetAsyncParams))
                    writer.WriteLine(string.Format("        url:{0},", GetAsyncParams));
                else
                    writer.WriteLine(string.Format("        url:\"{0}\",", AsyncUrl));
				writer.WriteLine(string.Format("        autoParam:[\"{0}\"],",DataValueField));
                writer.WriteLine(string.Format("        otherParam:{{\"DataTextField\":\"{0}\",\"DataValueField\":\"{1}\",\"DataParentField\":\"{2}\",\"ObjectFields\":\"{3}\",\"DefaultParentKey\":\"{4}\",\"ObjectName\":\"{5}\",\"ExpandAll\":\"{6}\"{7}}}", DataTextField, DataValueField, DataParentField, ObjectFields, DefaultParentKey, ObjectName, ExpandAll, OtherParams));
                writer.WriteLine("        },");
            }
            else
            {
                writer.WriteLine("        data: {");
                writer.WriteLine("            simpleData: {");
                writer.WriteLine("                enable: true,");
                writer.WriteLine(string.Format("                idKey: \"{0}\",", DataValueField));
                writer.WriteLine(string.Format("                pIdKey: \"{0}\",", DataParentField));
                writer.WriteLine("                rootPId: null");
                writer.WriteLine("            }");
                writer.WriteLine("        },"); 
            }
            writer.WriteLine("        edit: {");
            if (ShowDel)
            {
                writer.WriteLine("            enable: true,");
                writer.WriteLine("            showRemoveBtn: true,");
            }
            else
            {
                writer.WriteLine("            enable: false,");
                writer.WriteLine("            showRemoveBtn: false,");
            }
            writer.WriteLine("            showRenameBtn: false,");
            writer.WriteLine("            removeTitle: '删除'");
            writer.WriteLine("        },");
            writer.WriteLine("        showLine: true,");
            writer.WriteLine("        callback: {");
            writer.WriteLine(string.Format("            onClick: {0}treeClick,", ID));
            //writer.WriteLine(string.Format("            beforeRemove: {0}treeRemoveBefore", this.ID));
            writer.WriteLine("        }");
            writer.WriteLine("    };");
            if (!IsAsync)
            {
                writer.WriteLine(string.Format("  {0}zNodes = {1};", ID, TreeNodeStr));
                writer.WriteLine(string.Format("  {0}TreeObj =  $.fn.zTree.init($(\"#{0}\"), {0}setting, {0}zNodes);", ID));
            }
            else
            {
                writer.WriteLine(string.Format("  {0}TreeObj =  $.fn.zTree.init($(\"#{0}\"), {0}setting);", ID));
            }
            writer.WriteLine("});");

            #endregion
            RenderDialog(writer);
            #region 目录树节点单击事件绑定
            writer.WriteLine(string.Format("function {0}treeClick(event, treeId, treeNode) {{", ID));
            writer.WriteLine(!string.IsNullOrEmpty(ClickNode)
                                 ? string.Format("        return {0}(treeNode);", ClickNode)
                                 : "        return true;");
            writer.WriteLine("}");
            writer.WriteLine(string.Format("function {0}Init() {{", ID));
            if (!IsAsync)
            {
                writer.WriteLine(string.Format("  {0}TreeObj =  $.fn.zTree.init($(\"#{0}\"), {0}setting, {0}zNodes);", ID));
            }
            else
            {
                writer.WriteLine(string.Format("  {0}TreeObj =  $.fn.zTree.init($(\"#{0}\"), {0}setting);", ID));
            }
            writer.WriteLine("}");
            #endregion
            #region 目录树删除事件
            //writer.WriteLine(string.Format("function {0}treeRemoveBefore(treeId, treeNode) {{", this.ID));
            //writer.WriteLine("var returnValue = false;");
            //writer.WriteLine("if(confirm(\"是否要删除[\" + treeNode.name + \"]?\"))");
            //writer.WriteLine("{");
            //writer.WriteLine("    $.ajax({");
            //writer.WriteLine("        url: \"DelEntity.aspx\",");
            //writer.WriteLine("        type: \"post\",");
            //writer.WriteLine("        dataType: \"json\",");
            //writer.WriteLine("        async: false,");
            //writer.WriteLine(string.Format("        data: 'modelName={0}&funId={1}&idList=' + treeNode[\"{2}\"],", modelName, funId, idKey));
            //writer.WriteLine("        success: function (json) {");
            //writer.WriteLine("            if(json.result == 1)");
            //writer.WriteLine("            {");
            //writer.WriteLine("                MsgBox(json.message);");
            //writer.WriteLine("                returnValue = true;");
            //writer.WriteLine("            }");
            //writer.WriteLine("            else");
            //writer.WriteLine("            {");
            //writer.WriteLine("                MsgBox(json.message);");
            //writer.WriteLine("                returnValue = false;");
            //writer.WriteLine("            }");
            //writer.WriteLine("        },");
            //writer.WriteLine("        error: function (txt, st, st1) {");
            //writer.WriteLine("            returnValue = false;");
            //writer.WriteLine("        }");
            //writer.WriteLine("    });");
            //writer.WriteLine("}");
            //writer.WriteLine("return returnValue;");
            //writer.WriteLine("}");
            #endregion
        }

        protected virtual void RenderDialog(HtmlTextWriter writer)
        {
            if (ShowDialog)
            {
                writer.WriteLine(string.Format("$(\"#{0}\").hide();", TreeContainer));
                writer.WriteLine(string.Format("var IsShow{0}Dialog = false;", ClientID));
                writer.WriteLine(string.Format("function Init{0}Dialog(){{", ClientID));
                writer.WriteLine(string.Format("var IsShow{0}Dialog = true;", ClientID));
                writer.WriteLine(string.Format("{0}Dialog = new Winner.Dialog(\"{1}\", \"\");", ClientID, DialogTitle));
                writer.WriteLine(string.Format("{0}Dialog.IsShowDialog = false;", ClientID));
                writer.WriteLine(string.Format("{0}Dialog.Initialize();", ClientID));
                writer.WriteLine(string.Format("$(\"#{0}\").show();", TreeContainer));
                writer.WriteLine(string.Format("$({0}Dialog.Detail).append($(\"#{1}\"));", ClientID, TreeContainer));
                writer.WriteLine("}");

                ClickNode = string.Format("{0}SaveData", ClientID);
                writer.WriteLine(string.Format("global{0}Data= {{}};", ClientID));
                writer.WriteLine(string.Format("function {0}SaveData(data){{", ClientID));
                writer.WriteLine(string.Format("global{0}Data= data;", ClientID));
                writer.WriteLine("}");

                writer.WriteLine(string.Format("function Show{0}Dialog() {{", TreeContainer));
                writer.WriteLine(string.Format("if (!IsShow{0}Dialog) {{", ClientID));
                writer.WriteLine(string.Format("Init{0}Dialog();", ClientID));
                writer.WriteLine("}");
                writer.WriteLine(string.Format("{0}Dialog.ShowDialog();", ClientID));
                if (!string.IsNullOrEmpty(SaveDialog))
                {
                    writer.WriteLine(string.Format("{0}Dialog.SureFunction = function (){{", ClientID));
                    writer.WriteLine(string.Format("return {0}(global{1}Data)", SaveDialog, ClientID));
                    writer.WriteLine("}");
                }     
                writer.WriteLine("}");
            }
        }

        protected virtual void RenderOtherScript(HtmlTextWriter writer)
        {
            
        }

        /// <summary>
        /// 输出脚本
        /// </summary>
        /// <param name="writer"></param>
        private void RenderScript(HtmlTextWriter writer)
        {
            RenderScriptHeader(writer);
            RenderTreeScript(writer);
            RenderOtherScript(writer);
            RenderScriptEnd(writer);
        }        

        /// <summary>
        /// 加载Entity数据
        /// </summary>
        private void LoadTreeContent()
        {
            var infos = GetEntities();
            TreeNodeStr = ConvertJsonEntity(infos);
            TreeNodeStr = TreeNodeStr.Replace(string.Format("\"{0}\":null", DataParentField), string.Format("\"{0}\":\"{1}\"", DataParentField, "0"));
            if (ExpandAll)
                TreeNodeStr = TreeNodeStr.Replace("},", ",open:true},");
            TreeNodeStr = TreeNodeStr.Replace(string.Format("\"{0}\":", DataTextField), "\"name\":");
        }
        /// <summary>
        /// 得到数据集
        /// </summary>
        /// <returns></returns>
        protected virtual IList<BaseEntity> GetEntities()
        {
            if (Query == null) Query = new QueryInfo();
            if (string.IsNullOrEmpty(Query.SelectExp))
                Query.SelectExp = ObjectFields;
            if (string.IsNullOrEmpty(Query.FromExp))
                Query.From(ObjectName);
            if (string.IsNullOrEmpty(Query.WhereExp) && (IsAsync && DefaultParentKey.Length > 0))
            {
                Query.WhereExp = string.Format("{0}==@ParentKey", DataParentField);
                Query.SetParameter("ParentKey", DefaultParentKey);
            }
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(Query);
        }
        /// <summary>
        /// Entity转Json字符串
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual string ConvertJsonEntity(IList<BaseEntity> infos)
        {
            var arr = new ArrayList();
            var fileds = ObjectFields.Split(',').ToList();
            infos.ToList().ForEach(item =>
                {
                    var propertys = new Dictionary<string, object>();
                    fileds.ForEach(field =>
                        {
                            propertys.Add(field, Winner.Creator.Get<Winner.Base.IProperty>().GetValue<object>(item,field));
                        });
                    if(IsAsync)
                        propertys.Add("isParent", "true");
                    arr.Add(propertys);
                });
            return arr.SerializeJson();
        }
    }
}
