using System.Collections.Generic;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Domain.Services.Basedata
{
    public class AlbumDomainService : RealizeDomainService<AlbumEntity>
    {

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FrontFileName",BytePropertyName = "FrontFileByte"},
               new FileEntity {FilePropertyName = "BackFileName",BytePropertyName = "BackFileByte"},
               new FileEntity {FilePropertyName = "AboutFileName",BytePropertyName = "AboutFileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
 
    }
}
