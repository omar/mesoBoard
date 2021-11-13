using System.Collections.Generic;
using System.IO;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class FileTypeServices : BaseService
    {
        private IRepository<FileType> FileTypes;

        public FileTypeServices(
            IRepository<FileType> fileTypes,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            FileTypes = fileTypes;
        }

        public bool ValidFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return FileTypes.First(item => item.Extension.Equals(extension)) != null;
        }

        public List<FileInfo> GetFileTypeImages()
        {
            DirectoryInfo fileTypeImagesDirectory = new DirectoryInfo(DirectoryPaths.FileTypes);
            List<FileInfo> fileTypeImages = new List<FileInfo>();
            string[] fileExtensions = { ".png", ".gif" };

            foreach (string e in fileExtensions)
            {
                fileTypeImages.AddRange(fileTypeImagesDirectory.GetFiles("*" + e));
            }

            return fileTypeImages;
        }
    }
}