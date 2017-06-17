using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Component.Extension;
using Configuration;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Supplier;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Client.Publisher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigurationManager.Initialize(@"Beeant.Presentation.Client.Publisher");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            label1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImportGoodses();
            MessageBox.Show("导入完成");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDetailTemplate();
            LoadCategories();
            LoadFreights();
            LoadSuppliers();
        }
        /// <summary>
        /// 加载模板
        /// </summary>
        protected virtual void LoadDetailTemplate()
        {
            var dic = new DirectoryInfo("Template");
            if(!dic.Exists)
                return;
            var dics = dic.GetDirectories();
            foreach (var d in dics)
            {
                comboBox1.Items.Add(d.Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem==null)
                return;
            var dic = new DirectoryInfo(string.Format(@"Template/{0}", comboBox1.SelectedItem));
            txtImageTemplate.Text = File.ReadAllText(string.Format("{0}/图片模板.txt", dic.FullName));
            txtTempalte.Text = File.ReadAllText(string.Format("{0}/模板.txt", dic.FullName));
        }

        #region 得到导入类目目录
        /// <summary>
        /// 得到导入类目目录
        /// </summary>
        /// <returns></returns>
        protected virtual DirectoryInfo[] GetImportCategoryDirectories()
        {
            if (string.IsNullOrEmpty(label1.Text))
                return null;
            var dic = new DirectoryInfo(label1.Text);
            return dic.GetDirectories();
        }

        #endregion

        #region 初始化类目
        public IList<CategoryEntity> Categories { get; set; }
        /// <summary>
        /// 加载类目
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadCategories()
        {
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Select(it => new object[] { it, it.CategoryProperties.Select(s => s) });
            Categories = Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntities<CategoryEntity>(query);
        }
        public IList<FreightEntity> Freights { get; set; }
        /// <summary>
        /// 加载类目
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadFreights()
        {
            var query = new QueryInfo();
            query.Query<FreightEntity>();
            Freights = Ioc.Resolve<IApplicationService, FreightEntity>().GetEntities<FreightEntity>(query);
        }
        public IList<SupplierEntity> Suppliers { get; set; }
        /// <summary>
        /// 加载类目
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadSuppliers()
        {
            var query = new QueryInfo();
            query.Query<SupplierEntity>();
            Suppliers = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntities<SupplierEntity>(query);
        }
        #endregion

        #region 得到Excel内容

        /// <summary>
        /// 读取导入数据
        /// </summary>
        /// <param name="strExcelFileName">Excel表路径及文件名称</param>
        /// <param name="strSheet"></param>
        /// <returns>DataTable:importTable</returns>
        public DataTable GetImportTable(string strExcelFileName, string strSheet)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
            string strExcelSql = string.Format("select * from [{0}$]", strSheet);
            DataSet ds = new DataSet();
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcelSql, strConn);
                adapter.Fill(ds, "importTable");
                var dt = ds.Tables["importTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0][i].Convert<string>()))
                            continue;
                        dt.Columns[i].ColumnName = dt.Rows[0][i].Convert<string>();
                    }
                    return dt;
                }

            }
            catch (OleDbException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region 创建商品
        #region 导入商品
        /// <summary>
        /// 添加商品
        /// </summary>
        protected virtual void ImportGoodses()
        {
            var dics = GetImportCategoryDirectories();
            foreach (var dic in dics)
            {
                var category = Categories == null ? null : Categories.FirstOrDefault(it => it.Id == dic.Name.Convert<long>());
                if (category == null)
                    continue;
                var goodsDics = dic.GetDirectories();
                foreach (var goodsDic in goodsDics)
                {
                    CreateGoods(goodsDic, category);
                }
            }
        }
        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        protected virtual void CreateGoods(DirectoryInfo directory, CategoryEntity category)
        {
            var files = directory.GetFiles().Where(it => it.Name.ToLower().EndsWith(".xls") || it.Name.ToLower().EndsWith(".xlsx")).ToArray();
            if (files.Length == 0)
                return;
            var goods = new GoodsEntity
            {
                SaveType = SaveType.Add,
                Account = new AccountEntity { Id = 0 },
                Name = directory.Name,
                Category = category,
                UnusedStatus = "1",
                IsSales = false,
                Sequence = 1,
                OrderMinCount = 1,
                OrderStepCount = 1,
                DepositRate = 0,
                IsCustom = false,
                IsReturn = true,
                DataId = "",
                PublishTime = DateTime.Now,
                Freight = Freights == null ? null : Freights.FirstOrDefault()
            };
            var zutuDics = directory.GetDirectories("主图");
            var xiangDics = directory.GetDirectories("详情图");
            FillProducts(files[0], goods, category);
            FillGoodsProperties(files[0], goods, category);
            FillGoodImages(zutuDics.FirstOrDefault(), category, goods, "");
            FillGoodsDetail(xiangDics.FirstOrDefault(), goods, "");
            var rev = goods.Errors != null && goods.Errors.Count > 0 ? false : Ioc.Resolve<IApplicationService, GoodsEntity>().Save(goods);
            if (rev)
            {
                DeleteDirectory(directory.FullName);
                directory.Delete();
            }
            else
            {
                dataGridView1.Rows.Clear();
                var builder = new StringBuilder();
                if (goods.Errors != null)
                {

                    foreach (var error in goods.Errors)
                    {
                        builder.AppendFormat("{0},", error.Message);
                    }
                }
                dataGridView1.Rows.Add(new object[] { directory.FullName, builder.ToString() });
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="strPath"></param>
        protected virtual void DeleteDirectory(string strPath)
        {
            {
                //删除这个目录下的所有文件
                if (Directory.GetFiles(strPath).Length > 0)
                {
                    foreach (string var in Directory.GetFiles(strPath))
                    {
                        File.Delete(var);
                    }
                }
                //删除这个目录下的所有子目录
                if (Directory.GetDirectories(strPath).Length > 0)
                {
                    foreach (string var in Directory.GetDirectories(strPath))
                    {
                        DeleteDirectory(var);
                        Directory.Delete(var, true);
                    }
                }
            }
        }
        #endregion

        #region 填充商品主图

        /// <summary>
        /// 填充商品主图
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="category"></param>
        /// <param name="goods"></param>
        /// <param name="sku"></param>
        protected virtual void FillGoodImages(DirectoryInfo directory, CategoryEntity category, GoodsEntity goods, string sku)
        {
            if (directory == null || !directory.Exists)
                return;
            var files = directory.GetFiles()
                .Where(it => it.Name.ToLower().EndsWith(".jpg") || it.Name.ToLower().EndsWith(".png") || it.Name.ToLower().EndsWith(".gif"))
                .OrderBy(it => it.Name.Replace(Path.GetExtension(it.Name), "").Convert<int>()).ToArray();
            goods.GoodsImages = goods.GoodsImages ?? new List<GoodsImageEntity>();
            var i = 1;
            foreach (var file in files)
            {
                var product = goods.Products == null || string.IsNullOrEmpty(sku) ? null : goods.Products.FirstOrDefault(it => it.Sku == sku);
                if (product == null)
                    product = new ProductEntity { Id = 0 };
                var goodsImage = new GoodsImageEntity
                {
                    SaveType = SaveType.Add,
                    Sequence = i,
                    Goods = goods,
                    FileName = string.Format("Files/Images/Goods/{0}", file.Name),
                    Product = product,
                    FileByte = GetFileByte(file.FullName),
                };
                i++;
                goods.GoodsImages.Add(goodsImage);
            }
            if (goods.Products == null)
                return;
            var dics = directory.GetDirectories();
            foreach (var dic in dics)
            {
                var product = goods.Products.FirstOrDefault(it => it.Name == dic.Name);
                if (product == null)
                    continue;
                FillGoodImages(dic, category, goods, product.Sku);
            }
        }
        #endregion

        #region 填充商品扩展属性
        /// <summary>
        /// 填充商品扩展属性
        /// </summary>
        /// <param name="file"></param>
        /// <param name="goods"></param>
        /// <param name="category"></param>
        protected virtual void FillGoodsProperties(FileInfo file, GoodsEntity goods, CategoryEntity category)
        {
            var table = GetImportTable(file.FullName, "Sheet2");
            if (table == null)
                return;
            goods.GoodsProperties = goods.GoodsProperties ?? new List<GoodsPropertyEntity>();
            var i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                if (i == 1) continue;
                foreach (DataColumn col in table.Columns)
                {
                    foreach (var product in goods.Products)
                    {
                        var goodsProperty = new GoodsPropertyEntity
                        {
                            Goods = goods,
                            Product = product,
                            SaveType = SaveType.Add
                        };
                        if (category.CategoryProperties == null)
                            continue;
                        var property =
                            category.CategoryProperties.FirstOrDefault(
                                it => it.Name == col.ColumnName && it.IsUsed);
                        if (property == null)
                            continue;
                        goodsProperty.Property = property;
                        goodsProperty.Value = row[col.ColumnName].Convert<string>();
                        if (!string.IsNullOrEmpty(goodsProperty.Value))
                        {
                            goods.GoodsProperties.Add(goodsProperty);
                        }
                    }

                }

            }
        }
        #endregion

        #region 填充商品
        /// <summary>
        /// 填充产品
        /// </summary>
        /// <param name="file"></param>
        /// <param name="goods"></param>
        /// <param name="category"></param>
        protected virtual void FillProducts(FileInfo file, GoodsEntity goods, CategoryEntity category)
        {
            var table = GetImportTable(file.FullName, "Sheet1");
            if (table == null)
                return;
            goods.Products = new List<ProductEntity>();
            var i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                if (i == 1) continue;
                var product = new ProductEntity
                {
                    Goods = goods,
                    SaveType = SaveType.Add,
                    OrderStepCount = 1,
                    OrderMinCount = 1,
                    Count = 1000,
                    Price = 99999999,
                    Cost = 99999999,
                    Name = file.Name,
                    Sku = "",
                    DataId = ""
                };
                FillProductColumn(table, goods, product, row, category);
                if (string.IsNullOrEmpty(product.Name))
                    continue;
                goods.Products.Add(product);
            }
        }

        /// <summary>
        /// 填充产品字段
        /// </summary>
        /// <param name="table"></param>
        /// <param name="goods"></param>
        /// <param name="product"></param>
        /// <param name="row"></param>
        /// <param name="category"></param>
        protected virtual void FillProductColumn(DataTable table, GoodsEntity goods, ProductEntity product, DataRow row, CategoryEntity category)
        {
            var sku = new List<string>();
            foreach (DataColumn col in table.Columns)
            {
                switch (col.ColumnName)
                {
                    case "名称":
                        product.Name = row[col.ColumnName].Convert<string>().Trim();
                        break;
                    case "面价":
                        product.Price = row[col.ColumnName].Convert<decimal>();
                        break;
                    case "底价":
                        product.Cost = row[col.ColumnName].Convert<decimal>();
                        break;
                    case "起订数量":
                        product.OrderMinCount = row[col.ColumnName].Convert<int>();
                        break;
                    case "订购步长数量":
                        product.OrderStepCount = row[col.ColumnName].Convert<int>();
                        break;
                    case "数量":
                        product.Count = row[col.ColumnName].Convert<int>();
                        break;
                    case "商家编码":
                        product.DataId = row[col.ColumnName].Convert<string>();
                        break;
                    default:
                        if (category.CategoryProperties == null)
                            continue;
                        var property =
                            category.CategoryProperties.FirstOrDefault(
                                it => it.Name == col.ColumnName && it.IsSku && it.IsUsed);
                        if (property == null)
                            continue;
                        var rowValue = row[col.ColumnName].Convert<string>();
                        if (string.IsNullOrEmpty(rowValue))
                            continue;
                        if (property.IsSku)
                        {
                            var value = string.Format("Id:\"{0}\",Name:\"{1}\",Value:\"{2}\"", property.Id,
                                                      property.Name, rowValue);
                            sku.Add("{" + value + "}");
                        }
                        goods.GoodsProperties = goods.GoodsProperties ?? new List<GoodsPropertyEntity>();
                        var goodsProperty = new GoodsPropertyEntity
                        {
                            Goods = goods,
                            Value = rowValue,
                            Property = property,
                            Product = product,
                            SaveType = SaveType.Add
                        };

                        goods.GoodsProperties.Add(goodsProperty);
                        break;
                }
            }
            if (sku.Count > 0)
            {
                product.Sku = "[" + string.Join(",", sku.ToArray()) + "]";
            }
        }
        #endregion

        #region 填充详情图片

        /// <summary>
        /// 填充详情页模板
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="goods"></param>
        /// <param name="sku"></param>
        protected virtual void FillGoodsDetail(DirectoryInfo directory, GoodsEntity goods, string sku)
        {
            if (directory == null || !directory.Exists)
                return;
            goods.GoodsDetails = goods.GoodsDetails ?? new List<GoodsDetailEntity>();
            var product = goods.Products == null ? null : goods.Products.FirstOrDefault(it => it.Sku == sku);
            if (product == null)
                product = new ProductEntity { Id = 0 };
            var goodsDetail = new GoodsDetailEntity
            {
                SaveType = SaveType.Add,
                Goods = goods,
                Product = product,
                Detail = GetGoodsDetail(goods, directory),
            };
            goods.GoodsDetails.Add(goodsDetail);
            if (goods.Products == null)
                return;
            var dics = directory.GetDirectories();
            foreach (var dic in dics)
            {
                var tproduct = goods.Products.FirstOrDefault(it => it.Name == dic.Name);
                if (tproduct == null)
                    continue;
                FillGoodsDetail(dic, goods, tproduct.Sku);
            }
        }

        /// <summary>
        /// 得到详情页
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        protected virtual string GetGoodsDetail(GoodsEntity goods, DirectoryInfo directory)
        {
            var files = directory.GetFiles().Where(it => it.Name.ToLower().EndsWith(".jpg") || it.Name.ToLower().EndsWith(".png") || it.Name.ToLower().EndsWith(".gif"))
                .OrderBy(it => it.Name.Replace(Path.GetExtension(it.Name), "").Convert<int>()).ToArray().ToArray();
            var builder = new StringBuilder();
            foreach (var file in files)
            {
                var Entity = new ImageEntity
                {
                    Account = new AccountEntity { Id = 0 },
                    FileName = string.Format("Files/Eidtor/Images/Goods/{0}", file.Name),
                    Name = file.Name,
                    Folder = new FolderEntity { Id = 0 },
                    FileByte = GetFileByte(file.FullName),
                    SaveType = SaveType.Add
                };
                try
                {
                    if (Entity.Name.Length > 50)
                    {
                        Entity.Name = file.Name.Replace(Path.GetExtension(file.Name), "");
                        if (Entity.Name.Length > 50)
                            Entity.Name = Entity.Name.Substring(0, 50);
                    }
                    if (Ioc.Resolve<IApplicationService, ImageEntity>().Save(Entity))
                    {
                        builder.AppendFormat(txtImageTemplate.Text, Entity.FullFileName);
                    }
                    else
                    {
                        goods.Errors = goods.Errors ?? new List<ErrorInfo>();
                        goods.Errors.AddList(Entity.Errors);
                    }
                }
                catch 
                {
                    goods.Errors = goods.Errors ?? new List<ErrorInfo>();
                    goods.Errors.Add(new ErrorInfo { Key = "异常", Message = "商品详情导入异常" });
                }
            }
            return string.Format(txtTempalte.Text, builder);
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
