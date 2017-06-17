using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Product;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;

using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Ajax.Product
{
    public partial class Platform : AuthorizePageBase
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            switch (Request.QueryString["op"])
            {
                case "Load":
                    LoadTypes();
                    break;
                case "Synch":
                    Synchronize();
                    break;
                case "SynchRemove":
                    SynchRemove();
                    break;
                case "Remove":
                    Remove();
                    break;
            }
        }
        #region 加载
        /// <summary>
        /// 加载类型
        /// </summary>
        protected virtual void LoadTypes()
        {
            var infos = GetEntities();
            var builder = new StringBuilder("[");
            foreach (PlatformType item in Enum.GetValues(typeof (PlatformType)))
            {
                var info = infos == null ? null : infos.FirstOrDefault(it => it.Type == item);
                builder.Append("{");
                builder.AppendFormat("Type:{0},Name:'{1}',Id:{2}", (int) item, item.GetName(),
                                     info == null ? 0 : info.Id);
                builder.Append("},");
            }
            if (builder.Length > 1)
                builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            Response.Write(builder.ToString());
        }
        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PlatformEntity> GetEntities()
        {
            if (string.IsNullOrEmpty(Request.QueryString["goodsId"]))
                return null;
            var query = new QueryInfo();
            query.Query<PlatformEntity>().Where(it => it.Goods.Id == Request.QueryString["goodsId"].Convert<long>());
            var infos = Ioc.Resolve<IApplicationService, PlatformEntity>().GetEntities<PlatformEntity>(query);
            return infos;
        }
        #endregion

        #region 同步
        /// <summary>
        /// 加载类型
        /// </summary>
        protected virtual void Synchronize()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Type"]) || string.IsNullOrEmpty(Request.QueryString["goodsId"]))
            {
                Response.Write("参数错误");
                return;
            }
            var name = string.Format("I{0}PlatformApplicationService", ((PlatformType)Request.QueryString["Type"].Convert<int>()).ToString());
            var info=Ioc.Resolve<IPlatformApplicationService>(name).Synchronize(Request.QueryString["goodsId"].Convert<long>());
            if(info!=null && (info.Errors==null || info.Errors.Count==0))
            {
                  Response.Write("操作成功");
                return;
            }
            Response.Write(string.Format("操作失败:{0}", info.Errors.FirstOrDefault().Message));
        }
        #endregion

        #region 同步删除
        /// <summary>
        /// 加载类型
        /// </summary>
        protected virtual void SynchRemove()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Type"]) || string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Write("参数错误");
                return;
            }
            var name = string.Format("I{0}PlatformApplicationService", ((PlatformType)Request.QueryString["Type"].Convert<int>()).ToString());
            var info= Ioc.Resolve<IPlatformApplicationService>(name).Remove(Request.QueryString["Id"].Convert<long>());
            if (info != null && (info.Errors == null || info.Errors.Count == 0))
            {
                Response.Write("操作成功");
                return;
            }
            Response.Write(string.Format("操作失败:{0}", info.Errors.FirstOrDefault().Message));
        }
        #endregion

        #region 删除
        /// <summary>
        /// 加载类型
        /// </summary>
        protected virtual void Remove()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Write("参数错误");
                return;
            }
            var info = new PlatformEntity
                {
                    Id = Request.QueryString["Id"].Convert<long>(),
                    SaveType = SaveType.Remove
                };
            Ioc.Resolve<IApplicationService, PlatformEntity>().Save(info);
            if (info.Errors == null || info.Errors.Count == 0)
            {
                Response.Write("操作成功");
                return;
            }
            Response.Write(string.Format("操作失败:{0}", info.Errors.FirstOrDefault().Message));
        }
        #endregion
    }
}