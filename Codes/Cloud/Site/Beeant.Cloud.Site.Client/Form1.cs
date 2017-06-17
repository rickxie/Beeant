using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Component.Extension;
using Configuration;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigurationManager.Initialize(@"Beeant.Cloud.Site.Client");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            label1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImportCommodities();
            MessageBox.Show("导入完成");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }


        #region 得到导入类目目录
        /// <summary>
        /// 
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 得到导入类目目录
        /// </summary>
        /// <returns></returns>
        protected virtual DirectoryInfo[] GetImportCatalogDirectories()
        {
            if (string.IsNullOrEmpty(label1.Text))
                return null;
            var dic = new DirectoryInfo(label1.Text);
            SiteId = dic.Name.Convert<long>();
            return dic.GetDirectories();
        }

        /// <summary>
        /// 加载类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs()
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Select(it => new object[] { it });
            return Ioc.Resolve<IApplicationService, CatalogEntity>().GetEntities<CatalogEntity>(query);
        }
        public IList<CatalogEntity> Catalogs { get; set; }
        /// <summary>
        /// D
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCatalogSequence()
        {
            var query = new QueryInfo();
            query.SetPageSize(1).Query<CatalogEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = Ioc.Resolve<IApplicationService, CatalogEntity>().GetEntities<CatalogEntity>(query);
            var entity = entities?.FirstOrDefault();
            if (entity != null)
            {
                if (entity.Sequence + 5000 > 100000000)
                    return 100000000;
                return entity.Sequence + 5000;
            }
            return -100000000;
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        protected virtual void ImportCatalogs()
        {
            var catalogs = GetCatalogs();
            var dics = GetImportCatalogDirectories();
            IList<CatalogEntity> entities = new List<CatalogEntity>();
            var sequence = GetCatalogSequence();
            foreach (var dic in dics)
            {
                var category = catalogs == null
                    ? null
                    : catalogs.FirstOrDefault(it => it.Name == dic.Name);
                if (category != null)
                    continue;
                entities.Add(new CatalogEntity
                {
                    Name = dic.Name,
                    FileName = "",
                    Site=new SiteEntity { Id=SiteId},
                    Sequence= sequence,
                    SaveType = SaveType.Add
                });
                sequence = sequence + 5000 > 100000000 ? 100000000 + 1 : sequence + 5000;
            }
            if (entities.Count > 0)
            {
               Ioc.Resolve<IApplicationService, CatalogEntity>().Save(entities);
            }
        }
        /// <summary>
        /// 导入类目
        /// </summary>
        protected virtual void LoadCatalogs()
        {
            ImportCatalogs();
            Catalogs = GetCatalogs();
        }
        #endregion

        #region 

        #endregion
        /// <summary>
        /// 排序
        /// </summary>
        public int CommoditySequence { get; set; }
        /// <summary>
        /// D
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCommoditySequence()
        {
            var query = new QueryInfo();
            query.SetPageSize(1).Query<CommodityEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = Ioc.Resolve<IApplicationService, CommodityEntity>().GetEntities<CommodityEntity>(query);
            var entity = entities?.FirstOrDefault();
            if (entity != null)
            {
                if (entity.Sequence - 5000 < -100000000)
                    return -100000000;
                return entity.Sequence - 5000;
            }
            return 100000000;
        }
        #region 创建商品

        #region 导入商品
     
        /// <summary>
        /// 添加商品
        /// </summary>
        protected virtual void ImportCommodities()
        {
            LoadCatalogs();
            var dics = GetImportCatalogDirectories();
            CommoditySequence = GetCommoditySequence();
            foreach (var dic in dics)
            {
                var catalog = Catalogs == null ? null : Catalogs.FirstOrDefault(it => it.Name == dic.Name);
                if (catalog == null)
                    continue;
                CreateCommodity(dic, catalog);
            }
        }
        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        protected virtual void CreateCommodity(DirectoryInfo directory, CatalogEntity category)
        {

            if (directory == null || !directory.Exists)
                return;
            var files = directory.GetFiles()
                .Where(it => it.Name.ToLower().EndsWith(".jpg") || it.Name.ToLower().EndsWith(".png") || it.Name.ToLower().EndsWith(".gif"))
                .OrderBy(it => it.Name.Replace(Path.GetExtension(it.Name), "").Convert<int>()).ToArray();
            var dataInfos = GetCommodities(category.Id);
            for (int j = 0; j < files.Length; j++)
            {
                var index = files[j].Name.IndexOf(".");
                if (index <= -1)
                    continue;
                var names = files[j].Name.Substring(0, index).Split('|');
                var commodity = dataInfos.FirstOrDefault(it => it.Name == names[0]);
                var selectText = comboBox1.SelectedItem.Convert<string>();
                if (commodity == null && selectText == "目录图片")
                    continue;
                var fileName = string.Format("Files/Images/{0}/copy{1}", selectText== "目录图片"? "SiteAlbum": "SiteCommodity", Path.GetExtension(files[j].Name));
                if (commodity == null)
                {
                    commodity = new CommodityEntity
                    {
                        Catalog = category,
                        Name = names[0],
                        Description = names.Length > 1 ? names[1] : "",
                        Cost = 0,
                        Price = 0,
                        IsShowPrice = false,
                        Status = CommodityStatusType.Normal,
                        VenderAddress = "",
                        VenderMobile = "",
                        VenderName = "",
                        VenderLinkman = "",
                        Password = "",
                        Site = new SiteEntity { Id = SiteId },
                        Sequence = CommoditySequence,
                        FileName = "",
                        AlbumFileName = "",
                        IsCreateAlbum = true,
                        SaveType = SaveType.Add
                    };
                    if (selectText== "目录图片")
                    {
                        commodity.AlbumFileName = fileName;
                    }
                    else
                    {
                        commodity.FileName = fileName;
                    }

                }
                else
                {
                    if (selectText== "目录图片")
                    {
                        commodity.AlbumFileName = fileName;
                        commodity.SetProperty(it => it.AlbumFileName);
                    }
                    else
                    {
                        commodity.FileName = fileName;
                        commodity.SetProperty(it => it.FileName);
                    }
                    commodity.SaveType=SaveType.Modify;
                }
                var bt=ImageHelper.Create(files[j].FullName, 800, 800, Color.White, 0);
                ImageHelper.Save(files[j].FullName,bt);
                if (selectText == "目录图片")
                {
                    commodity.AlbumFileByte = GetFileByte(files[j].FullName);
                }
                else
                {
                    commodity.FileByte = GetFileByte(files[j].FullName);
                }
                CommoditySequence = CommoditySequence + 5000 > 100000000 ? 100000000 + 1 : CommoditySequence + 5000;
                var rev = Ioc.Resolve<IApplicationService, CommodityEntity>().Save(commodity);
                if (rev)
                {
                    files[j].Delete();
                }
                else
                {
                    dataGridView1.Rows.Add(new object[] { directory.FullName, commodity.Errors?.FirstOrDefault()?.Message });
                }
            }

         
        }

        /// <summary>
        /// 得到商品
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(long catalogId)
        {
            var query = new QueryInfo();
            query.Query<CommodityEntity>()
                .Where(it => it.Catalog.Id == catalogId)
                .Select(it => new object[] {it.Id, it.Name});
            return Ioc.Resolve<IApplicationService>().GetEntities<CommodityEntity>(query);
        }

        #endregion


        /// <summary>
        /// 得到文件流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual byte[] GetFileByte(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                var result = new byte[file.Length];
                file.Read(result, 0, result.Length);
                return result;
            }
        }

        #endregion
    }
}
