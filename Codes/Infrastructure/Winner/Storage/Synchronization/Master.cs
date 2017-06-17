using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Winner.Wcf;

namespace Winner.Storage.Synchronization
{
    public class Master :IMaster
    {
      
        #region 属性
        /// <summary>
        /// 服务实例
        /// </summary>
        public IWcfService WcfService { get; set; }

      
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Master()
        {
        
        }

        /// <summary>
        /// WCF客户端配置文件，节点集合，获取节点超时时间
        /// </summary>
        /// <param name="wcfService"></param>
        public Master(IWcfService wcfService)
        {
            WcfService = wcfService;
  
        }
        #endregion

        #region 接口的实现

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(IList<string> endpoints, string fileName, byte[] fileByte)
        {
            if (string.IsNullOrEmpty(fileName) || fileByte == null) return;
            var endPointNames = new List<string>();
            foreach (var endpoint in endpoints)
            {
                try
                {
                    WcfService.Invoke<IFileContract>(new List<EndPointInfo> { new EndPointInfo { Name = endpoint } }, SaveFiles, null, fileName,
                                                    fileByte);
                }
                catch 
                {
                    endPointNames.Add(endpoint);
                }
            }
            if(endPointNames.Count>0)
                AddSaveException(fileName, endPointNames);

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void Remove(IList<string> endpoints, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))return;
            var endPointNames = new List<string>();
            foreach (var endpoint in endpoints)
            {
                try
                {
                    WcfService.Invoke<IFileContract>(new List<EndPointInfo> { new EndPointInfo { Name = endpoint } }, RemoveFiles, null, fileName);
                }
                catch 
                {
                    endPointNames.Add(endpoint);
                }
            }
            if (endPointNames.Count > 0)
                AddSaveException(fileName, endPointNames);
        }

       
        /// <summary>
        /// 得到最新文件目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        protected virtual DirectoryInfo GetLastDirectory(DirectoryInfo directory)
        {
            var subDirectory = directory.GetDirectories().OrderByDescending(it => it.CreationTime).FirstOrDefault();
            if (subDirectory == null || !subDirectory.Exists)
                return directory;
            return GetLastDirectory(subDirectory);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object SaveFiles(IFileContract fileService, params object[] paramters)
        {
            fileService.Save(null, paramters[0] as string, paramters[1] as byte[]);
            return null;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object RemoveFiles(IFileContract fileService, params object[] paramters)
        {
            fileService.Remove(null, paramters[0] as string);
            return null;
           
        }
        #region 异常处理
        /// <summary>
        /// 执行时间间隔
        /// </summary>
        public double TimmerInterval { get; set; }

        /// <summary>
        /// 定时器
        /// </summary>
        public static Timer Timer { get; set; } = new Timer();

        /// <summary>
        /// 开启异常处理
        /// </summary>
        public void StartException()
        {
            Timer.Enabled = true;
            Timer.Interval = TimmerInterval > 0 ? TimmerInterval : 60000;
            Timer.Elapsed += timmer_Elapsed;
            Timer.Start(); 
        }

        /// <summary>
        /// 开启时间间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void timmer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var savePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, SaveExceptionPath);
            var savedi = new DirectoryInfo(savePath);
            if (!savedi.Exists)
                savedi.Create();
            var removePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, RemoveExceptionPath);
            var removedi = new DirectoryInfo(removePath);
            if (!removedi.Exists)
                removedi.Create();
            StartSaveExceptionHandle();
            StartRemoveExceptionHandle();
        }



        private string _saveExceptionPath = @"Exception\Save\";
        /// <summary>
        /// 异常路径
        /// </summary>
        public string SaveExceptionPath
        {
            get { return _saveExceptionPath; }
            set { _saveExceptionPath = value; }
        }
        private string _removeExceptionPath = @"Exception\Remove\";
        /// <summary>
        /// 异常路径
        /// </summary>
        public string RemoveExceptionPath
        {
            get { return _removeExceptionPath; }
            set { _removeExceptionPath = value; }
        }
        private static readonly object SaveLocker=new object();

        /// <summary>
        /// 开启异常处理
        /// </summary>
        protected virtual void StartSaveExceptionHandle()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, SaveExceptionPath);
            var di = new DirectoryInfo(path);
            if (!di.Exists) return;
            lock (SaveLocker)
            {
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    byte[] fileByte;
                    var fileName = Path.GetFileName(file.FullName).Replace("%", "/").Replace(".txt", "");
                    var fullfileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, fileName);
                    using (var filestream = new FileStream(fullfileName, FileMode.Open))
                    {
                        fileByte = new byte[filestream.Length];
                        filestream.Read(fileByte, 0, fileByte.Length);
                    }
                    var endpoits = File.ReadAllLines(file.FullName);
                    var endPointNames = new List<string>();

                    foreach (var endpoit in endpoits)
                    {
                        try
                        {

                            WcfService.Invoke<IFileContract>(new List<EndPointInfo> {new EndPointInfo {Name = endpoit}},
                                                             SaveFiles, fileName, fileByte);
                        }
                        catch 
                        {
                            endPointNames.Add(endpoit);
                        }
                    }
                    if (endPointNames.Count > 0)
                    {
                        AddSaveException(fileName, endPointNames);
                    }
                    else
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
        }
        private static readonly object RemoveLocker = new object();
        /// <summary>
        /// 开启异常处理
        /// </summary>
        protected virtual void StartRemoveExceptionHandle()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, RemoveExceptionPath);
            var di = new DirectoryInfo(path);
            if (!di.Exists) return;
            lock (RemoveLocker)
            {
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    var endpoits = File.ReadAllLines(file.FullName);
                    var endPointNames = new List<string>();
                    var fileName = Path.GetFileName(file.FullName).Replace("%", "/").Replace(".txt", "");
                    foreach (var endpoit in endpoits)
                    {
                        try
                        {

                            WcfService.Invoke<IFileContract>(
                                new List<EndPointInfo> {new EndPointInfo {Name = endpoit}}, RemoveFiles, fileName);
                        }
                        catch 
                        {
                            endPointNames.Add(endpoit);
                        }
                    }
                    if (endPointNames.Count > 0)
                    {
                        AddRemoveException(fileName, endPointNames);
                    }
                    else
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
        }
        /// <summary>
        /// 添加异常信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="endPointNames"></param>
        protected virtual void AddSaveException(string fileName, IList<string> endPointNames)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                    string.Format("{0}{1}.txt", SaveExceptionPath, fileName.Replace("/", "%")));
            File.WriteAllLines(fileName, endPointNames);
        }

        /// <summary>
        /// 添加异常信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="endPointNames"></param>
        protected virtual void AddRemoveException(string fileName, IList<string> endPointNames)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                    string.Format("{0}{1}.txt", RemoveExceptionPath, fileName.Replace("/", "%")));
            File.WriteAllLines(fileName, endPointNames);
        }
        #endregion

        #endregion




    }
}
