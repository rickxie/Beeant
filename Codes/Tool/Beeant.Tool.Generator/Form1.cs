using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beeant.Tool.Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            GetTableScheme();
        }

        #region 得到表结构
        /// <summary>
        /// 得到表结构
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable GetTableScheme()
        {
            var dt = new DataTable();
            try
            {
                var sql =
                    @"select  
     c.Name [表名], 
     isnull(f.[value],'') [表说明],  
     a.Column_id [列序号],  
     a.Name [列名],  
     isnull(e.[value],'') [列说明],  
     b.Name [数据库类型],    
     case when b.Name = 'image' then 'byte[]'
                  when b.Name in('image','uniqueidentifier','ntext','varchar','ntext','nchar','nvarchar','text','char') then 'string'
                  when b.Name in('tinyint','smallint','int','bigint') then 'int'
                  when b.Name in('datetime','smalldatetime') then 'DateTime'
                  when b.Name in('float','decimal','numeric','money','real','smallmoney') then 'decimal'
                  when b.Name ='bit' then 'bool' else b.name end [类型] ,
      case when is_identity=1 then '是' else '' end [标识],  
      case when exists(select 1 from sys.objects x join sys.indexes y on x.Type=N'PK' and x.Name=y.Name  
                         join sysindexkeys z on z.ID=a.Object_id and z.indid=y.index_id and z.Colid=a.Column_id)  
                     then '是' else '' end [主键],       
     case when a.[max_length]=-1 and b.Name!='xml' then 'max/2G'  
                   when b.Name='xml' then '2^31-1字节/2G' 
                   else rtrim(a.[max_length]) end [字节数],  
     case when ColumnProperty(a.object_id,a.Name,'Precision')=-1 then '2^31-1' 
                 else rtrim(ColumnProperty(a.object_id,a.Name,'Precision')) end [长度],  
     isnull(ColumnProperty(a.object_id,a.Name,'Scale'),0) [小数位],  
     case when a.is_nullable=1 then '是' else '' end [是否为空],      
     isnull(d.text,'') [默认值]      
 from  
     sys.columns a   
 left join 
     sys.types b on a.user_type_id=b.user_type_id  
 inner join 
     sys.objects c on a.object_id=c.object_id and c.Type='U' and c.name='" + txtTable.Text + @"'
 left join 
     syscomments d on a.default_object_id=d.ID  
 left join 
     sys.extended_properties e on e.major_id=c.object_id and e.minor_id=a.Column_id and e.class=1   
 left join 
     sys.extended_properties f on f.major_id=c.object_id and f.minor_id=0 and f.class=1 ";
                using (SqlConnection con = new SqlConnection(txtSqlCon.Text))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
            return dt;
        }
        #endregion
    }
}
