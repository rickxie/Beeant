using System;
using System.IO;

namespace Beeant.Tool.Copy
{
    class Program
    {
        private static void Main(string[] args)
        {
            var isCreateScripts = true;
            if (args.Length >= 2 && args[1] == "false")
            {
                isCreateScripts = false;
            }
            Replace(args[0], isCreateScripts);
        }

        /// <summary>
        /// 得到文件
        /// </summary>
        /// <param name="path"></param>
        private static void Replace(string path, bool isCreateScripts)
        {
            var rootPath = GetRootPath();
            ReplaceFiles(string.Format(@"{0}\Infrastructure\Configuration\Config", rootPath),
                string.Format(@"{0}Config", path));
            if (isCreateScripts)
            {
                ReplaceFiles(string.Format(@"{0}\Infrastructure\Resource\Scripts", rootPath),
                    string.Format(@"{0}Scripts", path));
            }
        }

        /// <summary>
        /// 得到根目录
        /// </summary>
        /// <returns></returns>
        private static string GetRootPath()
        {
            var tag = @"\Beeant\Codes";
            var path = Directory.GetCurrentDirectory();
           var index= path.IndexOf(tag);
            if (index == -1)
                return null;
            return path.Substring(0, index + tag.Length);
        }

        /// <summary>
        /// 得到文件
        /// </summary>
        /// <param name="orgPath"></param>
        /// <param name="desPath"></param>
        private static void ReplaceFiles(string orgPath, string desPath)
        {
            if (!Directory.Exists(desPath))
                Directory.CreateDirectory(desPath);
            var orgDirectory=new DirectoryInfo(orgPath);
            var files = orgDirectory.GetFiles();
            foreach (var file in files)
            {
                var desFileName = string.Format(@"{0}\{1}", desPath, file.Name);
                var desFile=new FileInfo(desFileName);
                if (!desFile.Exists || file.LastWriteTime != desFile.LastWriteTime)
                    file.CopyTo(desFileName, desFile.Exists);
            }
            var directories = orgDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                ReplaceFiles(string.Format(@"{0}\{1}", orgPath, directory.Name),
                    string.Format(@"{0}\{1}", desPath, directory.Name));
            }
        }
    }
}
