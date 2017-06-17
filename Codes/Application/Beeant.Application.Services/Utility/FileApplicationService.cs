using Beeant.Domain.Services.Utility;


namespace Beeant.Application.Services.Utility
{
    public class FileApplicationService : IFileApplicationService
    {
        public IFileRepository FileRepository { get; set; }

       

        ///  <summary>
        /// 存储文件,返回存储后的文件路径
        ///  </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(string fileName, byte[] fileByte)
        {
             FileRepository.Save(fileName, fileByte);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void Remove(string fileName)
        {
            FileRepository.Remove(fileName);
        }

  

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual byte[] Grab(string fileName)
        {
          
            return FileRepository.Download(fileName);
        }
         
    }
}
