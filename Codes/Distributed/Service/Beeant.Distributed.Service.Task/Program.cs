using System;
using System.Windows.Forms;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Dependent;
using Winner.Persistence;

namespace Beeant.Distributed.Service.Task
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常  
                System.Windows.Forms.Application.ThreadException += Application_ThreadException;
                //处理非UI线程异常  
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new Form1());
            }
            catch (Exception ex)
            {

                AddError(ex);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            AddError(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AddError(e.ExceptionObject as Exception);
        }
        /// <summary>
        /// 添加日志 
        /// </summary>
        /// <param name="ex"></param>
        public static void AddError(Exception ex)
        {
            if (ex == null) return;
            var info = new ErrorEntity
            {
                Address = "Beeant.Distributed.Service.Task",
                Device = "",
                Ip = "127.0.0.1",
                SaveType = SaveType.Add
            };
            info.SetEntity(ex);
            info.Account = new AccountEntity { Id = 0 };
            Ioc.Resolve<IApplicationService, EntityInfo>().Save(info);
        }
    }
}
