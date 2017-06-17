using System.IO;

namespace Component.Extension
{
    static public class DirectoryExtension
    {
        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="info"></param>
        /// <param name="path"></param>
        public static void Copy(this DirectoryInfo info, string path)
        {
            CopyDirectory(info.FullName, path);
        }
        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="deskName"></param>
        private static void CopyDirectory(string sourceName, string deskName)
        {
            string[] fileNames = Directory.GetFiles(sourceName);
            string[] directoryNames = Directory.GetDirectories(sourceName);
            if (!Directory.Exists(deskName))
                Directory.CreateDirectory(deskName);
            foreach (string fileName in fileNames)
            {
                string tFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                File.Copy(fileName, deskName + "\\" + tFileName, true);
            }
            if (directoryNames.Length == 0)
                return;
            foreach (string directoryName in directoryNames)
            {
                string tDirectoryName = directoryName.Substring(directoryName.LastIndexOf("\\") + 1);
                CopyDirectory(directoryName, deskName + "\\" + tDirectoryName);
            }
        }
    }
}
