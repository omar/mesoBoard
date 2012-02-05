using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class FileServices : BaseService
    {
        IRepository<Attachment> _attachmentRepository;
        IRepository<FileType> _fileTypeRepository;

        public FileServices(
            IRepository<Attachment> attachmentRepository, 
            IRepository<FileType> fileTypeRepository, 
            IRepository<UserProfile> userProfileRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _attachmentRepository = attachmentRepository;
            _fileTypeRepository = fileTypeRepository;
        }

        public Attachment GetAttachment(int attachmentID)
        {
            return _attachmentRepository.Get(attachmentID);
        }

        public bool ValidFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            var fileType = _fileTypeRepository.First(item => item.Extension.Equals(extension));

            return fileType != null;
        }

        public void LogAttachmentDownload(int attachmentID)
        {
            Attachment attachment = _attachmentRepository.Get(attachmentID);
            attachment.Downloaded++;
            _attachmentRepository.Update(attachment);
            _unitOfWork.Commit();
        }

        public IEnumerable<Attachment> GetPostAttachments(int postID)
        {
            return _attachmentRepository.Where(item => item.PostID.Equals(postID)).ToList();
        }

        public string UploadFile(HttpPostedFileBase file)
        {
            string filePath = Path.Combine(HostingEnvironment.MapPath(DirectoryPaths.Attachments), file.FileName);

            string savedName = file.FileName;

            if (File.Exists(filePath))
                savedName = Randoms.CleanGUID() + file.FileName;
            string path = HostingEnvironment.MapPath(Path.Combine(DirectoryPaths.Attachments, savedName));
            file.SaveAs(path);

            return savedName;
        }

        public void CreateAttachments(HttpFileCollectionBase files, int postID)
        {
            foreach (string filename in files)
            {
                HttpPostedFileBase f = files[filename];
                if (f.ContentLength > 0)
                {
                    CreateAttachment(f, postID);
                }
            }
        }

        public void CreateAttachments(HttpPostedFileBase[] files, int postID)
        {
            foreach (var file in files)
            {
                CreateAttachment(file, postID);
            }
        }

        public void CreateAttachment(HttpPostedFileBase file, int PostID)
        {
            string savedName = UploadFile(file);

            var attachment = new Attachment
            {
                Downloaded = 0,
                SavedName = savedName,
                DownloadName = file.FileName,
                PostID = PostID,
                Size = file.ContentLength,
                Type = file.ContentType
            };
            _attachmentRepository.Add(attachment);
            _unitOfWork.Commit();
        }

        public void DeleteAttachments(IEnumerable<Attachment> attachments)
        {
            var items = attachments.ToList();
            foreach (var attachment in items)
            {
                DeleteAttachment(attachment);
            }
        }

        public void DeleteAttachment(int attachmentID)
        {
            Attachment attachment = _attachmentRepository.Get(attachmentID);
            DeleteAttachment(attachment);
        }

        public void DeleteAttachments(int[] attachmentIDs)
        {
            foreach (int i in attachmentIDs)
                DeleteAttachment(i);
        }

        public void DeleteAttachment(Attachment attachment)
        {
            string filePath = Path.Combine(HostingEnvironment.MapPath(DirectoryPaths.Attachments), attachment.SavedName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _attachmentRepository.Delete(attachment.AttachmentID);
            _unitOfWork.Commit();
        }

        public string UploadAvatar(HttpPostedFileBase avatar)
        {
            string random_name = Randoms.CleanGUID() + ".png";

            while (File.Exists(Path.Combine(HostingEnvironment.MapPath(DirectoryPaths.Avatars), random_name)))
                random_name = Randoms.CleanGUID() + ".png";

            string filePath = Path.Combine(HostingEnvironment.MapPath(DirectoryPaths.Avatars), random_name);

            int maxHeight = SiteConfig.AvatarHeight.ToInt();
            int maxWidth = SiteConfig.AvatarWidth.ToInt();
            
            using (Image ravatar = Image.FromStream(avatar.InputStream, true, true))
            {
                if (ravatar.Width > maxWidth || ravatar.Height > maxHeight)
                {
                    double ratio = (double)ravatar.Width / ravatar.Height;
                    double newHeight;
                    double newWidth;

                    if (ravatar.Width > ravatar.Height)
                    {
                        ratio = 1 / ratio;
                        newWidth = maxWidth;
                        newHeight = maxHeight * ratio;
                    }
                    else
                    {
                        newWidth = maxWidth * ratio;
                        newHeight = maxHeight;
                    }

                    using (var resizedAvatar = ravatar.GetThumbnailImage((int)newWidth, (int)newHeight, null, IntPtr.Zero))
                    {
                        resizedAvatar.Save(filePath, ImageFormat.Png);
                    };
                }
                else
                    ravatar.Save(filePath, ImageFormat.Png);
            }
            return random_name;
        }
    }
}