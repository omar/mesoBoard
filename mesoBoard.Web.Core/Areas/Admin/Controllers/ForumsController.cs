using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using mesoBoard.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class ForumsController : BaseAdminController
    {
        private IRepository<Category> _categoryRepository;
        private IRepository<Forum> _forumRepository;
        private ForumServices _forumServices;
        private IRepository<Role> _roleRepository;
        private IRepository<ForumPermission> _forumPermissionRepository;
        private IRepository<Attachment> _attachmentRepository;
        private FileServices _fileServices;
        private CategoryServices _categoryServices;

        public ForumsController(
            IRepository<Category> categoryRepository,
            IRepository<Forum> forumRepository,
            ForumServices forumServices,
            IRepository<Role> roleRepository,
            IRepository<ForumPermission> forumPermissionRepository,
            IRepository<Attachment> attachmentRepository,
            FileServices fileServices,
            CategoryServices categoryServices)
        {
            _categoryRepository = categoryRepository;
            _forumRepository = forumRepository;
            _forumServices = forumServices;
            _roleRepository = roleRepository;
            _forumPermissionRepository = forumPermissionRepository;
            _fileServices = fileServices;
            _attachmentRepository = attachmentRepository;
            _categoryServices = categoryServices;
            SetCrumb("Forums");
        }

        public ActionResult Forums()
        {
            return View(_categoryRepository.Get());
        }

        [HttpPost]
        public ActionResult DeleteForum(int ForumID)
        {
            var attachments = _attachmentRepository.Where(item => item.Post.Thread.ForumID == ForumID);
            _fileServices.DeleteAttachments(attachments);
            _forumRepository.Delete(ForumID);

            SetSuccess("Forum Deleted");
            return RedirectToAction("Forums");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int CategoryID)
        {
            var attachments = _attachmentRepository.Where(item => item.Post.Thread.Forum.CategoryID == CategoryID);
            _fileServices.DeleteAttachments(attachments);
            _categoryRepository.Delete(CategoryID);
            SetSuccess("Category Deleted");
            return RedirectToAction("Forums");
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            CategoryViewModel model = new CategoryViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                _categoryServices.CreateCategory(model.Name, model.Description);
                SetSuccess("Category created");
                return RedirectToAction("Forums");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditCategory(int categoryID)
        {
            Category category = _categoryRepository.Get(categoryID);
            CategoryViewModel model = new CategoryViewModel()
            {
                CategoryID = category.CategoryID,
                Description = category.Description,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                Category category = _categoryRepository.Get(model.CategoryID);
                category.Name = model.Name;
                category.Description = model.Description;
                _categoryRepository.Update(category);
                SetSuccess("Category Updated");
                return RedirectToAction("Forums");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult CreateForum()
        {
            if (_forumRepository.Get().Count() == 0)
            {
                SetNotice("You must create a category before you can create a forum");
                return RedirectToAction("Forums");
            }
            ForumViewModel model = new ForumViewModel()
            {
                Categories = _categoryRepository.Get()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateForum(ForumViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = _categoryRepository.Get(model.CategoryID);
                if (category == null)
                    ModelState.AddModelError("CategoryID", "Category does not exist.");
            }

            if (IsModelValidAndPersistErrors())
            {
                _forumServices.CreateForum(model.CategoryID, model.Name, model.Description, model.VisibleToGuests, model.AllowGuestDownloads);
                SetSuccess("Forum created");
                return RedirectToAction("Forums");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditForum(int ForumID)
        {
            Forum forum = _forumRepository.Get(ForumID);
            ForumViewModel model = new ForumViewModel()
            {
                CategoryID = forum.CategoryID,
                Categories = _categoryRepository.Get(),
                Description = forum.Description,
                ForumID = forum.ForumID,
                Name = forum.Name,
                VisibleToGuests = forum.VisibleToGuests,
                AllowGuestDownloads = forum.AllowGuestDownloads
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditForum(ForumViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = _categoryRepository.Get(model.CategoryID);

                if (category == null)
                    ModelState.AddModelError("CategoryID", "Category does not exist.");
            }

            if (IsModelValidAndPersistErrors())
            {
                Forum forum = _forumRepository.Get(model.ForumID);
                forum.Name = model.Name;
                forum.Description = model.Description;
                forum.CategoryID = model.CategoryID;
                forum.VisibleToGuests = model.VisibleToGuests;
                forum.AllowGuestDownloads = model.AllowGuestDownloads;
                _forumRepository.Update(forum);
                SetSuccess("Forum Updated");
                return RedirectToAction("Forums");
            }

            return RedirectToSelf();
        }

        public ActionResult Move(int Direction, int? ForumID, int CategoryID)
        {
            if (ForumID.HasValue)
                _forumServices.ChangeForumOrder(ForumID.Value, CategoryID, Direction);
            else
                _forumServices.ChangeCategoryOrder(CategoryID, Direction);

            SetSuccess("Order Changed");
            return RedirectToAction("Forums");
        }

        public ActionResult ForumPermissions(int ForumID)
        {
            Forum forum = _forumRepository.Get(ForumID);
            IEnumerable<ForumPermission> forumPermissions = _forumPermissionRepository.Where(item => item.ForumID == ForumID);
            ForumPermissionsViewer model = new ForumPermissionsViewer()
            {
                Forum = forum,
                ForumPermissions = forumPermissions
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteForumPermission(int ForumPermissionID)
        {
            ForumPermission forumPermission = _forumPermissionRepository.Get(ForumPermissionID);
            _forumPermissionRepository.Delete(forumPermission);
            SetSuccess("Forum permission deleted");
            return RedirectToAction("ForumPermissions", new { ForumID = forumPermission.ForumID });
        }

        [HttpGet]
        public ActionResult CreateForumPermission(int? ForumID)
        {
            ForumPermissionViewModel model = new ForumPermissionViewModel()
            {
                Forums = _forumRepository.Get(),
                Roles = _roleRepository.Get()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateForumPermission(ForumPermissionViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                ForumPermission permission = _forumPermissionRepository.First(item => item.ForumID == model.ForumID && item.RoleID == model.RoleID);
                if (permission != null)
                    SetError("Permission for the role/forum combination already exists");
                else
                {
                    ForumPermission forumPermission = new ForumPermission()
                    {
                        ForumID = model.ForumID,
                        RoleID = model.RoleID,
                        Attachments = model.Attachments,
                        Polling = model.Polling,
                        Posting = model.Posting,
                        Visibility = model.Visibility
                    };

                    _forumPermissionRepository.Add(forumPermission);
                    SetSuccess("Permission added");
                    return RedirectToAction("ForumPermissions", new { ForumID = model.ForumID });
                }
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditForumPermission(int ForumPermissionID)
        {
            ForumPermission forumPermission = _forumPermissionRepository.Get(ForumPermissionID);
            ForumPermissionViewModel model = new ForumPermissionViewModel()
            {
                Forums = _forumRepository.Get(),
                Roles = _roleRepository.Get(),

                ForumID = forumPermission.ForumID,
                ForumPermissionID = forumPermission.ForumPermissionID,
                RoleID = forumPermission.RoleID,
                Attachments = forumPermission.Attachments,
                Polling = forumPermission.Polling,
                Posting = forumPermission.Posting,
                Visibility = forumPermission.Visibility,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditForumPermission(ForumPermissionViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                ForumPermission forumPermission = _forumPermissionRepository.Get(model.ForumPermissionID);
                forumPermission.ForumID = model.ForumID;
                forumPermission.RoleID = model.RoleID;
                forumPermission.Attachments = model.Attachments;
                forumPermission.Polling = model.Polling;
                forumPermission.Posting = model.Posting;
                forumPermission.Visibility = model.Visibility;
                _forumPermissionRepository.Update(forumPermission);
                SetSuccess("Forum permissions updated");
                return RedirectToAction("ForumPermissions", new { ForumID = model.ForumID });
            }

            return RedirectToSelf(new { ForumPermissionID = model.ForumPermissionID });
        }
    }
}