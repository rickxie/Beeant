using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Base;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class ListPageBase<T> : SearchPageBase<T> where T : BaseEntity, new()
    {
        #region 具体控件属性

  
    
        /// <summary>
        /// 删除按钮
        /// </summary>
        public virtual Button RemoveButton { get; set; }

        public virtual HtmlInputHidden ItemsValueControl { get; set; }
     
        private string _selectName = "ckSelect";
        /// <summary>
        /// GirdView的选择名称
        /// </summary>
        public virtual string SelectName
        {
            get { return _selectName; }
            set { _selectName = value; }
        }
        #endregion

        #region 检查控件
        /// <summary>
        /// 检查GridView和PagerControlBase
        /// </summary>
        protected override void CheckControl()
        {
            base.CheckControl();
            LoadRemoveButton();
        }
        /// <summary>
        /// 加载删除控件
        /// </summary>
        protected virtual void LoadRemoveButton()
        {
            if (RemoveButton == null)
                RemoveButton = Page.FindControl("ctl00$Body$btnRemove") as Button;
            if (RemoveButton == null) return;
            RemoveButton.Click += Remove_Click;
            RemoveButton.Attributes.Add("ConfirmBox", "Remove");
            RemoveButton.Attributes.Add("ConfirmMessage", "您确定要删除吗");
            RemoveButton.Attributes.Add("ComfirmCheckBoxMessage", "你没有选择任何行");
           
        }

        #endregion

        #region 操作结果处理

        /// <summary>
        /// 操作结果处理
        /// </summary>
        /// <param name="rev"></param>
        /// <param name="infos"></param>
        /// <param name="sucessfulMessage"></param>
        /// <param name="failureMessage"></param>
        protected virtual void SetReusltMessage<TEntityType>(bool rev, IList<TEntityType> infos, string sucessfulMessage, string failureMessage)
            where TEntityType : BaseEntity, new()
        {
            if (rev)
            {
                this.ShowMessage("操作提醒", sucessfulMessage);
                LoadData();
                return;
            }
            this.ShowMessage(failureMessage, GetFailResultMessage(infos));
        }

        /// <summary>
        /// 显示失败信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual string GetFailResultMessage<TEntityType>(IList<TEntityType> infos) where TEntityType : BaseEntity, new()
        {
            var mess = new StringBuilder();
            int rowIndex = Pager == null ? 1 : Pager.PageIndex * Pager.PageSize+1;
            foreach (var info in infos)
            {
                AppendFailResultMessage(mess, info, rowIndex);
                rowIndex++;
            }
            return mess.ToString();
        }

        /// <summary>
        /// 添加单条出错信息
        /// </summary>
        /// <param name="mess"></param>
        /// <param name="info"></param>
        /// <param name="rowIndex"></param>
        protected virtual void AppendFailResultMessage<TEntityType>(StringBuilder mess, TEntityType info, int rowIndex) where TEntityType : BaseEntity, new()
        {
            if (info==null || info.Errors==null || info.Errors.Count == 0)
                return;
            mess.AppendFormat("<div class='one'><div class='subject'>您选择第{0}记录失败原因</div>", rowIndex);
            int i = 1;
            foreach (var error in info.Errors)
            {
                mess.AppendFormat("<div class='detail'>{0}.{1}</div>", i++, error.Message);
            }
            mess.Append("</div>");
        }


        #endregion

        #region 得到操作对象

        /// <summary>
        /// 得到存储对象集合
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="isBindDataKey"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected virtual IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
            where TEntityType : BaseEntity, new()
        {
            if (GridView == null) return null;
            return (from GridViewRow gridViewRow in GridView.Rows
                    where gridViewRow.RowType == DataControlRowType.DataRow
                    select GetSaveEntity<TEntityType>(gridViewRow, saveType, isBindDataKey,dropDownList) into info
                    where info != null
                    select info).ToList();
        }

        /// <summary>
        /// 得到存储对象
        /// </summary>
        /// <param name="gridViewRow"></param>
        /// <param name="saveType"></param>
        /// <param name="isBindDataKey"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected virtual TEntityType GetSaveEntity<TEntityType>(GridViewRow gridViewRow, SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
                   where TEntityType : BaseEntity, new()
        {
            var ckSelect = gridViewRow.FindControl(SelectName) as HtmlInputCheckBox;
            if (ckSelect == null || !ckSelect.Checked) return null;
            var id = GridView.DataKeyNames.Contains("Id")
                         ? GridView.DataKeys[gridViewRow.RowIndex]["Id"].Convert<long>()
                         : ckSelect.Value.Convert<long>();
            var info = CreateSaveEntity<TEntityType>(id, saveType);
            if (dropDownList != null && dropDownList.SelectedIndex != 0)
            {
                Winner.Creator.Get<IProperty>().SetValue(info, dropDownList.GetAttributeValue("SaveName"), dropDownList.SelectedValue);
                info.SetProperty(dropDownList.GetAttributeValue("SaveName"));
            }
            if (isBindDataKey) 
                FillSaveEntityDateKey(info,gridViewRow);
            return info;
        }
        /// <summary>
        /// 创建存储对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        protected virtual TEntityType CreateSaveEntity<TEntityType>(long id, SaveType saveType)
            where TEntityType : BaseEntity, new()
        {
            return new TEntityType { Id = id, SaveType = saveType };
        }

        /// <summary>
        /// 填充其它字段
        /// </summary>
        /// <param name="info"></param>
        /// <param name="gridViewRow"></param>
        protected virtual void FillSaveEntityDateKey<TEntityType>(TEntityType info, GridViewRow gridViewRow)
              where TEntityType : BaseEntity, new()
        {
            if(gridViewRow.RowType!=DataControlRowType.DataRow) return;
            foreach (string t in GridView.DataKeyNames)
            {
                if (GridView.DataKeys[gridViewRow.RowIndex] == null) continue;
                Winner.Creator.Get<IProperty>().SetValue(info, t, GridView.DataKeys[gridViewRow.RowIndex][t]);
            }
        }
     


        #endregion

        #region 设置girdview

        /// <summary>
        /// 设置gridview显示的列
        /// </summary>
        protected override void SetGridViewColumn()
        {
            if (GridView == null) return;
            base.SetGridViewColumn();
            if (SelectList != null) this.SetGridViewColumnVisible(GridView, SelectList);
          
        }
       
        #endregion

        #region 存储对象

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="sucessfulMessage"></param>
        /// <param name="failureMessage"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected virtual bool SaveEntities(SaveType saveType, string sucessfulMessage, string failureMessage, DropDownList dropDownList = null)
        {
            var infos = GetSaveEntities<T>(saveType,true,dropDownList);
            return SaveEntities(infos, sucessfulMessage, failureMessage,dropDownList);
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="sucessfulMessage"></param>
        /// <param name="failureMessage"></param>
        /// <param name="dropDownList"></param>
        protected virtual bool SaveEntities<TEntityType>(IList<TEntityType> infos, string sucessfulMessage, string failureMessage, DropDownList dropDownList = null)
            where TEntityType : BaseEntity, new()
        {
            if (!CheckSaveEntities(infos,dropDownList)) return false;
            var rev = Ioc.Resolve<IApplicationService,TEntityType>().Save(infos);
            SetReusltMessage(rev, infos, sucessfulMessage, failureMessage);
            return rev;
        }

        /// <summary>
        /// 检查操作对象
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="dropDownList"></param>
        /// <returns></returns>
        protected virtual bool CheckSaveEntities<TEntityType>(IList<TEntityType> infos, DropDownList dropDownList = null)
               where TEntityType : BaseEntity, new()
        {
            if (dropDownList!=null && dropDownList.SelectedIndex == 0)
            {
                this.ShowMessage("操作提醒", dropDownList.GetAttributeValue("ComfirmDropdownListMessage"));
                return false;
            }
            if (infos == null || infos.Count == 0)
            {
                this.ShowMessage("操作提醒", "你没有选择任何行");
                return false;
            }
            return true;
        }

        #endregion

        #region 删除事件

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Remove_Click(object sender, EventArgs e)
        {
            SaveEntities(SaveType.Remove, "删除成功", "删除失败");
        }
        #endregion

        #region 导出Excel
     
 

        /// <summary>
        /// 拼接Excel标题
        /// </summary>
        protected override void HideExcelGridViewColumn()
        {
            if (SelectList == null) return;
            for (int i = 0; i < GridView.Columns.Count; i++)
            {
                GridView.Columns[i].Visible = false;
                if (SelectList.Items.Cast<ListItem>().Any(item => GridView.Columns[i].HeaderText.Equals(item.Text) && item.Selected))
                {
                    GridView.Columns[i].Visible = true;
                    SetExcelGridViewColumnItemStyle(GridView.Columns[i]);
                }
            }
        }
      

        #endregion

        protected virtual IList<TItem> GetItems<TItem>() where TItem : BaseEntity, new()
        {
            var info = this.GetItemEntities<TItem>(ItemsValueControl.Value, SaveType.Add);
            return info;
        }

    }
}
