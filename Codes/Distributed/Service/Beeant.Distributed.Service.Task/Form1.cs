using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Component.Extension;
using Configuration;
using Beeant.Application.Services;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Entities.Sys;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Distributed.Service.Task
{
    public partial class Form1 : Form
    {
        #region 初始化

        public Form1()
        {
            InitializeComponent();
            ConfigurationManager.Initialize(@"Beeant.Distributed.Service.Task");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTasks();
            var th = new Thread(Start) { IsBackground = true };
            th.Start();
            btnSwich.Text = @"停止";
        }

        #endregion

        #region 服务调用

        protected delegate void ExecuteHandler(TaskEntity info);

        protected bool Switch = false;


        private void btnSwich_Click(object sender, EventArgs e)
        {
            if (!Switch)
            {
                var th = new Thread(Start) { IsBackground = true };
                th.Start();
                btnSwich.Text = @"停止";
            }
            else
            {
                Stop();
                btnSwich.Text = @"开启";
            }

        }


        protected virtual void Stop()
        {
            Switch = false;

        }

        protected virtual void Start()
        {

            Switch = true;
            while (Switch)
            {
                ExecuteTasks();
                Thread.Sleep((int)numericUpDown1.Value);
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        protected virtual void ExecuteTasks()
        {
            var infos = GetTaskEntitys();
            if (infos == null || infos.Count == 0) return;
            var handler = new ExecuteHandler(Execute);
            foreach (var info in infos)
            {
                handler.BeginInvoke(info, null, null);
            }
        }

        /// <summary>
        /// 执行单个
        /// </summary>
        /// <param name="info"></param>
        protected virtual void Execute(TaskEntity info)
        {
            try
            {
                Ioc.Resolve<ITaskApplicationService>()
                   .Execute(info, Ioc.Resolve<IJobApplicationService>(info.ClassName));
            }
            catch (Exception ex)
            {
                AddErrorInfo(ex, info);
            }
        }

        /// <summary>
        /// 得到任务列表
        /// </summary>
        /// <returns></returns>
        protected virtual IList<TaskEntity> GetTaskEntitys()
        {
            var query =
                new QueryInfo().SetCacheTime(DateTime.Now.AddSeconds((int)numericUpDown2.Value));
            var ids =
                (from object item in ckTask.CheckedItems select item.ToString().Split(':')[0].Convert<long>()).ToArray();
            query.Query<TaskEntity>().Where(it => ids.Contains(it.Id));
            return Ioc.Resolve<IApplicationService, TaskEntity>().GetEntities<TaskEntity>(query);
        }


        /// <summary>
        /// 添加错误信息
        /// </summary>
        protected virtual void AddErrorInfo(Exception ex, TaskEntity task)
        {
            if (ex == null) return;
            var info = new ErrorEntity
                {
                    Address = task.ClassName,
                    Device= task.Args,
                    Ip = "127.0.0.1",
                    SaveType = SaveType.Add
                };
            info.SetEntity(ex);
            info.Account = new AccountEntity { Id = 0};
            Ioc.Resolve<IApplicationService, EntityInfo>().Save(info);
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {
            var rev = Winner.Creator.Get<IContext>().Flush("TaskEntity");
            MessageBox.Show(rev ? "清除成功" : "清除失败");
        }

        #endregion

        #region 调度设置

        private const string TaskFileName = "task.txt";

        /// <summary>
        /// 加载调度服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadTasks();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ckTask.Items.Count; i++)
            {
                ckTask.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ckTask.Items.Count; i++)
            {
                ckTask.SetItemChecked(i, false);
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var builder = new StringBuilder();
                foreach (var item in ckTask.CheckedItems)
                {
                    builder.AppendLine(item.ToString());
                }
                if (!File.Exists(TaskFileName))
                    File.Create(TaskFileName).Dispose();
                File.WriteAllText(TaskFileName, builder.ToString());
                MessageBox.Show(@"保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存失败：{0}", ex.Message));
            }

        }

        /// <summary>
        /// 加载调度服务
        /// </summary>
        protected virtual void LoadTasks()
        {
            try
            {
                ckTask.Items.Clear();
                var query =
                    new QueryInfo().From<TaskEntity>();
                var infos = Ioc.Resolve<IApplicationService, TaskEntity>().GetEntities<TaskEntity>(query);
                if (infos == null)
                    return;
                if (!File.Exists(TaskFileName))
                    File.Create(TaskFileName).Dispose();
                var items = File.ReadAllLines(TaskFileName);
                foreach (var info in infos)
                {
                    var value = string.Format("{0}:{1}", info.Id, info.Name);
                    ckTask.Items.Add(value, items.Contains(value));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载失败：{0}", ex.Message));
            }

        }

        #endregion

        private void btnExcute_Click(object sender, EventArgs e)
        {
            if (ckTask.SelectedItem == null)
            {
                MessageBox.Show("请选择要执行的任务");
                return;
            }
            var info = Ioc.Resolve<IApplicationService, TaskEntity>()
                .GetEntity<TaskEntity>(ckTask.SelectedItem.ToString().Split(':')[0].Convert<long>());
            if (info == null)
            {
                MessageBox.Show("任务不存在");
                return;
            }
            try
            {
                Ioc.Resolve<IJobApplicationService>(info.ClassName).Execute(info.ArgsArray);
            }
            catch (Exception ex)
            {
                AddErrorInfo(ex, info);
                MessageBox.Show(string.Format("执行异常:{0}", ex.Message));
            }
            MessageBox.Show("执行成功");
        }

    }
}
