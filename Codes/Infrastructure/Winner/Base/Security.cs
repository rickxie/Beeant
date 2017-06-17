using System;
using System.Text;
using System.Security.Cryptography;

namespace Winner.Base
{
    public class Security : ISecurity
    {


        #region 接口的实现
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string EncryptSha1(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytesSha1In = Encoding.Default.GetBytes(input);
            byte[] bytesSha1Out = sha1.ComputeHash(bytesSha1In);
            string strSha1Out = BitConverter.ToString(bytesSha1Out);
            return strSha1Out;

        }
        /// <summary>
        /// 得到MD5加密
        /// </summary>
        /// <returns></returns>
        public virtual string EncryptMd5(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(input);
            byte[] bytHash = md5.ComputeHash(bytValue);
            var sTemp = new StringBuilder();
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp.Append(bytHash[i].ToString("X").PadLeft(2, '0'));
            }
            return sTemp.ToString().ToLower();
        }

        /// <summary>
        /// 得到3DES
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string Encrypt3Des(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(key), Mode = CipherMode.ECB
                };
            var desEncrypt = des.CreateEncryptor();
            byte[] buffer = Encoding.ASCII.GetBytes(input);
            return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        /// <summary>
        /// 得到3DES
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string Decrypt3Des(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
            var desDecrypt = des.CreateDecryptor();
            string result = "";
            try
            {
                byte[] buffer = Convert.FromBase64String(input);
                result = Encoding.ASCII.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {

            }
            return result;
        }
        /// <summary>
        /// 得到公钥和秘钥
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public virtual void GetRsaKeys(out string privateKey, out string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }
        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public virtual string EncryptRsa(string input, string privateKey, string publicKey)
        {
            if (string.IsNullOrEmpty(input)) return input;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                var f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] source = Encoding.ASCII.GetBytes(input);
                var sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);
                byte[] b = f.CreateSignature(result);
                return Convert.ToBase64String(b);
            }
        }
        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pollCode"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public virtual bool DecryptRsa(string input, string pollCode, string publicKey)
        {
            if (string.IsNullOrEmpty(input)) return false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                var f = new RSAPKCS1SignatureDeformatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] key = Convert.FromBase64String(pollCode);
                var sha = new SHA1Managed();
                byte[] name = sha.ComputeHash(Encoding.ASCII.GetBytes(input));
                return f.VerifySignature(name, key);
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string EncryptSign(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 10)
                return null;
            input = input.Remove(0, 1);
            input.Insert(3, input.Substring(2, 3));
            var mark = EncryptMd5(input);
            return mark.Remove(0, 1).Insert(0, mark[15].ToString());
        }

        #endregion


    }
}
