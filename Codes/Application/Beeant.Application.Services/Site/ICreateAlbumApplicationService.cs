namespace Beeant.Application.Services.Site
{
    public interface ICreateAlbumApplicationService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="albumId"></param>
        bool Add(long siteId, long albumId);
    }
}
