using System;
using Beeant.Domain.Entities.Member;
using Winner.Persistence;

namespace Beeant.Presentation.Mobile.Member.Models.Member
{
    public class MemberModel
    {
        /// <summary>
        /// 会员
        /// </summary>
        public MemberEntity Member { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Postal { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public virtual MemberEntity CreateMember(long memberId)
        {
            var member = new MemberEntity
            {
                Id = memberId,
                Nickname = Nickname,
                Gender = Gender,
                Birthday = Birthday,
                Telephone = Telephone,
                IdCardNumber = IdCardNumber,
                Postal = Postal,
                Address = Address,
                HeadUrl = "",
                Remark = Remark
            };
            member.SaveType = memberId == 0 ? SaveType.Add : SaveType.Modify;
            if (member.SaveType == SaveType.Modify)
            {
                member.SetProperty(it => it.Nickname).SetProperty(it => it.Gender)
                    .SetProperty(it => it.Birthday).SetProperty(it => it.Telephone)
                    .SetProperty(it => it.IdCardNumber).SetProperty(it => it.Postal)
                    .SetProperty(it => it.Address).SetProperty(it => it.Remark);
            }
            return member;
        }

    }
}
