using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;


namespace Beeant.Cloud.Site.Mobile.Models.Book
{
    public class BookListModel : PagerModel
    {
        /// <summary>
        /// 公司信息
        /// </summary>
        public CompanyEntity Company { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public IList<BookEntity> Books { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public virtual void SetSize()
        {
            if(Company==null || Company.Album==null || Company.Album.Width==0 || Company.Album.Height==0)
                return;
            if (Company.Album.Width < Company.Album.Height)
            {
                Width = 800;
                Height = Company.Album.Height/Company.Album.Width*800;
            }
            else
            {
                Height = 1000;
                Height = Company.Album.Width / Company.Album.Height * 1000;
            }
        }

    }
}
