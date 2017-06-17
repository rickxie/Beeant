using System;
using Winner.Storage;

namespace Beeant.Domain.Entities
{


    public static  class EntityExtension
   {
     
       /// <summary>
       /// 文件名
       /// </summary>
       public static string GetFullFileName(this BaseEntity info, string fileName, string tag)
       {
           if (string.IsNullOrEmpty(fileName))
               return "";
           if (string.IsNullOrEmpty(tag))
               return GetFullFileName(info,fileName);
           return GetFullFileName(info,string.Format("{0}.{1}{2}", fileName, tag, System.IO.Path.GetExtension(fileName)));
       }
        /// <summary>
        /// 文件名
        /// </summary>
        public static string GetFullFileName(this BaseEntity info, string fileName, int width,int height)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";
            return GetFullFileName(info, string.Format("{0}.{1}-{2}{3}", fileName, width, height, System.IO.Path.GetExtension(fileName)));
        }
        /// <summary>
        /// 得到全名
        /// </summary>
        /// <param name="info"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFullFileName(this BaseEntity info, string fileName)
       {
           return Winner.Creator.Get<IFile>().GetFullFileName(fileName);
       }
       /// <summary>
       /// 得到地址
       /// </summary>
       /// <param name="info"></param>
       /// <param name="fileName"></param>
       /// <returns></returns>
       public static string GetDownLoadUrl(this BaseEntity info, string fileName)
       {
           var fullFileName = GetFullFileName(info, fileName);
           var url = DocumentHandler.SignUrl(fullFileName);
           return url;
       }


       /// <summary>
       /// 得到状态名称,返回是或者否
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetStatusName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "StatusName", value);
       }

       /// <summary>
       /// 得到处理名称，返回已处理或者未处理
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetReplayName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "ReplayName", value);
       }

       /// <summary>
       /// 得到状态名称,返回启用或者禁止
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetVerifyName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "VerifyName", value);
       }
       /// <summary>
       /// 得到状态名称,返回展示或者隐藏
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetShowName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "ShowName", value);
       }
       /// <summary>
       /// 得到状态名称,返回展示或者隐藏
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetSalesName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "SalesName", value);
       }
       /// <summary>
       /// 得到状态名称,返回开启或者停止
       /// </summary>
       /// <param name="info"></param>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string GetServiceName(this BaseEntity info, object value)
       {
           return info.GetLanguage(typeof(BaseEntity).FullName, "ServiceName", value);
       }
        /// <summary>
        /// 得到状态名称,返回开启或者停止
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DateTime GetMinDateTime(this BaseEntity info)
        {
            return new DateTime(1800, 1, 1);
        }


    }
}
