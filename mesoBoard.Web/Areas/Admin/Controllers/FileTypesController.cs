using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.ViewModels;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class FileTypesController : BaseAdminController
    {
        IRepository<FileType> _fileTypeRepository;
        FileServices _fileServices;

        public FileTypesController(IRepository<FileType> fileTypeRepository, FileServices fileServices)
        {
            _fileTypeRepository = fileTypeRepository;
            _fileServices = fileServices;
            SetCrumb("File Types");
        }


        public ActionResult FileTypes()
        {
            IEnumerable<FileType> fileTypes = _fileTypeRepository.Get();
            List<FileInfo> fileTypeImages = _fileServices.GetFileTypeImages();

            FileTypeViewModel model = new FileTypeViewModel()
            {
                FileTypes = fileTypes,
                Images = fileTypeImages.Select(item => item.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult FileTypes(FileTypeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                FileType fileType = new FileType()
                {
                    Extension = model.Extension,
                    Image = model.Image
                };
            
                _fileTypeRepository.Add(fileType);
                SetSuccess("File type created");
            }

            return RedirectToSelf();
        }

        [HttpPost]
        public ActionResult DeleteFileType(int FileTypeID)
        {
            _fileTypeRepository.Delete(FileTypeID);
            SetSuccess("File type deleted");
            return RedirectToAction("FileTypes");
        }

        [HttpGet]
        public ActionResult EditFileType(int FileTypeID)
        {
            List<FileInfo> fileTypeImages = _fileServices.GetFileTypeImages();
            FileType fileType = _fileTypeRepository.Get(FileTypeID);

            FileTypeViewModel model = new FileTypeViewModel()
            {
                Extension = fileType.Extension,
                Image = fileType.Image,
                FileTypeID = fileType.FileTypeID,
                Images = fileTypeImages.Select(item => item.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditFileType(FileTypeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                FileType fileType = _fileTypeRepository.Get(model.FileTypeID);
                UpdateModel(fileType);
                _fileTypeRepository.Update(fileType);
                SetSuccess("File type edited");
                return RedirectToAction("FileTypes");
            }

            return RedirectToSelf();    
        }
    }
}
