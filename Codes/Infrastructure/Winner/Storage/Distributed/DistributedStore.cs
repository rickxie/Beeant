using System;
using System.Collections.Generic;
using System.Linq;
using Winner.Storage.Address;
using Winner.Storage.Route;
using Winner.Wcf;

namespace Winner.Storage.Distributed
{
    public class DistributedStore :IFile
    {
      
        #region 属性
        /// <summary>
        /// 服务实例
        /// </summary>
        public IWcfService WcfService { get; set; }
        private IList<DataServiceGroupInfo>  _dataServiceGroups = new List<DataServiceGroupInfo> ();

        /// <summary>
        /// 存储
        /// </summary>
        public IList<DataServiceGroupInfo> DataServiceGroups
        {
            get { return _dataServiceGroups; }
            set { _dataServiceGroups = value; }
        }
        /// <summary>
        /// 文件路由
        /// </summary>
        public IFileRoute FileRoute { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public IAddress Address { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public DistributedStore()
        { 
        }

        /// <summary>
        /// WCF客户端配置文件，节点集合，获取节点超时时间
        /// </summary>
        /// <param name="wcfService"></param>
        public DistributedStore(IWcfService wcfService)
        {
            WcfService = wcfService;
        }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual string GetFullFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;
            var hashValue = GetHashValue();
            var dataServiceGroup = GetDataServiceGroup(fileName);
            if (dataServiceGroup == null || dataServiceGroup.Addresses == null || dataServiceGroup.Addresses.Length==0)
                return fileName;
            var name = dataServiceGroup.Addresses[hashValue%dataServiceGroup.Addresses.Length];
            var address = Address.GetAddress(name);
            if (address == null) return fileName;
            return string.Format("{0}{1}", address.Url, fileName);
        }
        /// <summary>
        /// 创建文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateFileName(string fileName)
        {
           
            fileName = FileRoute.CreateFileName(fileName);
            if (string.IsNullOrEmpty(fileName))
                return fileName;
            var index = fileName.LastIndexOf('.');
            if (index == -1)
                index = fileName.Length;
            var hashValue = GetHashValue();
            var dataServiceGroup = GetDataServiceGroup(fileName,hashValue);
            if (dataServiceGroup == null)
                return fileName;
            return fileName.Insert(index, string.Format("_{0}",dataServiceGroup.Name));
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(string fileName,byte[] fileByte)
        {
            var hashValue = GetHashValue();
            var dataServiceGroup = GetDataServiceGroup(fileName);
            if (dataServiceGroup == null || dataServiceGroup.DataServices == null)
                return;
            var dataService = GetMasterDataService(dataServiceGroup, hashValue);
            var endpoints = dataServiceGroup.DataServices.Select(it => it.EndPoint.Name).ToList();
            WcfService.Invoke<IFileContract>(new[] { dataService.EndPoint }, SaveFiles,null,true, endpoints, fileName, fileByte);
        }

 
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void Remove(string fileName)
        {
            var hashValue = GetHashValue();
            var dataServiceGroup = GetDataServiceGroup(fileName);
            if (dataServiceGroup == null || dataServiceGroup.DataServices == null)
                return;
            var dataService = GetMasterDataService(dataServiceGroup, hashValue);
            var endpoints = dataServiceGroup.DataServices.Select(it => it.EndPoint.Name).ToList();
            WcfService.Invoke<IFileContract>(new[] { dataService.EndPoint }, RemoveFiles, null, true, endpoints, fileName);

        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual byte[] Download(string fileName)
        {
            var dataServiceGroup = GetDataServiceGroup(fileName);
            if (dataServiceGroup == null || dataServiceGroup.DataServices == null)
                return null;
            return WcfService.Invoke<IFileContract>(dataServiceGroup.DataServices.Select(it => it.EndPoint).ToList(), DownloadFiles, fileName) as byte[];
  
  
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
            var endPointNames=paramters[0] as IList<string>;
            endPointNames = endPointNames==null?null:endPointNames.Where(it => it != ((EndPointInfo)paramters[3]).Name).ToList();
            fileService.Save(endPointNames, paramters[1] as string, paramters[2] as byte[]);
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
            var endPointNames = paramters[0] as IList<string>;
            endPointNames = endPointNames == null ? null : endPointNames.Where(it => it != ((EndPointInfo)paramters[2]).Name).ToList();
            fileService.Remove(endPointNames, paramters[1] as string);
            return null;
           
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual byte[] DownloadFiles(IFileContract fileService, params object[] paramters)
        {
            return fileService.Download(paramters[0] as string);

        }

        /// <summary>
        /// 得到写的服务器
        /// </summary>
        /// <param name="dataServiceGroup"></param>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        protected virtual DataServiceInfo GetMasterDataService(DataServiceGroupInfo dataServiceGroup,long hashValue)
        {
            var dataServices = dataServiceGroup.DataServices.Where(it=>it.Type==DataServiceType.Master).ToList();
            if (dataServices.Count == 0) return null;
            return dataServices[(int)(hashValue%dataServices.Count)];
        }

        /// <summary>
        /// 得到数据服务器
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        protected virtual DataServiceGroupInfo GetDataServiceGroup(string fileName,long hashValue)
        {
            var dataServiceGroups =
                DataServiceGroups.Where(it => (string.IsNullOrEmpty(it.Path) || fileName.Contains(it.Path)) && !it.IsClose).ToList();
            var index = (int) (hashValue%(dataServiceGroups.Count == 0 ? 1 : dataServiceGroups.Count));
            return dataServiceGroups[index];
        }
        /// <summary>
        /// 得到hash值
        /// </summary>
        /// <returns></returns>
        protected virtual long GetHashValue()
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// 得到数据服务器
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual DataServiceGroupInfo GetDataServiceGroup(string fileName)
        {
            string name = null;
            var startIndex = fileName.LastIndexOf("_") + 1;
            if (startIndex > 0)
            {
                var endIndex = fileName.IndexOf(".", startIndex);
                if (startIndex > 0 && endIndex > -1 && endIndex > startIndex)
                {
                    name = fileName.Substring(startIndex, endIndex - startIndex);
                }
            }
            var dataServiceGroup =string.IsNullOrEmpty(name)?
                DataServiceGroups.FirstOrDefault(it => (string.IsNullOrEmpty(it.Path) || fileName.Contains(it.Path))):
                DataServiceGroups.FirstOrDefault(it=>it.Name.Equals(name));
            return dataServiceGroup;
        }

        #endregion
 



    }
}
