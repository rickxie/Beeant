
namespace Winner.Base
{

    public interface ISecurity
    {
        /// <summary>
        /// 得到Sha1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       string EncryptSha1(string input);
        /// <summary>
        /// 得到MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string EncryptMd5(string input);

        /// <summary>
        /// 得到3DES
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string Encrypt3Des(string input, string key);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string Decrypt3Des(string input, string key);
        /// <summary>
        /// 得到公钥密码
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        void GetRsaKeys(out string privateKey, out string publicKey);
        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        string EncryptRsa(string input, string privateKey, string publicKey);
        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pollCode"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        bool DecryptRsa(string input, string pollCode, string publicKey);
        /// <summary>
        /// 得到3DES
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string EncryptSign(string input);
    }
}
