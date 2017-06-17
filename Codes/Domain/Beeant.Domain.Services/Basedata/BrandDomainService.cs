using System.Collections.Generic;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Basedata
{
    public class BrandDomainService : RealizeDomainService<BrandEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
   
    }
}
