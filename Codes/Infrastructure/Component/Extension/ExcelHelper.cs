using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace Component.Extension
{
    public static class ExcelHelper
    {
        static private readonly object CreateHtmlFileLocker = new object();
        static private readonly object ConvertToPdfLocker = new object();
        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="id"></param>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int id);

        public static void CreateHtmlFile(string sourcePath, string descPath)
        {
            Application application = new ApplicationClass();
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(descPath)) return;
            _Workbook xls = null;
            object o = System.Reflection.Missing.Value;
            object format = XlFileFormat.xlHtml;//Html
            lock (CreateHtmlFileLocker)
            {
                try
                {
                    application.Visible = false;
                    application.DisplayAlerts = false;
                    xls = application.Workbooks.Open(sourcePath, o, true, o, o, o, o, o, o, o, o, o, o, o, o);
                    xls.SaveAs(descPath, format, o, o, o, o, XlSaveAsAccessMode.xlExclusive, o, o, o, o, o);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (xls != null)
                    {
                        xls.Close();
                    }
                    ReleaseComObject(o);
                    ReleaseComObject(format);
                    ReleaseComObject(xls);
                    application.Quit();
                    ReleaseComObject(application);
                    GC.Collect();
                }
            }
        }
        /// <summary>
        /// 释放Com
        /// </summary>
        /// <param name="o"></param>
        private static void ReleaseComObject(object o)
        {
            try
            {
                Marshal.ReleaseComObject(o);
            }
            catch { }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// 转换Pdf
        /// </summary>
        /// <param name="strSourceFile"></param>
        /// <param name="strTargetFile"></param>
        /// <returns></returns>
        public static bool ConvertToPdf(string strSourceFile, string strTargetFile)
        {
            if (!File.Exists(strSourceFile))
            {
                return false;
            }
            lock (ConvertToPdfLocker)
            {
                ApplicationClass application = null;
                Workbook workBook = null;
                Worksheet sheet = null;
                object missing = Type.Missing;
                try
                {
                    var targetType = XlFixedFormatType.xlTypePDF;
                    object targetFile = strTargetFile;
                    application = new ApplicationClass { Visible = false, DisplayAlerts = false };
                    workBook = application.Workbooks.Open(strSourceFile, missing, missing, missing, missing, missing,
                                                          missing, missing, missing, missing, missing, missing, missing,
                                                          missing, missing);
                    sheet = (Worksheet)workBook.Worksheets[1];
                    sheet.PageSetup.Orientation = XlPageOrientation.xlPortrait;
                    sheet.ExportAsFixedFormat(targetType, targetFile, XlFixedFormatQuality.xlQualityStandard, true,
                                              false,
                                              missing, missing, missing, missing);
                    return true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    if (workBook != null)
                    {
                        workBook.Close(true, missing, missing);
                    }
                    ReleaseComObject(missing);
                    ReleaseComObject(sheet);
                    ReleaseComObject(workBook);
                    if (application != null)
                    {
                        application.Quit();
                    }
                    ReleaseComObject(application);
                    GC.Collect();

                }
            }
        }
        #region 保存数据列表到
        /// <summary>
        /// 保存数据列表到Excel（泛型）
        /// </summary>
        /// <param name="stream">数据列表</param>
        public static byte[] ExportExcel(Stream stream)
        {

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    package.Load(stream);
                    return package.GetAsByteArray();
                    //var worksheet = package.Workbook.Worksheets.Add("sheet1");
                    //package.Load();
                    //worksheet.Cells.Style.ShrinkToFit = true;
                    //int colIndex = 0;
                    //输出标题
                    //for (int i = 0; i < gv.Columns.Count; i++)
                    //{
                    //    if (gv.Columns[i].Visible && gv.Columns[i].HeaderText.Length > 0)
                    //    {
                    //        worksheet.Cells[1, ++colIndex].Value = gv.Columns[i].HeaderText;
                    //        worksheet.Cells[1, colIndex].Style.Font.Bold = true;
                    //        worksheet.Cells[1, colIndex].Style.Font.Size = 12;
                    //        worksheet.Cells[1, colIndex].Style.Font.Name = "黑体";
                    //    }
                    //}

                    ////输出内容
                    //for (int i = 0; i < gv.Rows.Count; i++)
                    //{
                    //    colIndex = 0;
                    //    for (int j = 0; j < gv.Columns.Count; j++)
                    //    {
                    //        if(!gv.Columns[j].Visible)
                    //            continue;
                    //        var text = gv.Rows[i].Cells[j].Text;
                    //        if (gv.Rows[i].Cells[j].Controls.Count > 0)
                    //        {
                    //            text = ((System.Web.UI.DataBoundLiteralControl)gv.Rows[i].Cells[j].Controls[0]).Text;
                    //        }
                    //        worksheet.Cells[i + 2, ++colIndex].Value = text.Replace("&nbsp;", "").Replace("\r\n", "").Replace("\t", "").Replace("  ", "");
                    //        worksheet.Cells[i + 2, colIndex].Style.Font.Size = 11;
                    //        worksheet.Cells[i + 2, colIndex].Style.Font.Name = "黑体";
                    //    }
                    //}

                    //worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 保存数据列表到Excel（泛型）
        /// </summary>
        /// <param name="infos">数据列表</param>
        public static byte[] ExportExcel<T>(IEnumerable<T> infos)
        {

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("sheet1");
                    worksheet.Cells.LoadFromCollection(infos);
                    return package.GetAsByteArray();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 保存数据列表到Excel（泛型）
        /// </summary>
        /// <param name="dt">数据列表</param>
        public static byte[] ExportExcel(System.Data.DataTable dt)
        {

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("sheet1");
                    worksheet.Cells.LoadFromDataTable(dt, true);
                    return package.GetAsByteArray();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        #endregion
    }
}
