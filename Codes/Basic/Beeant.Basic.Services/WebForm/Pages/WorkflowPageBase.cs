using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Workflow;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public abstract class  WorkflowPageBase<T> : DatumPageBase<T> where T : BaseEntity, new()
    {
        #region 属性
      

        private long? _taskId;
        /// <summary>
        /// 任务编号
        /// </summary>
        public virtual long TaskId
        {
            get
            {
                if (_taskId.HasValue)
                    return _taskId.Value;
                _taskId = Request["TaskId"].Convert<long>();
                return _taskId.Value;
            }
            set { _taskId = value; }
        }
        /// <summary>
        /// 是否填充实体
        /// </summary>
        public override bool IsFillAllEntity
        {
            get { return TaskId==0; }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

        private SaveType? _saveType;
        /// <summary>
        /// 重写保存
        /// </summary>
        public override SaveType SaveType
        {
            get
            {
                if (_saveType.HasValue)
                    return _saveType.Value;
                return TaskId == 0 ? SaveType.Add : SaveType.Modify;
            }
            set
            {
                _saveType = value;
            }
        }

        /// <summary>
        /// 级别下拉框
        /// </summary>
        public virtual DropDownList LevelDrowDownList { get; set; }
        /// <summary>
        /// 保存控件
        /// </summary>
        public virtual Button PassButton { get; set; }
        /// <summary>
        /// 保存控件
        /// </summary>
        public virtual Button RejectButton { get; set; }
        /// <summary>
        /// 提交控件
        /// </summary>
        public virtual Button SubmitButton { get; set; }
        /// <summary>
        /// 用户备注控件
        /// </summary>
        public virtual HtmlInputText RemarkControl { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public virtual PagerControlBase TaskPagerControl { get; set; }



        private WorkflowArgsEntity _workflowArgs;
        /// <summary>
        /// 当前工作流
        /// </summary>
        public WorkflowArgsEntity WorkflowArgs
        {
            get
            {
                if (_workflowArgs == null)
                {
                    _workflowArgs = Ioc.Resolve<IWorkflowEngineApplicationService>().GetWorkflowArgs();
                    _workflowArgs.Entity = new T();
                }
                return _workflowArgs;
            }
        }
        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <returns></returns>
        protected virtual long AccountId
        {
            get { return Identity == null ? Request["accountid"].Convert<long>() : Identity.Id; }
        }

        private long? _requestId;
        /// <summary>
        /// 重写编号
        /// </summary>
        public override long RequestId
        {
            get
            {
                if (_requestId.HasValue)
                    return _requestId.Value;
                _requestId = Task == null || Task.Account==null || Task.Account.Id== AccountId ? base.RequestId : Task.DataId;
                return _requestId.Value;
            }
            set { base.RequestId = value; }
        }
        /// <summary>
        /// 任务
        /// </summary>
        public virtual TaskEntity Task { get; set; }
        /// <summary>
        /// 得到任务
        /// </summary>
        /// <returns></returns>
        protected virtual TaskEntity SetTask()
        {
            if (TaskId == 0)
                return null;
            return Ioc.Resolve<IApplicationService>().GetEntity<TaskEntity>(TaskId);

        }
        #endregion

        #region 检查控件
        /// <summary>
        /// 检查GridView和PagerControlBase
        /// </summary>
        protected override void CheckControl()
        {
            base.CheckControl();
            LoadSubmitButtonControl();
            LoadRejectButtonControl();
            LoadPassButtonControl();
            SetControlVisible();
            LoadHistoryPagerControl();
        }
        /// <summary>
        /// 设置
        /// </summary>
        protected virtual void SetControlVisible()
        {
            if (SaveType == SaveType.Add)
            {
                if (RejectButton != null)
                    RejectButton.Visible = false;
                if (PassButton != null)
                    PassButton.Visible = false;
            }
            if (SaveType == SaveType.Modify)
            {
                var rev = CheckHandleButton();
                if (RejectButton != null)
                    RejectButton.Visible = rev;
                if (PassButton != null)
                    PassButton.Visible = rev;
                if (SubmitButton != null && !CheckSubmitButton())
                {
                    SubmitButton.Visible = false;
                }
                  
            }
        }
        /// <summary>
        /// 检查是否隐藏提交
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckSubmitButton()
        {
        
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(1)
                .Query<TaskEntity>()
                .Where(it => it.DataId == RequestId && it.Flow.Id == WorkflowArgs.Flow.Id)
                .OrderByDescending(it=>it.Id)
                .Select(it => it.Node.NodeType);
            var info = Ioc.Resolve<IApplicationService, TaskEntity>().GetEntities<TaskEntity>(query)?.FirstOrDefault();
            return info==null || info.Node!=null && info.Node.NodeType == NodeType.Start;
        }
        /// <summary>
        /// 检查是否隐藏提交
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckHandleButton()
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(1)
                .Query<TaskEntity>()
                .Where(it => it.DataId == RequestId && it.Flow.Id == WorkflowArgs.Flow.Id && it.Status== TaskStatusType.Waiting && it.Account.Id==Identity.Id)
                .Select(it => it.Id);
            var infos = Ioc.Resolve<IApplicationService, TaskEntity>().GetEntities<TaskEntity>(query);
            return infos != null && infos.Count > 0;
        }
        /// <summary>
        /// 重新保存
        /// </summary>
        protected override void LoadSaveButtonControl()
        {
            if (SaveButton == null)
                SaveButton = Page.FindControl("ctl00$Body$btnSave") as Button;
            base.LoadSaveButtonControl();
        }

        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadSubmitButtonControl()
        {
            if (SubmitButton == null)
                SubmitButton = Page.FindControl("ctl00$Body$btnSubmit") as Button;
            if (SubmitButton != null)
            {
                SubmitButton.Click += SubmitButton_Click;
            }
        }
        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SubmitButton_Click(object sender, EventArgs e)
        {
            SaveWorkflowArgs(true);
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadRejectButtonControl()
        {
            if (RejectButton == null)
                RejectButton = Page.FindControl("ctl00$Body$btnReject") as Button;
            if (RejectButton != null)
            {
                RejectButton.Click += RejectButton_Click;
            }
        }
        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RejectButton_Click(object sender, EventArgs e)
        {
            SaveWorkflowArgs(false);
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadPassButtonControl()
        {
            if (PassButton == null)
                PassButton = Page.FindControl("ctl00$Body$btnPass") as Button;
            if (PassButton != null)
            {
                PassButton.Click += PassButton_Click;
            }
        }
        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PassButton_Click(object sender, EventArgs e)
        {
            SaveWorkflowArgs(true);
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadTaskRemarkControl()
        {
            if (RemarkControl == null)
                RemarkControl = Page.FindControl("ctl00$Body$txtTaskRemark") as HtmlInputText;
           
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadHistoryPagerControl()
        {
            if (TaskPagerControl == null)
                TaskPagerControl = Page.FindControl("ctl00$Body$pgTask") as PagerControlBase;

        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadLevelControl()
        {
            if (LevelDrowDownList == null)
                LevelDrowDownList = Page.FindControl("ctl00$Body$ddlLevel") as DropDownList;
           
        }
  
        #endregion

        #region 重写保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="isPass"></param>
        protected virtual void SaveWorkflowArgs(bool isPass)
        {
            var info = FillEntity();
            if (info == null) return;
            var args = new WorkflowArgsEntity
            {
                Entity = info,
                TaskId = TaskId,
                AccountId = AccountId,
                IsPass = isPass,
                LevelId = LevelDrowDownList == null ? 0 : LevelDrowDownList.SelectedValue.Convert<long>(),
                Remark = RemarkControl == null ? "" : RemarkControl.Value
            };
            var rev = Ioc.Resolve<IWorkflowEngineApplicationService>().Handle(args);
            SetResult(rev, args.Errors);
        }

        #endregion

        #region  重写加载信息
        /// <summary>
        /// 验证信息
        /// </summary>
        /// <returns></returns>
        public override bool Verify()
        {
            if (Identity != null && base.Verify())
            {
                SetTask();
                if (TaskId > 0 && (Task == null || Task.Account == null || Task.Account.Id != Identity.Id))
                {
                    throw new HttpException(403, "您无权访问该资源");
                }
                return true;
            }
            if (!string.IsNullOrEmpty(Request["mark"]))
            {
                SetTask();
                if (!Ioc.Resolve<IWorkflowEngineApplicationService>().CheckSign(Request.Url.ToString()))
                {
                    throw new HttpException(403, "您无权访问该资源");
                }
                return true;
            }
            return false;
        }

       
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLevel();
            }
            base.Page_Load(sender, e);
        }
 
        /// <summary>
        /// 加载级别
        /// </summary>
        protected virtual void LoadLevel()
        {
            if (LevelDrowDownList == null) return;
            var infos = WorkflowArgs.Engine.GetLevels()?.OrderBy(it => it.Sequence);
            LevelDrowDownList.DataTextField = "Name";
            LevelDrowDownList.DataValueField = "Id";
            LevelDrowDownList.DataSource = infos;
            LevelDrowDownList.DataBind();
         
        }
        /// <summary>
        /// 加载方法
        /// </summary>
        protected override void LoadEntity()
        {
            var info = GetEntity();
            if (info == null && SaveType == SaveType.Modify)
            {
                InvalidateData("您访问的信息不存在或者已经被删除");
            }
            if (info == null) return;
            WorkflowArgs.Entity = info;
            BindEntity(info);
        }

       
        #endregion

        #region 重写SetFindWhere
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="container"></param>
        /// <param name="paging"></param>
        protected override void SetFindQuery(System.Web.UI.Control container, PagerControlBase paging)
        {
            if (paging == TaskPagerControl)
                paging.Query.SetParameter("FlowId", WorkflowArgs.Flow.Id);
            base.SetFindQuery(container, paging);
        }


        #endregion


    }
}
