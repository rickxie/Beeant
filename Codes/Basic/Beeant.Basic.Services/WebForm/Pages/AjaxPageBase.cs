using System;
using System.Collections.Generic;
using System.Text;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Dependent;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class AjaxPageBase<T> : AuthorizePageBase where T : BaseEntity, new()
    {
        public override bool IsImageRecover
        {
            get { return false; }
            set { base.IsImageRecover = value; }
        }
        public override bool IsVerifyResource
        {
            get { return false; }
        }


        protected virtual void Page_Load(object sender, EventArgs e)
        {
            WriteEntities();
        }

        protected virtual void WriteEntities()
        {
            try
            {
                var infos = GetEntities();
                Response.Write(GetResult(infos));
            }
            catch (Exception)
            {
                Response.Write("[]");
                throw;
            }
        }

        protected virtual void SetQuery(QueryInfo query)
        {
          
            SetQueryWhere(query);
            SetQuerySelect(query);
        }

        protected virtual void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Name";
        }

        protected virtual void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                query.Where("Name.StartsWith(@Name)");
                query.SetParameter("Name",Server.UrlDecode(Request.QueryString["name"]));
            }
  
        }

        protected virtual IList<T> GetEntities()
        {
            var query = new QueryInfo { FromExp = typeof(T).FullName, PageIndex = 0, PageSize = 10 };
            SetQuery(query);
            return Ioc.Resolve<IApplicationService,T>().GetEntities<T>(query);
        }

        protected virtual string GetResult(IList<T> infos)
        {
            if (infos == null || infos.Count == 0) return "[]";
            var builder = new StringBuilder();
            builder.Append("[");
            foreach (var info in infos)
            {
                var value = GetListItem(info);
                if(value==null)continue;
                builder.Append("{");
                builder.Append(value);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        protected virtual string GetListItem(T info)
        {
            return "";
        }
 
 
    }
}
