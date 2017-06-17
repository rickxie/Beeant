namespace Beeant.Application.Services.Utility
{
    public interface IFileApplicationService
    {
        ///  <summary>
        /// 存储文件,返回存储后的文件路径
        ///  </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        void Save(string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void Remove(string fileName);

     

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] Grab(string fileName);
    }
}
