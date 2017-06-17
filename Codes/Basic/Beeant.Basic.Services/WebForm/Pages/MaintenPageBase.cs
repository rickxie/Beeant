using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Basic.Services.WebForm.Controls;
using Dependent;
using Beeant.Application.Services;
using Winner.Filter;
using Winner.Persistence;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class MaintenPageBase<T> : ListPageBase<T> where T : Domain.Entities.BaseEntity, new()
    {
        #region 具体控件

        /// <summary>
        /// 请求的Id控件
        /// </summary>
        public HtmlInputHidden RequestIdControl { get; set; }

        /// <summary>
        /// 请求的Id
        /// </summary>
        public virtual long RequestId 
        {
            get { return RequestIdControl != null ? RequestIdControl.Value.Convert<long>() : 0; }
            set
            {
                if (RequestIdControl != null) RequestIdControl.Value = value.ToString();
            }
        }

        /// <summary>
        /// 消息控件
        /// </summary>
        public MessageControlBase MessageControl { get; set; }

        /// <summary>
        /// 保存控件
        /// </summary>
        public Button SaveButton { get; set; }

        private string _validScriptPath = "/scripts/Winner/Validator/Winner.Validator.js";
        /// <summary>
        /// 验证脚本路径
        /// </summary>
        public string ValidScriptPath
        {
            get { return _validScriptPath; }
            set { _validScriptPath = value; }
        }

 
        #endregion

        #region 属性

        private bool _isFillAllEntity = true;


        /// <summary>
        /// 是否是部分绑定
        /// </summary>
        public virtual bool IsFillAllEntity
        {
            get
            {
                return _isFillAllEntity;
            }
            set
            {
                _isFillAllEntity = value;
            }
        }

        /// <summary>
        /// 是否显示编辑容器
        /// </summary>
        public virtual bool IsShowEditContainer { get; set; }
        /// <summary>
        /// 验证脚本
        /// </summary>
        protected string ValidScriptString { get; set; }
        #endregion

        #region 检查控件
        /// <summary>
        /// 检查MessageControlBase
        /// </summary>
        protected override void CheckControl()
        {
            base.CheckControl();
            LoadMessageControl();
            LoadSaveButtonControl();
            LoadIdControl();
            BindGirdViewRowCommand();
        }
        /// <summary>
        /// 加载消息
        /// </summary>
        protected virtual void LoadMessageControl()
        {
            if (MessageControl == null)
                MessageControl = Page.FindControl("ctl00$Body$Message1") as MessageControlBase;
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadSaveButtonControl()
        {
            if (SaveButton == null)
                SaveButton = Page.FindControl("ctl00$Body$btnSave") as Button;
            if (SaveButton != null)
                SaveButton.Click += Save_Click;
        }
        /// <summary>
        /// 加载Id隐藏控件
        /// </summary>
        protected virtual void LoadIdControl()
        {
            if (RequestIdControl == null)
                RequestIdControl = Page.FindControl("ctl00$Body$IdControl") as HtmlInputHidden;
        }

        /// <summary>
        /// 绑定编辑命令
        /// </summary>
        protected virtual void BindGirdViewRowCommand()
        {
            if(GridView!=null)
                GridView.RowCommand+=GridView_RowCommand;
        }

        #endregion

        #region 注册脚本
        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected override void AddClientScript ()
        {
            base.AddClientScript();
            if (SaveButton != null)
            {
                this.RegisterScript(ValidScriptPath);
                ValidScriptString = string.IsNullOrEmpty(ValidScriptString) ?
                    this.SetEntity(SaveButton, typeof(T), RequestId ==0? ValidationType.Add : ValidationType.Modify) : ValidScriptString;
                this.ExecuteScript(ValidScriptString);
            }
            ExecuteScriptMaintenPage();
        }
    
        /// <summary>
        ///执行维护页面的脚本
        /// </summary>
        protected virtual void ExecuteScriptMaintenPage()
        {
            string isShow = IsShowEditContainer || RequestId!=0 ||
                            (MessageControl != null && !string.IsNullOrEmpty(MessageControl.Message))
                                ? "true"
                                : "false";
            this.ExecuteScript(
                string.Format("var maintenPage=new MaintenPage('Edit','Add','Hide',{0});", isShow));
        }

        #endregion

        #region 方法

        /// <summary>
        /// 加载方法
        /// </summary>
        protected virtual void LoadEntity()
        {
            var info = GetEntity();
            BindEntity(info);
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="info"></param>
        protected virtual void BindEntity(T info)
        {
            if (info != null)
            {
                ValidScriptString = this.BindEntity(info, SaveButton, ValidationType.Modify);
            }
            
        }
        /// <summary>
        /// 加载实体
        /// </summary>
        /// <returns></returns>
        protected virtual T GetEntity()
        {
            if (RequestId==0)
                return null;
            return Ioc.Resolve<IApplicationService,T>().GetEntity<T>(RequestId);
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <returns></returns>
        protected virtual T FillEntity()
        {
            var info = this.GetEntity<T>(IsFillAllEntity);
            if (RequestId==0)
            {
                info.SaveType = SaveType.Add;
            }
            else
            {
                info.Id = RequestId;
                info.SaveType = SaveType.Modify;
            }
            return info;
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="info"></param>
        protected virtual void Save(T info)
        {

            Ioc.Resolve<IApplicationService,T>().Save(info);

        }
        /// <summary>
        /// 设置返回结果
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SetResult(T info)
        {
            if (info==null) return;
            if (MessageControl != null)
                MessageControl.ShowMessage(info.Errors);
            if(info.Errors == null || info.Errors.Count == 0)
            {
                RequestId = 0;
                LoadData();
                this.ResetControl();
            }
        }


        #endregion

        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Save_Click(object sender, EventArgs e)
        {
            T info = FillEntity();
            Save(info);
            SetResult(info);
        }
        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Modify"))
            {
                RequestId = e.CommandArgument.Convert<long>();
                LoadEntity();
            }
        }
        #endregion
 
    }
}
