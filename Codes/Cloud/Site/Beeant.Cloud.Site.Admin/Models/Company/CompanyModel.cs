using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Company
{
    public class CompanyModel
    {
        /// <summary>
        /// 公司信息
        /// </summary>
        public CompanyEntity Company { get; set; }

        /// <summary>
        ///邮箱必填
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
 
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual CompanyEntity CreateEntity(SaveType saveType)
        {
            var entity = new CompanyEntity
            {
                Email = string.IsNullOrWhiteSpace(Email) ? "" : Email,
                Mobile = string.IsNullOrWhiteSpace(Mobile) ? "" : Mobile,
                Linkman = string.IsNullOrWhiteSpace(Linkman) ? "" : Linkman,
                Address = string.IsNullOrWhiteSpace(Address) ? "" : Address,
                Name = string.IsNullOrWhiteSpace(Name) ? "" : Name,
                Qq = string.IsNullOrWhiteSpace(Qq) ? "" : Qq,
                Fax = string.IsNullOrWhiteSpace(Fax) ? "" : Fax,
                Detail = string.IsNullOrWhiteSpace(Detail) ? "" : Detail,
                Album=new AlbumEntity(),
                SaveType = saveType
            };
            if (saveType == SaveType.Modify)
            {
                if (Email != null)
                    entity.SetProperty(it => it.Email);
                if (Mobile != null)
                    entity.SetProperty(it => it.Mobile);
                if (Linkman != null)
                    entity.SetProperty(it => it.Linkman);
                if (Address != null)
                    entity.SetProperty(it => it.Address);
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (Qq != null)
                    entity.SetProperty(it => it.Qq);
                if (Fax != null)
                    entity.SetProperty(it => it.Fax);
                if (Detail != null)
                    entity.SetProperty(it => it.Detail);
            }
            return entity;
        }
    }
}
