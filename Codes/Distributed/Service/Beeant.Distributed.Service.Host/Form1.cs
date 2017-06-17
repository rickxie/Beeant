using System;
using System.Configuration;
using System.Windows.Forms;

namespace Beeant.Distributed.Service.Host
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            this.Closing += Form1_Closing;
            if (comboBox1.Items.Count == 1)
            {
                comboBox1.SelectedIndex = 0;
                StartService();
            }

        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (comboBox1.SelectedItem == null || button1.Enabled)
            {
                return;
            }
            var value = comboBox1.SelectedItem.ToString().Split('-');
            Winner.Creator.Get<Winner.Wcf.IWcfHost>().Stop(Type.GetType(value[1]));
        }

        /// <summary>
        /// 得到表格
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadData()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                comboBox1.Items.Add(string.Format("{0}-{1}", key, ConfigurationManager.AppSettings[key]));

            }
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        protected virtual void StartService()
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择服务");
                return;
            }
            var value = comboBox1.SelectedItem.ToString().Split('-');
            Configuration.ConfigurationManager.Initialize(Type.GetType(value[1]).FullName);
            if (value[1] == "Beeant.Distributed.Host.Service.SearchService")
            {
                Winner.Creator.Get<Winner.Search.IIndexer>().InitlizeDocumentIndex();
            }
            Winner.Creator.Get<Winner.Wcf.IWcfHost>().Start(Type.GetType(value[1]));
            Text = comboBox1.SelectedItem.ToString();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartService();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || button1.Enabled)
            {
                return;
            }
            var value = comboBox1.SelectedItem.ToString().Split('-');
            Winner.Creator.Get<Winner.Wcf.IWcfHost>().Stop(Type.GetType(value[1]));
            Close();
        }
    }
}
