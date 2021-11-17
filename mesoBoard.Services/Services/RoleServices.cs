using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class RoleServices : BaseService
    {
        private IRepository<Config> _configRepository;
        private IRepository<InRole> _inRoleRepository;
        private IRepository<User> _userRepository;
        private SiteConfig _siteConfig;

        public RoleServices(
            IRepository<Config> configRepository,
            IRepository<InRole> inRoleRepository,
            IRepository<User> userRepository,
            SiteConfig siteConfig,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _configRepository = configRepository;
            _inRoleRepository = inRoleRepository;
            _userRepository = userRepository;
            _siteConfig = siteConfig;
        }

        public IEnumerable<Role> GetUserRoleRanks(int userID)
        {
            return _inRoleRepository.Where(item => item.UserID == userID).Select(item => item.Role).Where(item => item.RankID.HasValue).ToList();
        }

        public bool UserHasSpecialPermissions(int userID, params SpecialPermissionValue[] permission)
        {
            User user = _userRepository.Get(userID);

            var userInRoles = _inRoleRepository.Where(item => item.UserID.Equals(userID)).ToList();

            foreach (var perm in permission)
            {
                if (userInRoles.Any(x => x.Role.SpecialPermissions == (int)perm))
                    return true;
            }
            return false;
        }

        public void SetRegistrationRole(int roleID)
        {
            Config registrationRole = _configRepository.Get(SiteConfig.RegistrationRole.ConfigID);
            registrationRole.Value = roleID.ToString();
            _configRepository.Update(registrationRole);
            _unitOfWork.Commit();
            _siteConfig.UpdateCache();
        }
    }
}