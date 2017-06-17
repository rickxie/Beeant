using Winner.Storage;

namespace Beeant.Distributed.Service.Host.Service
{
    public class FileService : Winner.Storage.FileService
    {
        public FileService()
        {
            var fileService = Winner.Creator.Get<IFileContract>() as Winner.Storage.FileService;
            File = Winner.Creator.Get<IFile>("Winner.Storage.IIamgeFile");
            Master = fileService.Master;
            Master.StartException();
        }


    }
}
