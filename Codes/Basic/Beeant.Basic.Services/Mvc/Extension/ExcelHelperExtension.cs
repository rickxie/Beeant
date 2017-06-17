using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Beeant.Application.Services;
using Component.Extension;
using Dependent;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.Extension
{
    public static class ExcelHelperExtension
    { 

        /// <summary>
        /// 设置导出格式
        /// </summary>
        private static DataTable SetExcelDataTable(DataTable dt, Dictionary<string, string> items)
        {
            if (items != null)
            {
                var i = 0;
                foreach (KeyValuePair<string, string> item in items)
                {
                    dt.Columns[i].ColumnName = item.Key;
                    dt.Columns[i].Caption = item.Value;
                    i++;
                }
            }
            return dt;
        }
         
        /// <summary>
        /// 输出Excel
        /// </summary>
        public static ActionResult ExportExcel<T>(this Controller controller, string excelName,
            Dictionary<string, string> items, QueryInfo query)
        {
            var dt = Ioc.Resolve<IApplicationService, T>().Execute<DataTable>(query);
            if (dt.Rows.Count > 0)
            {
                dt = SetExcelDataTable(dt, items);
                var file = new FileContentResult(ExcelHelper.ExportExcel(dt),
                    "applicationnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = excelName
                };
                return file;
            }
            return null;
        }
    }
}
