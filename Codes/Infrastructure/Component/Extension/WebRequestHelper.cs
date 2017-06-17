using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Component.Extension
{
    static public class WebRequestHelper
    {
        #region 发送POST

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public static string SendPostRequest(string url, IDictionary<string, string> sParaTemp)
        {
            return SendPostRequest((HttpWebRequest)WebRequest.Create(url), Encoding.UTF8, sParaTemp);
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public static string SendPostRequest(string url, Encoding encoding, IDictionary<string, string> sParaTemp)
        {
           return SendPostRequest((HttpWebRequest) WebRequest.Create(url), encoding, sParaTemp);
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="myReq"></param>
        /// <param name="encoding"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public static string SendPostRequest(HttpWebRequest myReq, Encoding encoding, IDictionary<string, string> sParaTemp)
        {
            var code = encoding;
            var sPara = new StringBuilder();
            if (sParaTemp != null)
            {
                foreach (var val in sParaTemp)
                {
                    sPara.AppendFormat("{0}={1}&", val.Key, val.Value);
                }
                sPara.Remove(sPara.Length - 1, 1);
            }

            string strRequestData = sPara.ToString();
            byte[] bytesRequestData = code.GetBytes(strRequestData);
            try
            {
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.ContentLength = bytesRequestData.Length;
                var requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                var httpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = httpWResp.GetResponseStream();
                if (myStream == null)
                    return null;
                var reader = new StreamReader(myStream, code);
                var result = reader.ReadToEnd();
                myStream.Close();
                return result;
            }
            catch(Exception ex)
            {

            }
            return null;
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendPostRequest(string url, Encoding encoding, string content)
        {
            return SendPostRequest((HttpWebRequest)WebRequest.Create(url), Encoding.UTF8, content);
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="myReq"></param>
        /// <param name="encoding"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendPostRequest(HttpWebRequest myReq, Encoding encoding, string content)
        {
            var code = encoding;
            byte[] bytesRequestData = code.GetBytes(content);
            try
            {
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.ContentLength = bytesRequestData.Length;
                var requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                var httpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = httpWResp.GetResponseStream();
                if (myStream == null)
                    return null;
                var reader = new StreamReader(myStream, code);
                var result = reader.ReadToEnd();
                myStream.Close();
                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion

        #region 证书服务
        /// <summary>
        /// 创建带证书设置的httpwebrequest
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="issuerName">颁发者名称</param>
        /// <param name="friendlyName">友好名称</param>
        /// <param name="validOnly">仅限有效证书</param>
        /// <returns></returns>
        public static WebRequest CreateWebRequestWithCertificate(string url, string issuerName, string friendlyName,
            bool validOnly)
        {

            HttpWebRequest request = null;
            var cert = CreateX509Certificate(null, null, issuerName, friendlyName, validOnly);
            if (cert != null)
            {
                request = (HttpWebRequest) WebRequest.Create(url);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //request.ProtocolVersion = HttpVersion.Version10;
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback((a, b, c, d) => true);
                request.ClientCertificates.Add(cert);
            }

            return request;
        }
        /// <summary>
        /// 创建带证书设置的httpwebrequest
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="certFileName">证书名全路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <returns></returns>
        public static WebRequest CreateWebRequestWithCertificate(string url, string certFileName, string certPassword)
        {

            HttpWebRequest request = null;
            var cert = CreateX509Certificate(certFileName, certPassword, null, null);
            if (cert != null)
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //request.ProtocolVersion = HttpVersion.Version10;
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback((a, b, c, d) => true);
                request.ClientCertificates.Add(cert);
            }

            return request;
        }
        /// <summary>
        /// 获取指定证书
        /// </summary>
        /// <param name="issuerName">颁发者名称</param>
        /// <param name="friendlyName">友好名称</param>
        /// <param name="validOnly">仅限有效证书</param>
        /// <returns></returns>
        public static X509Certificate CreateX509Certificate(string issuerName, string friendlyName, bool validOnly)
        {
            return CreateX509Certificate(null, null, issuerName, friendlyName, validOnly);
        }
        /// <summary>
        /// 获取指定证书
        /// </summary>
        /// <param name="certFileName">证书名全路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <returns></returns>
        public static X509Certificate CreateX509Certificate(string certFileName, string certPassword)
        {
            return CreateX509Certificate(certFileName, certPassword, null, null);
        }

        /// <summary>
        /// 获取指定证书
        /// </summary>
        /// <param name="certFileName">证书名全路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <param name="issuerName">颁发者名称</param>
        /// <param name="friendlyName">友好名称</param>
        /// <param name="validOnly">仅限有效证书</param>
        /// <returns></returns>
        private static X509Certificate CreateX509Certificate(string certFileName, string certPassword, string issuerName,
            string friendlyName, bool validOnly = true)
        {
            X509Certificate cert = null;
            if (!string.IsNullOrEmpty(friendlyName))
            {
                var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                try
                {
                    certStore.Open(OpenFlags.ReadOnly);
                    //证书需导入 本地计算机-》个人目录 并同时导入 受信任的根证书颁发机构。 设置权限
                    foreach (
                        var cer in
                            (string.IsNullOrEmpty(issuerName)
                                ? certStore.Certificates
                                : certStore.Certificates.Find(X509FindType.FindByIssuerName, issuerName, validOnly))
                        )
                    {
                        //throw new Exception(cer.FriendlyName);
                        if (cer.FriendlyName != friendlyName) continue;
                        cert = cer;
                        break;
                    }

                }
                finally
                {
                    certStore.Close();
                }
            }

            //没有获取到证书，则从文件创建
            if (cert == null && !string.IsNullOrEmpty(certFileName))
            {

                try
                {
                    var password = GetSecureString(certPassword);
                    cert = password == null
                        ? new X509Certificate(certFileName)
                        : new X509Certificate(certFileName, password);
                }
                catch
                {
                    // ignored
                }
            }
            return cert;
        }

        /// <summary>
        /// 转换成SecureString
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private static SecureString GetSecureString(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            // Instantiate the secure string.
            var pasSecureString = new SecureString();
            // Use the AppendChar method to add each char value to the secure string.
            foreach (var ch in password)
                pasSecureString.AppendChar(ch);
            return pasSecureString;
        }
        #endregion
    }
}
