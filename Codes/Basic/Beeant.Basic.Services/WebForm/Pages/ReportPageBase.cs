using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class ReportPageBase<T> : SearchPageBase<T>
    {
        #region 具体控件属性
 
        /// <summary>
        /// 统计选项
        /// </summary>
        public virtual CheckBoxList DimensionCheckList { get; set; }

        /// <summary>
        /// 统计选项
        /// </summary>
        public virtual RadioButtonList TypeRadioButtonList { get; set; }
        #endregion

        #region 检查控件

        /// <summary>
        /// 检查GridView和PagerControlBase
        /// </summary>
        protected override void CheckControl()
        {
            base.CheckControl();
            LoadGroupPatternCheckList();
            LoadTypeRadioButtonList();
        }

     
        /// <summary>
        /// 统计方式
        /// </summary>
        protected virtual void LoadGroupPatternCheckList()
        {
            if (DimensionCheckList == null)
            {
                DimensionCheckList = Page.FindControl("ctl00$Body$ckDimension") as CheckBoxList;
            }
        }
        /// <summary>
        /// 统计方式
        /// </summary>
        protected virtual void LoadTypeRadioButtonList()
        {
            if (TypeRadioButtonList == null)
            {
                TypeRadioButtonList = Page.FindControl("ctl00$Body$rdType") as RadioButtonList;
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
            foreach (DataControlField column in GridView.Columns)
            {
                column.Visible = true;
            }
            if (SelectList != null) this.SetGridViewColumnVisible(GridView, SelectList);
            if (DimensionCheckList != null) this.SetGridViewColumnVisible(GridView, DimensionCheckList);
            if (TypeRadioButtonList != null) this.SetGridViewColumnVisible(GridView, TypeRadioButtonList);
        }
     

        #endregion

        #region 设置查询
        protected override void SetQuery(QueryInfo query)
        {
            base.SetQuery(query);
            SetQueryGroupby(query);
            if (string.IsNullOrEmpty(query.SelectExp))
            {
                query.SelectExp = query.GroupByExp;
            }
            else if(!string.IsNullOrEmpty(query.GroupByExp))
            {
                query.SelectExp = string.Format("{0},{1}", query.SelectExp, query.GroupByExp);
            }
        }

     
        /// <summary>
        /// 设置排序
        /// </summary>
        protected override void SetQueryOrderby(QueryInfo query)
        {
            if (OrderbyList != null && !string.IsNullOrEmpty(OrderbyList.SelectedValue))
            {
                base.SetQueryOrderby(query);
            }
            if (string.IsNullOrEmpty(query.OrderByExp) && SelectList != null && SelectList.Items.Count > 0)
                query.OrderByExp = SelectList.Items[0].Value;

        }

        /// <summary>
        /// 设置排序
        /// </summary>
        protected virtual void SetQueryGroupby(QueryInfo query)
        {
            var select = new StringBuilder();
            if (DimensionCheckList != null)
            {
                if (OrderbyList != null && !string.IsNullOrEmpty(OrderbyList.SelectedValue))
                {
                    var item =
                        DimensionCheckList.Items.Cast<ListItem>()
                                             .FirstOrDefault(it => it.Value.Equals(OrderbyList.SelectedValue));
                    if (item != null)
                        item.Selected = true;
                }
                foreach (ListItem item in DimensionCheckList.Items)
                {
                    if (item.Selected) @select.AppendFormat("{0},", item.Value);
                }
               
            }
            if (TypeRadioButtonList != null && !string.IsNullOrEmpty(TypeRadioButtonList.SelectedValue))
            {
                @select.AppendFormat("{0},", TypeRadioButtonList.SelectedValue);
            }
            if (select.Length > 0)
            {
                select.Remove(select.Length - 1, 1);
                query.GroupByExp = select.ToString();
            }
        }

       
        #endregion

        #region 导出Excel
 

 

        /// <summary>
        /// 拼接Excel标题
        /// </summary>
        protected override void HideExcelGridViewColumn()
        {
            var items = new ListItemCollection();
            if (SelectList != null)
                foreach (ListItem item in SelectList.Items)
                    items.Add(item);
            if (DimensionCheckList != null)
                foreach (ListItem item in DimensionCheckList.Items)
                    items.Add(item);
            if (TypeRadioButtonList != null)
                foreach (ListItem item in TypeRadioButtonList.Items)
                    items.Add(item);
            for (int i = 0; i < GridView.Columns.Count; i++)
            {
                GridView.Columns[i].Visible = true;
                if (items.Cast<ListItem>().Any(item => GridView.Columns[i].HeaderText.Equals(item.Text) && !item.Selected))
                {
                    GridView.Columns[i].Visible = false;
                }
                if (GridView.Columns[i].Visible)
                {
                    SetExcelGridViewColumnItemStyle(GridView.Columns[i]);
                }
            }
        }
       
 

        #endregion

    

    }
}
