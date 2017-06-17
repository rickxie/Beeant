
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Search;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Winner.Search.Document;
using Winner.Search.Store;

namespace Beeant.Application.Services.Product
{
    public class GoodsSearchEventApplicationService : IJobApplicationService
    {
        public static bool IsExecte;
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// 搜索实例
        /// </summary>
        public virtual ISearchRepository SearchRepository { get; set; }

        #region 执行服务

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Execute(object[] args)
        {
            if (args == null || IsExecte) return false;
            try
            {
                IsExecte = true;
                var name = args[0].Convert<string>();
                CreateGoods(name);
            }
            catch 
            {


            }
            finally
            {
                IsExecte = false;
            }
            return true;
        }

        #endregion

        #region 创建索引
        /// <summary>
        /// 添加
        /// </summary>
        protected virtual void CreateGoods(string name)
        {
            var indexer = Winner.Creator.Get<Winner.Search.IIndexer>();
            indexer.Delete(name);
            indexer.InitlizeDocumentIndex();
            indexer.Begin(name);
            SaveDocuments(name,indexer);
            indexer.End(name);
            UpdateCache(name);
        }

        ///  <summary>
        /// 得到搜索产品
        ///  </summary>
        ///  <param name="pageIndex"></param>
        /// <returns></returns>
        protected virtual IList<ProductEntity> GetSearchGoods(int pageIndex)
        {
            var query = new QueryInfo();
            query.SetPageIndex(pageIndex)
                 .SetPageSize(10000).Query<ProductEntity>().Where(it => it.IsSales)
                 .Select(it => new object[] { it.Id,it.Name,it.Price,it.Cost,it.Goods.Category.Id,it.Goods.Category.Name,it.IsCustom,it.Goods.PublishTime,it.Goods.FileName,
                 it.Goods.Category.Parent.Name,it.Goods.Category.Parent.Parent.Name,it.GoodsProperties.Select(s=>new object[]{s.Property.Name,s.Value,s.Property.IsUsed,s.Property.SearchType})});
            return Repository.GetEntities<ProductEntity>(query);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CategoryEntity> GetCategories()
        {
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Select(it => new object[] {it.Id, it.Parent.Id, it.Name});
            return Repository.GetEntities<CategoryEntity>(query);
        }
      

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="name"></param>
        /// <param name="indexer"></param>
        protected virtual void SaveDocuments(string name,Winner.Search.IIndexer indexer)
        {

            for (int i = 0; ; i++)
            {
                var documents = new List<DocumentInfo>();
                var infos = GetSearchGoods(i);
                if (infos == null || infos.Count == 0)
                    break;
                foreach (var info in infos)
                {
                    if (info.Goods == null)
                        continue;
                    var document = new DocumentInfo
                        {
                            Feilds = new[]
                                {
                                    new FeildInfo {Text = info.Id.ToString()},
                                    new FeildInfo {Text = string.IsNullOrEmpty(info.Name)?info.Name:info.Name.ToLower()},
                                    new FeildInfo {Text = GetGoodsCategoryValue(info.Goods).ToLower()},
                                    new FeildInfo {Text = GetGoodsPropertyValue(info).ToLower()},
                                    new FeildInfo {Text = info.Cost.ToString()},
                                    new FeildInfo {Text = info.Price.ToString()},
                                    new FeildInfo {Text = info.IsCustom.ToString()},
                                    new FeildInfo {Text = info.Goods.PublishTime.ToString("yyyy-MM-dd HH:mm:ss")},
                                  
                                    new FeildInfo {Text = info.Goods.FileName},
                                    new FeildInfo
                                        {
                                            Text = info.Goods.Category == null ? "" : info.Goods.Category.Id.ToString()
                                        }
                                },


                        };
                    documents.Add(document);
                }
                i++;
                indexer.AddDocuments(name,documents);
                if (infos.Count < 10000)
                    break;
            }
        }

        /// <summary>
        /// 得到商品属性
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        protected virtual string GetGoodsCategoryValue(GoodsEntity goods)
        {
            if (goods == null || goods.Category == null)
                return "";
            var category = goods.Category;
            var sb = new StringBuilder();
            do
            {
                sb.AppendFormat("{0},", category.Name);
                category = category.Parent;
            } while (category!=null && !string.IsNullOrEmpty(category.Name));
            return sb.ToString().Trim();
        }
        /// <summary>
        /// 得到商品属性
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        protected virtual string GetGoodsPropertyValue(ProductEntity product)
        {
            if (product == null || product.GoodsProperties == null)
                return "";
            var values = new List<string>();
            foreach (var gp in product.GoodsProperties)
            {
                if(gp.Property==null || !gp.Property.IsUsed || gp.Property.SearchType==PropertySearchType.None)
                    continue;
                values.Add(string.Format("{0}{1},", gp.Property.Name,gp.Value));
            }
            values = values.OrderBy(it => it).ToList();
            return string.Join(",", values);
        }
        #endregion

        #region 更新缓存

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="name"></param>
        public virtual void UpdateCache(string name)
        {
            var path = string.Format("{0}{1}", Winner.Creator.Get<IStorer>().Path, name);
            SearchRepository.RemoveIndex(path);
            TransferFiles(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path));
            SearchRepository.UpdateIndexCache(name);
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceName"></param>
        protected virtual void TransferFiles(string sourceName)
        {
            if(!Directory.Exists(sourceName))
                return;
            string[] fileNames = Directory.GetFiles(sourceName);
            string[] directoryNames = Directory.GetDirectories(sourceName);
            foreach (string fileName in fileNames)
            {
                var filebyte = GetFileByte(fileName);
                SearchRepository.SynchronousIndex(fileName.Replace(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ""), filebyte);
            }
            if (directoryNames.Length == 0) return;
            foreach (string directoryName in directoryNames)
            {
                TransferFiles(directoryName);
            }
        }
        /// <summary>
        /// 得到文件流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual byte[] GetFileByte(string fileName)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, fileName);
            using (var fileSream = new FileStream(fileName, FileMode.Open))
            {
                var fileByte = new byte[fileSream.Length];
                fileSream.Read(fileByte, 0, fileByte.Length);
                return fileByte;
            }
        }
        #endregion
    }
}