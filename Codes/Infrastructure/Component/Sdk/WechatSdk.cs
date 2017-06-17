using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using Component.Extension;

namespace Component.Sdk
{
    public class WechatSdk
    {
        /// <summary>
        /// 应用Key
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用KEY
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="token"></param>
        public WechatSdk(string appId, string secret, string token)
        {
            AppId = appId;
            Secret = secret;
            Token = token;
        }

        #region 通用

        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        public virtual string GetTicket(string sceneId)
        {
            var accessToken = GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);
            var request = WebRequest.Create(string.Format("{0}{1}", url, accessToken)) as HttpWebRequest;
            if (request == null)
                return null;
            request.Method = "POST";
            var data =
                "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(data);
            request.ContentLength = buf.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);
            }
            var json = GetResponse(request, "UTF-8");
            var dis = DeserializeJson<IDictionary<string, string>>(json);
            if (dis == null || !dis.ContainsKey("ticket"))
                return null;
            return dis["ticket"];
        }

        /// <summary>
        /// 得到token
        /// </summary>
        /// <returns></returns>
        public virtual string GetAccessToken()
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppId,
                    Secret);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            var json = GetResponse(request, "gb2312");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = DeserializeJson<IDictionary<string, string>>(json);
            if (dis == null || !dis.ContainsKey("access_token"))
                return null;
            return dis["access_token"];
        }

        /// <summary>
        /// 得到文件内容
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        protected virtual string GetResponse(WebRequest request, string encoding)
        {
            using (WebResponse response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null) return null;
                using (var reader = new StreamReader(stream, Encoding.GetEncoding(encoding)))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public  string SendPostRequest(string url, IDictionary<string, string> sParaTemp)
        {
            HttpWebRequest request;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            return SendPostRequest(request, Encoding.UTF8, sParaTemp);
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public  string SendPostRequest(string url, Encoding encoding, IDictionary<string, string> sParaTemp)
        {
            return SendPostRequest((HttpWebRequest)WebRequest.Create(url), encoding, sParaTemp);
        }
        /// <summary>
        ///  Post请求
        /// </summary>
        /// <param name="myReq"></param>
        /// <param name="encoding"></param>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public  string SendPostRequest(HttpWebRequest myReq, Encoding encoding, IDictionary<string, string> sParaTemp)
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
            catch
            {

            }
            return null;
        }
        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="input"></param>
        protected virtual T DeserializeJson<T>(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return default(T);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        #endregion

        #region 生成带场景二维码

        /// <summary>
        /// 得到二维码
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public virtual byte[] GetSemacode(string sceneId)
        {
            var ticket = GetTicket(sceneId);
            if (string.IsNullOrEmpty(ticket))
                return null;
            var url = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", ticket);
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return null;
            request.Method = "GET";
            request.Accept = "*/*";
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent =
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            using (WebResponse response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null) return null;
                var ms = new MemoryStream();
                const int bufferLength = 1024;
                int actual;
                var buffer = new byte[bufferLength];
                while ((actual = stream.Read(buffer, 0, bufferLength)) > 0)
                {
                    ms.Write(buffer, 0, actual);
                }
                ms.Position = 0;
                return ms.GetBuffer();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected virtual bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        #endregion

        #region 微信用户

        /// <summary>
        /// 得到微信用户
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetUser(string openId, string accessToken = null)
        {
            if (string.IsNullOrEmpty(openId))
                return null;
            var url = string.Format(
                "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN",
                !string.IsNullOrEmpty(accessToken) ? accessToken : GetAccessToken(),
                openId);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            var json = GetResponse(request, "utf-8");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = json.DeserializeJson<IDictionary<string, object>>();
            if (dis.ContainsKey("errcode"))
                return null;
            return dis;
        }


        /// <summary>
        /// 得到用户列表
        /// </summary>
        /// <param name="nextOpenId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetUsers(string nextOpenId, string accessToken = null)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", !string.IsNullOrEmpty(accessToken) ? accessToken : GetAccessToken());
            if (!string.IsNullOrEmpty(nextOpenId))
            {
                url = string.Format("{0}&next_openid={1}", url, nextOpenId);
            }
            var request = WebRequest.Create(url) as HttpWebRequest;
            var json = GetResponse(request, "utf-8");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = DeserializeJson<IDictionary<string, object>>(json);
            if (dis.ContainsKey("errcode"))
                return null;
            return dis;
           
        }

        /// <summary>
        /// 得到微信用户
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="remark"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> UpdateRemark(string openId,string remark, string accessToken = null)
        {
            if (string.IsNullOrEmpty(openId))
                return null;
            var url = string.Format(
                "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}",
                !string.IsNullOrEmpty(accessToken) ? accessToken : GetAccessToken());
            var request = WebRequest.Create(url);
            request.Method = "POST";
            var builder=new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("\"openid\":\"{0}\",\"remark\":\"{1}\"", openId, remark);
            builder.Append("}");
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(builder.ToString());
            request.ContentLength = buf.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);
            }
            var json = GetResponse(request, "utf-8");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = json.DeserializeJson<IDictionary<string, object>>();
            if (dis.ContainsKey("errmsg") && dis["errmsg"].Convert<string>()== "ok")
                return dis;
            return null;
        }
        #endregion

        #region 发送消息

        /// <summary>
        /// 客服接口发送文本消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual void SendTextMessage(string openid, string text)
        {
            var accessToken = GetAccessToken();
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?";
            var request = WebRequest.Create(string.Format("{0}access_token={1}", url, accessToken));
            request.Method = "POST";
            var data = "{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + text + "\"}}}}}";
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(data);
            request.ContentLength = buf.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);
            }
        }

        /// <summary>
        /// 发送图片信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="mediaId"></param>
        public virtual void SendMediaMessage(string openid, string mediaId)
        {
            var accessToken = GetAccessToken();
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?";
            var request = WebRequest.Create(string.Format("{0}access_token={1}", url, accessToken));
            request.Method = "POST";
            var data = "{\"touser\":\"" + openid + "\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"" + mediaId +
                       "\"}}}}}";
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(data);
            request.ContentLength = buf.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);

            }
        }

        #endregion

        #region 上传临时素材返回Media_Id


        /// <summary> 
        /// 上传媒体返回媒体ID 
        /// </summary> 
        public virtual string UploadMedia(string type, string path)
        {
            string accessToken = GetAccessToken();
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}",
                accessToken, type);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                return null;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线 
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);
            StringBuilder sbHeader =
                new StringBuilder(
                    string.Format(
                        "Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n",
                        fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();
            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response == null)
            {
                return null;
            }
            Stream instream = response.GetResponseStream();
            if (instream == null)
            {
                return null;
            }
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            string content = sr.ReadToEnd();
            return content;
        }

        #endregion 上传临时素材返回Media_Id

        #region 交互

        /// <summary>
        /// 检查是否合法
        /// </summary>
        public virtual bool Check()
        {
            string[] array =
            {
                Token, System.Web.HttpContext.Current.Request.QueryString["timestamp"],
                System.Web.HttpContext.Current.Request.QueryString["Nonce"]
            };
            array = array.OrderBy(it => it).ToArray();
            var builder = new StringBuilder();
            foreach (var s in array)
            {
                builder.Append(s);
            }

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytesSha1In = Encoding.Default.GetBytes(builder.ToString());
            byte[] bytesSha1Out = sha1.ComputeHash(bytesSha1In);
            string rev = BitConverter.ToString(bytesSha1Out);
            if (!string.IsNullOrEmpty(rev))
                rev = rev.Replace("-", "").ToLower();
            return rev == HttpContext.Current.Request.QueryString["Signature"];
        }

        /// <summary>
        /// JS签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetJsSdkSignature(string url)
        {
            string timestamp = DateTime.Now.Ticks.ToString();
            string nonce = Guid.NewGuid().ToString().Replace("-", "");
            string jsapiTicket = GetJsApiTicket();
            var builder = new StringBuilder();
            builder.Append("jsapi_ticket=").Append(jsapiTicket).Append("&")
                .Append("noncestr=").Append(nonce).Append("&")
                .Append("timestamp=").Append(timestamp).Append("&")
                .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytesSha1In = Encoding.Default.GetBytes(builder.ToString());
            byte[] bytesSha1Out = sha1.ComputeHash(bytesSha1In);
            string signature = BitConverter.ToString(bytesSha1Out);
            if (!string.IsNullOrEmpty(signature))
                signature = signature.Replace("-", "").ToLower();
            return new Dictionary<string, string>
            {
                {"jsapi_ticket", jsapiTicket},
                {"signature", signature},
                {"noncestr", nonce},
                {"timestamp", timestamp}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetJsApiTicket()
        {
            var acceccToken = GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi",
                acceccToken);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            var json = GetResponse(request, "gb2312");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = DeserializeJson<IDictionary<string, string>>(json);
            if (dis == null || !dis.ContainsKey("ticket"))
                return null;
            return dis["ticket"];
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <returns></returns>
        public virtual void Echostr()
        {
            var echostr = HttpContext.Current.Request.QueryString["echostr"];
            HttpContext.Current.Response.Write(echostr);
        }

        /// <summary>
        /// 得到XML文档
        /// </summary>
        /// <returns></returns>
        public virtual XmlDocument GetMessage()
        {
            using (Stream source = HttpContext.Current.Request.InputStream)
            {
                var b = new byte[source.Length];
                source.Read(b, 0, (int) source.Length);
                var xml = Encoding.UTF8.GetString(b);
                if (string.IsNullOrEmpty(xml))
                    return null;
                var document = new XmlDocument();
                document.LoadXml(xml);
                return document;
            }
        }

        /// <summary>
        /// 响应
        /// </summary>
        public virtual void Response(IDictionary<string, Action<XmlDocument>> handles)
        {
            if (!Check())
                return;
            if (handles != null)
            {
       
                var doc = GetMessage();
                if (doc != null)
                {
                    var eventNode = doc.SelectSingleNode("xml/Event");
                    var msgTypeNode = doc.SelectSingleNode("xml/MsgType");
                    if (eventNode != null && handles.ContainsKey(eventNode.InnerText))
                    {
                        handles[eventNode.InnerText](doc);
                        return;
                    }
                    if (msgTypeNode != null && handles.ContainsKey(msgTypeNode.InnerText))
                    {
                        handles[msgTypeNode.InnerText](doc);
                        return;
                    }

                }
            }
            Echostr();
        }

        #endregion

        #region 授权

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isBase"></param>
        /// <returns></returns>
        public virtual string CreateAuthorityUrl(string url, bool isBase)
        {
            return
                string.Format(
                    "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state=STATE#wechat_redirect",
                    AppId, HttpContext.Current.Server.UrlEncode(url), isBase ? "snsapi_base" : "snsapi_userinfo");
        }

        /// <summary>
        /// 得到授权的AccessToken
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, string> GetAuthorityAccessToken()
        {
            var code = HttpContext.Current.Request["code"];
            if (string.IsNullOrEmpty(code))
                return null;
            var url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    AppId,Secret, code);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            var json = GetResponse(request, "utf-8");
            if (string.IsNullOrEmpty(json))
                return null;
            var dis = json.DeserializeJson<IDictionary<string, string>>();
            if (dis.ContainsKey("errcode"))
                return null;
            return dis;
        }

        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetAuthorityUser()
        {
            var json = GetAuthorityAccessToken();
            if (json == null || !json.ContainsKey("openid") || !json.ContainsKey("access_token"))
                return null;
            return GetUser(json["openid"]);
        }

        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <returns></returns>
        public virtual string GetAuthorityOpenId()
        {
            var json = GetAuthorityAccessToken();
            if (json == null || !json.ContainsKey("openid"))
                return null;
            return json["openid"];
        }

        #endregion

        #region 检查是否为微信浏览器
        /// <summary>
        /// 检查是否为微信浏览器
        /// </summary>
        /// <returns></returns>
        static public bool CheckWechatBrower()
        {
            var userAgent = HttpContext.Current.Request.ServerVariables["Http_User_Agent"];
            if (userAgent == null || userAgent.ToLower().Contains("micromessenger"))
                return true;
            return false;
        }

        #endregion
    }
}
