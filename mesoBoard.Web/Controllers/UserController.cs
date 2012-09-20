using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework;
using mesoBoard.Framework.Core;
using mesoBoard.Framework.Models;
using mesoBoard.Services;

namespace mesoBoard.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private ThemeServices _themeServices;
        private UserServices _userServices;
        private ParseServices _parseServices;
        private MessageServices _messageServices;
        private EmailServices _emailServices;
        private FileServices _fileServices;
        private RoleServices _roleServices;
        private User _currentUser;

        public UserController(
            ThemeServices themes,
            UserServices users,
            ParseServices parseServices,
            MessageServices messageServices,
            EmailServices emailServices,
            FileServices fileServices,
            RoleServices roleServices,
            User currentUser)
        {
            _themeServices = themes;
            _userServices = users;
            _parseServices = parseServices;
            _messageServices = messageServices;
            _emailServices = emailServices;
            _fileServices = fileServices;
            _roleServices = roleServices;
            _currentUser = currentUser;
            SetTopBreadCrumb("User CP");
            SetBreadCrumb("User CP");
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            int messageCount = Request.IsAuthenticated ? _messageServices.GetUnreadMessages(_currentUser.UserID).Count() : 0;
            ViewData["NewMessagesCount"] = messageCount;
            return View("_Menu");
        }

        [HttpGet]
        [DefaultAction]
        public ActionResult Profile()
        {
            SetBreadCrumb("Modify Profile");

            var profile = _currentUser.UserProfile;

            // WTB Automapper 5k gold
            var model = new ProfileViewModel()
            {
                AIM = profile.AIM,
                AlwaysShowSignature = profile.AlwaysShowSignature,
                AlwaysSubscribeToThread = profile.AlwaysSubscribeToThread,
                Birthdate = profile.Birthdate,
                CanSelectTheme = !SiteConfig.OverrideUserTheme.ToBool(),
                Day = profile.Birthdate.HasValue ? profile.Birthdate.Value.Day : new Nullable<int>(),
                Month = profile.Birthdate.HasValue ? profile.Birthdate.Value.Month : new Nullable<int>(),
                Year = profile.Birthdate.HasValue ? profile.Birthdate.Value.Year : new Nullable<int>(),
                DefaultRankRole = profile.DefaultRole,
                ICQ = profile.ICQ,
                Location = profile.Location,
                MSN = profile.MSN,
                RankRoles = _roleServices.GetUserRoleRanks(_currentUser.UserID),
                ThemeID = profile.ThemeID,
                Themes = _themeServices.GetVisibleThemes(),
                Website = profile.Website,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Profile(ProfileViewModel model)
        {
            DateTime? birthdate = null;
            if (model.Day.HasValue || model.Month.HasValue || model.Year.HasValue)
            {
                if (!(model.Day.HasValue && model.Month.HasValue && model.Year.HasValue))
                    ModelState.AddModelError("Birthdate", "Enter the month, day and year of your birthdate.");
                else
                {
                    try
                    {
                        birthdate = new DateTime(model.Year.Value, model.Month.Value, model.Day.Value);
                    }
                    catch
                    {
                        ModelState.AddModelError("Birthdate", "Enter a valid month, day and year.");
                    }
                }
            }

            if (model.ThemeID.HasValue)
            {
                var themes = _themeServices.GetVisibleThemes();
                if (themes.FirstOrDefault(item => item.ThemeID == model.ThemeID) == null)
                    ModelState.AddModelError("ThemeID", "Invalid theme");
            }

            if (model.DefaultRankRole.HasValue)
            {
                var roles = _roleServices.GetUserRoleRanks(_currentUser.UserID);
                if (roles.FirstOrDefault(item => item.RoleID == model.DefaultRankRole) == null)
                    ModelState.AddModelError("DefaultRankRole", "Invalid role");
            }

            if (IsModelValidAndPersistErrors())
            {
                SetSuccess("Profile updated");
                _userServices.UpdateProfile(
                    _currentUser.UserID,
                    model.AlwaysShowSignature,
                    model.AlwaysSubscribeToThread,
                    model.Location,
                    model.ThemeID,
                    model.DefaultRankRole,
                    model.AIM,
                    model.ICQ,
                    model.MSN,
                    model.Website,
                    birthdate);
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult Signature()
        {
            SetBreadCrumb("Signature");

            var userProfile = _currentUser.UserProfile;
            var model = new SignatureViewModel()
            {
                ParsedSignature = userProfile.ParsedSignature,
                Signature = userProfile.Signature
            };

            if (TempData.ContainsKey("Preview_Signature"))
            {
                model.Preview = true;
                model.PreviewParsedSignature = (string)TempData["Preview_ParsedSignature"];
                model.Signature = (string)TempData["Preview_Signature"];
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Signature(SignatureViewModel model)
        {
            if (!model.Preview.HasValue)
            {
                _currentUser.UserProfile.Signature = model.Signature;
                _currentUser.UserProfile.ParsedSignature = _parseServices.ParseBBCodeText(model.Signature);
                _userServices.UpdateSignature(_currentUser.UserID, model.Signature);
                SetSuccess("Signature Saved");
            }
            else
            {
                TempData["Preview_ParsedSignature"] = _parseServices.ParseBBCodeText(model.Signature);
                TempData["Preview_Signature"] = model.Signature;
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult Password()
        {
            SetBreadCrumb("Password");
            var model = new PasswordViewModel()
            {
                MinimumPasswordLength = SiteConfig.PasswordMin.ToInt()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Password(PasswordViewModel model, string Password, string NewPassword, string ConfirmPassword)
        {
            SetBreadCrumb("Password");

            if (!_userServices.ValidatePassword(_currentUser, model.CurrentPassword))
                ModelState.AddModelError("CurrentPassword", "Invalid password.");

            if (IsModelValidAndPersistErrors())
            {
                _userServices.UpdatePassword(_currentUser.UserID, NewPassword);
                _emailServices.PasswordChanged(_currentUser, NewPassword);
                SetSuccess("Password changed. An email confirmation has been sent");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult Email()
        {
            SetBreadCrumb("Email");
            var model = new EmailViewModel()
            {
                CurrentEmail = _currentUser.Email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Email(EmailViewModel model)
        {
            SetBreadCrumb("Email");

            if (_userServices.EmailInUse(model.NewEmail))
                ModelState.AddModelError("NewEmail", "This email is already in use by another user");

            if (IsModelValidAndPersistErrors())
            {
                _currentUser.Email = model.NewEmail;
                _userServices.UpdateEmail(_currentUser.UserID, model.NewEmail);
                SetSuccess("Email changed");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult Avatar()
        {
            SetBreadCrumb("Avatar");

            var model = new AvatarViewModel()
            {
                HeightMax = SiteConfig.AvatarHeight.ToInt(),
                WidthMax = SiteConfig.AvatarWidth.ToInt()
            };
            switch (_currentUser.UserProfile.AvatarType)
            {
                case "Upload":
                    model.AvatarType = AvatarType.Upload;
                    break;

                case "Url":
                    model.AvatarType = AvatarType.Url;
                    model.Url = _currentUser.UserProfile.Avatar;
                    break;

                case "None":
                default:
                    model.AvatarType = AvatarType.None;
                    break;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Avatar(AvatarViewModel model, string ExternalImageURL)
        {
            switch (model.AvatarType)
            {
                case AvatarType.Url:
                    if (string.IsNullOrWhiteSpace(model.Url))
                        ModelState.AddModelError("Url", "Enter an avatar url.");
                    break;

                case AvatarType.Upload:
                    List<string> validFormats = new List<string>
                    {
                        "image/png", "image/x-png",
                        "image/gif", "image/x-gif",
                        "image/jpeg", "image/x-jpeg",
                        "image/jpg", "image/x-jpg",
                    };

                    if (model.Image == null || model.Image.ContentLength == 0)
                        ModelState.AddModelErrorFor<AvatarViewModel>(m => m.Image.FileName, "Please select an image to upload.");
                    else if (!validFormats.Contains(model.Image.ContentType))
                        ModelState.AddModelErrorFor<AvatarViewModel>(m => m.Image, "Invalid file format; only gif,png,jpeg,jpg are allowed.");
                    break;

                case AvatarType.None:
                default:
                    break;
            }

            if (IsModelValidAndPersistErrors())
            {
                switch (model.AvatarType)
                {
                    case AvatarType.Upload:
                        string uploadedFileName = _fileServices.UploadAvatar(model.Image);
                        _userServices.UpdateAvatarToUpload(_currentUser.UserID, uploadedFileName);
                        break;

                    case AvatarType.Url:
                        _userServices.UpdateAvatarToUrl(_currentUser.UserID, model.Url);
                        break;

                    case AvatarType.None:
                    default:
                        _userServices.UpdateAvatarToNone(_currentUser.UserID);
                        break;
                }
                SetSuccess("Profile Settings Updated");
            }

            return RedirectToSelf();
        }
    }
}