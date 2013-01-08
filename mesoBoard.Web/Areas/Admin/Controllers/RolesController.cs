using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using mesoBoard.Web.Areas.Admin.ViewModels;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class RolesController : BaseAdminController
    {
        private RoleServices _roleServices;
        private IRepository<Role> _roleRepository;
        private IRepository<InRole> _inRoleRepository;
        private IRepository<User> _userRepository;
        private IRepository<Rank> _rankRepository;
        private User _currentUser;

        public RolesController(
            RoleServices roleServices,
            IRepository<Role> roleRepository,
            IRepository<InRole> inRoleRepository,
            IRepository<User> userRepository,
            IRepository<Rank> rankRepository,
            User currentUser)
        {
            _roleServices = roleServices;
            _roleRepository = roleRepository;
            _inRoleRepository = inRoleRepository;
            _userRepository = userRepository;
            _rankRepository = rankRepository;
            _currentUser = currentUser;
            SetCrumb("Roles");
        }

        public ActionResult Roles()
        {
            IEnumerable<Role> roles = _roleRepository.Get().ToList();
            RolesViewer model = new RolesViewer()
            {
                Roles = roles,
                RoleViewModel = new RoleViewModel()
                {
                    Ranks = _rankRepository.Where(x => x.IsRoleRank == true).ToList()
                }
            };
            return View(model);
        }

        public ActionResult RoleDetails(int RoleID)
        {
            IEnumerable<User> users = _inRoleRepository.Where(item => item.RoleID.Equals(RoleID)).Select(item => item.User).ToList();
            Role role = _roleRepository.Get(RoleID);
            RoleViewModel model = new RoleViewModel()
            {
                IsGroup = role.IsGroup,
                Name = role.Name,
                RankID = role.RankID,
                RoleID = role.RoleID,
                Ranks = _rankRepository.Where(x => x.IsRoleRank == true).ToList(),
                SpecialPermissions = role.SpecialPermissions,
                Users = users
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToUserRole(string Username, int RoleID)
        {
            User user = _userRepository.First(item => item.UsernameLower == Username.ToLower());
            if (user != null)
            {
                InRole inRole = _inRoleRepository.First(item => item.UserID == user.UserID && item.RoleID == RoleID);
                if (inRole == null)
                {
                    _inRoleRepository.Add(new InRole()
                    {
                        UserID = user.UserID,
                        RoleID = RoleID
                    });
                    SetSuccess(Username + " was added to role");
                }
                else
                    SetError(Username + " is already in role");
            }
            else
                SetError(Username + " is not a valid username");

            return RedirectToAction("RoleDetails", new { RoleID = RoleID });
        }

        public ActionResult RemoveFromRole(int UserID, int RoleID)
        {
            InRole inRole = _inRoleRepository.First(item => item.UserID.Equals(UserID) && item.RoleID.Equals(RoleID));
            if (inRole.UserID == _currentUser.UserID && inRole.Role.SpecialPermissions == (byte)SpecialPermissionValue.Administrator)
            {
                SetError("You can't remove your self from an administrator role");
            }
            else
            {
                _inRoleRepository.Delete(inRole);
                SetSuccess("User removed from role");
            }
            return RedirectToAction("RoleDetails", new { RoleID = RoleID });
        }

        public ActionResult CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_roleRepository.First(item => item.Name == model.Name) != null)
                    SetError("Role name is a duplicate");
            }

            if (IsModelValidAndPersistErrors())
            {
                Role role = new Role()
                {
                    IsGroup = model.IsGroup,
                    Name = model.Name,
                    RankID = model.RankID,
                    SpecialPermissions = model.SpecialPermissions,
                };
                _roleRepository.Add(role);
                SetSuccess("Role added");
            }

            return RedirectToAction("Roles");
        }

        [HttpPost]
        public ActionResult DeleteRole(int RoleID)
        {
            _roleRepository.Delete(RoleID);
            SetSuccess("Role deleted");
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public ActionResult EditRole(RoleViewModel model)
        {
            Role roleTest = _roleRepository.First(item => item.Name.Equals(model.Name));

            if (ModelState.IsValid)
            {
                if (roleTest != null && roleTest.RoleID != model.RoleID)
                    ModelState.AddModelError("Name", "A role with that name already exists");

                // If the edit will lower special permissions from admin,
                // make sure that the user doesn't remove admin permissions from themself
                if (roleTest.SpecialPermissions == (byte)SpecialPermissionValue.Administrator && model.SpecialPermissions != (byte)SpecialPermissionValue.Administrator)
                {
                    // Get all the roles that grant the user administrator permissions
                    IEnumerable<Role> administratorRoles = _currentUser.InRoles.Where(item => item.Role.SpecialPermissions == (byte)SpecialPermissionValue.Administrator).Select(item => item.Role);

                    // If there are multiple roles, the user can still access the admin cp
                    if (administratorRoles.Count() == 1)
                    {
                        Role administratorRole = administratorRoles.FirstOrDefault();
                        // If it's only one role that grants the user admin permission,
                        // make sure it's not the one being edited
                        if (administratorRole.RoleID == roleTest.RoleID)
                        {
                            ModelState.AddModelError("SpecialPermissions", "");
                            SetError("You can't remove administrator permissions from the ONLY role that is allowing you to access the administrator control panel");
                        }
                    }
                }
            }

            if (IsModelValidAndPersistErrors())
            {
                Role role = _roleRepository.Get(model.RoleID);
                role.IsGroup = model.IsGroup;
                role.Name = model.Name;
                role.RankID = model.RankID;
                role.SpecialPermissions = model.SpecialPermissions;
                _roleRepository.Update(role);
                SetSuccess("Role updated");
            }

            return RedirectToAction("RoleDetails", new { RoleID = model.RoleID });
        }

        public ActionResult SetRegistrationRole(int roleID)
        {
            _roleServices.SetRegistrationRole(roleID);
            SetSuccess("Registration role changed");
            return RedirectToAction("Roles");
        }
    }
}