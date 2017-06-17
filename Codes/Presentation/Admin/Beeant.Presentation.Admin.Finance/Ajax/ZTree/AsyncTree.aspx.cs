using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Finance.Ajax.ZTree
{
    public partial class AsyncTree : AuthorizePageBase
    {
        protected string DefaultParentKey = "";
        protected string DataTextField = "";
        protected string DataValueField = "";
        protected string DataParentField = "";
        protected string ObjectFields = "";
        protected string ObjectName = "";
        protected string IdKey = "";
        protected bool ExpandAll = false;
        public override bool IsVerifyResource
        {
            get { return false; }
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            GetParams();
            ResponseObject();
        }

        protected void ResponseObject()
        {
            Response.Write(SerialzeToZTree(ConvertJsonEntity(GetEntities())));
            Response.End();
        }

        protected virtual void GetParams()
        {
            DefaultParentKey = Request.Params["DefaultParentKey"];
            DataTextField = Request.Params["DataTextField"];
            DataValueField = Request.Params["DataValueField"];
            DataParentField = Request.Params["DataParentField"];
            ObjectFields = Request.Params["ObjectFields"];
            ObjectName = Request.Params["ObjectName"];
            ExpandAll = Request.Params["ExpandAll"] == null ? false : Request.Params["ExpandAll"].Convert<bool>();
            IdKey = Request.Params[DataValueField] ?? string.Empty;
        }

        protected virtual IList<BaseEntity> GetEntities()
        {
            var query = new QueryInfo();
            query.From(ObjectName);
            query.SelectExp = ObjectFields;
            query.WhereExp = string.Format("{0}==@ParentKey", DataParentField);
            query.SetParameter("ParentKey", IdKey.Length > 0 ? IdKey : DefaultParentKey);
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(query);
        }
        /// <summary>
        /// Entity转Json字符串
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual string ConvertJsonEntity(IList<BaseEntity> infos)
        {
            var arr = new ArrayList();
            var fileds = ObjectFields.Split(',').ToList();
            infos.ToList().ForEach(item =>
            {
                var propertys = new Dictionary<string, object>();
                fileds.ForEach(field =>
                {
                    propertys.Add(field, Winner.Creator.Get<Winner.Base.IProperty>().GetValue<object>(item, field));
                });
                propertys.Add("isParent", "true");
                arr.Add(propertys);
            });
            return arr.SerializeJson();
        }
        /// <summary>
        /// 输出ZTree可接受的字符串
        /// </summary>
        /// <param name="serialzeObjects"></param>
        /// <returns></returns>
        protected virtual string SerialzeToZTree(string serialzeObjects)
        {
            serialzeObjects.Replace(string.Format("\"{0}\":null", DataParentField), string.Format("\"{0}\":\"{1}\"", DataParentField, "0"));
            if (ExpandAll)
                serialzeObjects = serialzeObjects.Replace("},", ",open:true},");
            return serialzeObjects.Replace(string.Format("\"{0}\":", DataTextField), "\"name\":");
        }
    }
}