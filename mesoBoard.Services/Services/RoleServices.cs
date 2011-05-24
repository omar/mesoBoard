using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class RoleServices 
    {
        IRepository<Config> _configRepository;
        IRepository<InRole> _inRoleRepository;
        IRepository<User> _userRepository;

        public RoleServices(
            IRepository<Config> configRepository, 
            IRepository<InRole> inRoleRepository, 
            IRepository<User> userRepository)   
        {
            _configRepository = configRepository;
            _inRoleRepository = inRoleRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<Role> GetUserRoleRanks(int userID)
        {
            return _inRoleRepository.Where(item => item.UserID.Equals(userID)).Select(item => item.Role).Where(item => item.RankID.HasValue);
        }

        public bool UserHasSpecialPermissions(int userID, params SpecialPermissionValue[] permission)
        {
            User user = _userRepository.Get(userID);

            IEnumerable<InRole> userInRoles = _inRoleRepository.Where(item => item.UserID.Equals(userID));

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
            SiteConfig.UpdateCache();
        }
    }
}
