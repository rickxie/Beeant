
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Editor;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Editor.Editor.Template
{
    public partial class List : Basic.Services.WebForm.Pages.ListPageBase<TemplateEntity>
    {
        public override bool IsVerifyResource
        {
            get
            {
                return false;
            }
        }
        public virtual long FolderId
        {
            get { return Request.QueryString["folderid"].Convert<long>(); }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            Pager = Pager1;
            RemoveButton = btnRemove;
            base.OnPreLoad(e);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                litFolder.LoadData();
                litMoveFolder.LoadData();
            }
   
        }

      

        protected override void SetQuery(QueryInfo query)
        {

            if (FolderId != 0)
                query.Query<ImageEntity>().Where(it => it.Folder.Id == FolderId && it.Account.Id == 0);
            else
                query.Query<ImageEntity>().Where(it => it.Account.Id == 0);
            base.SetQuery(query);
        }

        protected override void BindEntities(IList<TemplateEntity> infos)
        {
            if (infos == null) return;
            Repeater1.DataSource = infos;
            Repeater1.DataBind();
            Pager1.DataBind();

        }
  
        /// <summary>
        /// 重写脚本
        /// </summary>
        protected override void AddClientScript()
        {
            this.RegisterScript(CheckScriptPath);
            this.RegisterScript(ConfirmBoxScriptPath);
            this.RegisterScript("/Scripts/Winner/Editor/Finder/Finder.js");
            this.RegisterScript("/Scripts/Folder.js");
            this.ExecuteScript(string.Format("var checkbox=new Winner.CheckBox('finder');"));
            this.ExecuteScript(string.Format("document.domain='{0}';", ConfigurationManager.GetSetting<string>("Domain")));
            this.ExecuteScript("var confirmBox=new Winner.ConfirmBox();");
            this.ExecuteScript("var finder=new Winner.Editor.Finder('finder');");
            this.ExecuteScript("var folder = new Folder('hfMoveSwitcher', 'hfMoveContainer', '/Editor/Template/Move.aspx');");
            this.ExecuteScript("checkbox.Initialize();confirmBox.Initialize();finder.Initialize();folder.Initialize();");
            base.AddClientScript();
        }


        
        /// <summary>
        /// 重写得到存储集合
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="saveType"></param>
        /// <param name="isBindDataKey"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
        {
            return (from RepeaterItem item in Repeater1.Items select item.FindControl("ckSelect") as HtmlInputCheckBox into ckSelect where ckSelect != null && ckSelect.Checked select new TemplateEntity { SaveType = saveType, Id = ckSelect.Value.Convert<long>() } as TEntityType).ToList();
        }
 
       
    }
}