using System.Text;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Workflow;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;

namespace Beeant.Presentation.Admin.Configurator.Workflow.Node
{
    public partial class List : MaintenPageBase<NodeEntity>
    {

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlFlow.LoadData();
                ddlSearchFlow.LoadData();
                ddlAuditor.LoadData();
                ddlAssignType.LoadData();
                ddlConditionType.LoadData();
                ddlNodeType.LoadData();
                LoadMessageType();
            } 
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 填充实体
        /// </summary>
        /// <returns></returns>
        protected override NodeEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.MessageType = GetMessageType();
            }
            return info;
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        protected virtual void LoadMessageType()
        {
           
            var source = EnumExtension.GetNames<MessageType>();
            ckMessageType.DataTextField = "Message";
            ckMessageType.DataValueField = "Name";
            ckMessageType.DataSource = source;
            ckMessageType.DataBind();
            if (ckMessageType.Items.Count > 0)
                ckMessageType.Items[0].Selected = true;
        }
        /// <summary>
        /// 得到消息类型
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMessageType()
        {
            if (ckMessageType == null) return 0;
            var value = new StringBuilder();
            foreach (ListItem li in ckMessageType.Items)
            {
                if (li.Selected)
                    value.AppendFormat("{0},", li.Value);
            }
            if (value.Length > 0) value.Remove(value.Length - 1, 1);
            return value.ToString().GetEnumSumValue<MessageType>();
        }
    }
}