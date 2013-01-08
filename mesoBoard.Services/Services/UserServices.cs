using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class UserServices : BaseService
    {
        private IRepository<User> _userRepository;
        private IRepository<Message> _messageRepository;
        private IRepository<OnlineUser> _onlineUserRepository;
        private IRepository<OnlineGuest> _onlineGuestRepository;
        private IRepository<ThreadViewStamp> _threadViewStampRepository;
        private IRepository<UserProfile> _userProfileRepository;
        private IRepository<PasswordResetRequest> _passwordResetRequestRepository;
        private ParseServices _parseServices;

        public UserServices(
            IRepository<User> userRepository,
            IRepository<Message> messageRepository,
            IRepository<OnlineUser> onlineUserRepository,
            IRepository<OnlineGuest> onlineGuestRepository,
            IRepository<ThreadViewStamp> threadViewStampRepository,
            IRepository<UserProfile> userProfileRepository,
            IRepository<PasswordResetRequest> passwordResetRequestRepository,
            IRepository<InRole> inRolesRepository,
            ParseServices parseServices,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _onlineUserRepository = onlineUserRepository;
            _onlineGuestRepository = onlineGuestRepository;
            _threadViewStampRepository = threadViewStampRepository;
            _userProfileRepository = userProfileRepository;
            _passwordResetRequestRepository = passwordResetRequestRepository;
            _parseServices = parseServices;
        }

        public IEnumerable<User> GetBirthdays(DateTime date)
        {
            return _userProfileRepository.Where(item => item.Birthdate.HasValue && item.Birthdate.Value.Month == date.Month && item.Birthdate.Value.Day == date.Day).Select(item => item.User).ToList();
        }

        public bool ActivateUser(string userName, string activationCode)
        {
            if (!UserExists(userName))
                return false;

            User user = GetUser(userName);

            if (string.IsNullOrEmpty(user.ActivationCode))
                return true;

            if (user.ActivationCode == activationCode)
            {
                user.ActivationCode = string.Empty;
                _userRepository.Update(user);
                _unitOfWork.Commit();
                return true;
            }
            else
                return false;
        }

        public bool UserExists(string userNameorID)
        {
            if (string.IsNullOrWhiteSpace(userNameorID))
                return false;

            int userID;
            User user;
            bool IsInt = int.TryParse(userNameorID, out userID);
            if (IsInt)
                user = _userRepository.Get(userID);
            else
                user = _userRepository.First(item => item.UsernameLower.Equals(userNameorID));

            return user != null;
        }

        public bool ValidatePassword(string userName, string password)
        {
            User user = GetUser(userName);
            return ValidatePassword(user, password);
        }

        public bool ValidatePassword(int userID, string password)
        {
            User user = GetUser(userID);
            return ValidatePassword(user, password);
        }

        public bool ValidatePassword(User user, string password)
        {
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + user.PasswordSalt, "MD5");
            return user.Password == hashedPassword;
        }

        public bool EmailInUse(string email)
        {
            return _userRepository.First(item => item.Email.Equals(email)) != null;
        }

        public void LoginRoutine(User user, string ipAddress)
        {
            user.LastLoginDate = DateTime.UtcNow;
            user.LastLoginIP = ipAddress;

            OnlineGuest guest = _onlineGuestRepository.First(item => item.IP.Equals(ipAddress));

            if (guest != null)
                _onlineGuestRepository.Delete(guest);

            OnlineUser oUser = _onlineUserRepository.Get(user.UserID);
            if (oUser == null)
            {
                _onlineUserRepository.Add(new OnlineUser
                {
                    Date = DateTime.UtcNow,
                    UserID = user.UserID
                });
            }
            else
            {
                oUser.Date = DateTime.UtcNow;
                _onlineUserRepository.Update(oUser);
            }
            IEnumerable<ThreadViewStamp> views = _threadViewStampRepository.Where(item => item.UserID.Equals(user.UserID));
            _threadViewStampRepository.Delete(views);
            _unitOfWork.Commit();
        }

        public void LogoutRoutine(int userID)
        {
            IEnumerable<ThreadViewStamp> views = _threadViewStampRepository.Where(item => item.UserID.Equals(userID));
            _threadViewStampRepository.Delete(views);
            OnlineUser onlineUser = _onlineUserRepository.First(item => item.UserID.Equals(userID));
            _onlineUserRepository.Delete(onlineUser);
            User user = GetUser(userID);
            user.LastLogoutDate = DateTime.UtcNow;
            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public User GetUser(int userID)
        {
            return _userRepository.Get(userID);
        }

        public User GetUser(string userNameOrEmail)
        {
            if (EmailInUse(userNameOrEmail))
                return _userRepository.First(item => item.Email.Equals(userNameOrEmail));
            else
                return _userRepository.First(item => item.UsernameLower.Equals(userNameOrEmail));
        }

        public string ResetPassword(int userID)
        {
            User user = GetUser(userID);

            string newPass = Randoms.RandomPassword();
            string salt = Randoms.CreateSalt();
            string hashedPass = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass + salt, "MD5");

            user.Password = hashedPass;
            user.PasswordSalt = salt;

            _userRepository.Update(user);
            _unitOfWork.Commit();
            return newPass;
        }

        public User UpdatePassword(int userID, string newPassword)
        {
            string newSalt = Randoms.CreateSalt();
            string newHashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword + newSalt, "MD5");

            User user = GetUser(userID);

            user.Password = newHashedPassword;
            user.PasswordSalt = newSalt;

            _userRepository.Update(user);
            _unitOfWork.Commit();
            return user;
        }

        public UserProfile UpdateSignature(int userID, string signature)
        {
            var userProfile = _userProfileRepository.Get(userID);
            userProfile.Signature = signature;
            userProfile.ParsedSignature = _parseServices.ParseBBCodeText(signature);
            _userProfileRepository.Update(userProfile);
            _unitOfWork.Commit();
            return userProfile;
        }

        public User UpdateEmail(int userID, string newEmail)
        {
            var user = _userRepository.Get(userID);
            user.Email = newEmail;
            _userRepository.Update(user);
            _unitOfWork.Commit();
            return user;
        }

        public UserProfile UpdateAvatarToNone(int userID)
        {
            return UpdateAvatar(userID, "None", string.Empty);
        }

        public UserProfile UpdateAvatarToUrl(int userID, string url)
        {
            return UpdateAvatar(userID, "Url", url);
        }

        public UserProfile UpdateAvatarToUpload(int userID, string uploadedFileName)
        {
            return UpdateAvatar(userID, "Upload", uploadedFileName);
        }

        private UserProfile UpdateAvatar(int userID, string avatarType, string avatar)
        {
            var userProfile = _userProfileRepository.Get(userID);
            userProfile.AvatarType = avatarType;
            userProfile.Avatar = avatar;
            _userProfileRepository.Update(userProfile);
            _unitOfWork.Commit();
            return userProfile;
        }

        public void UpdateProfile(
            int userID,
            bool alwaysShowSignature,
            bool alwaysSubscribeToThread,
            string location,
            int? themeID,
            int? defaultRankRole,
            string aim,
            int? icq,
            string msn,
            string website,
            DateTime? birthdate)
        {
            UserProfile userProfile = _userProfileRepository.First(item => item.UserID == userID);
            userProfile.AlwaysShowSignature = alwaysShowSignature;
            userProfile.Location = location;
            userProfile.ThemeID = themeID;
            userProfile.DefaultRole = defaultRankRole;
            userProfile.AIM = aim;
            userProfile.ICQ = icq;
            userProfile.MSN = msn;
            userProfile.Website = website;
            userProfile.Birthdate = birthdate;
            userProfile.AlwaysSubscribeToThread = alwaysSubscribeToThread;
            _userProfileRepository.Update(userProfile);
            _unitOfWork.Commit();
        }

        public User Register(string username, string password, string email)
        {
            string activationType = SiteConfig.AccountActivation.Value;
            string salt = Randoms.CreateSalt() + DateTime.UtcNow.ToString();
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "MD5");

            var user = new User()
            {
                Password = hashedPassword,
                PasswordSalt = salt,
                RegisterDate = DateTime.UtcNow,
                RegisterIP = HttpContext.Current.Request.UserHostAddress,
                ActivationCode = activationType == "None" ? string.Empty : Randoms.CleanGUID(),
                Status = false,
                LastLoginIP = null,
                Username = username,
                UsernameLower = username.ToLower(),
                LastLoginDate = DateTime.UtcNow,
                LastLogoutDate = DateTime.UtcNow,
                LastPostDate = DateTime.UtcNow,
                Email = email
            };

            user.UserProfile = new UserProfile()
            {
                AlwaysShowSignature = true,
                AlwaysSubscribeToThread = true,
                AvatarType = "None",
                ThemeID = SiteConfig.BoardTheme.ToInt()
            };

            var inRole = new InRole()
            {
                UserID = user.UserID,
                RoleID = SiteConfig.RegistrationRole.IntValue()
            };
            user.InRoles.Add(inRole);

            _userRepository.Add(user);
            _unitOfWork.Commit();
            return user;
        }

        public string RequestPasswordReset(int userID)
        {
            PasswordResetRequest request = _passwordResetRequestRepository.Get(userID);

            if (request != null)
                _passwordResetRequestRepository.Delete(request);

            PasswordResetRequest pwrr = new PasswordResetRequest
            {
                UserID = userID,
                Token = Randoms.CleanGUID(),
                Date = DateTime.UtcNow
            };

            _passwordResetRequestRepository.Add(pwrr);
            _unitOfWork.Commit();
            string token = userID.ToString() + "-" + pwrr.Token;

            return token;
        }

        public bool ValidatePasswordResetRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            string[] split = token.Split('-');
            int userID = int.Parse(split[0]);
            string code = split[1];

            PasswordResetRequest resetRequest = _passwordResetRequestRepository.Get(userID);

            if ((DateTime.UtcNow - resetRequest.Date).Minutes > 15)
                return false;

            return (resetRequest.UserID == userID && resetRequest.Token == code);
        }

        public void DeleteUser(int userID)
        {
            var messagesSent = _messageRepository.Where(item => item.FromUserID == userID);
            foreach (var message in messagesSent)
            {
                if (message.FromUserID == null && message.ToUserID == null)
                    _messageRepository.Delete(message);
                else
                {
                    message.FromUserID = null;
                    _messageRepository.Update(message);
                }
            }

            var messagesReceived = _messageRepository.Where(item => item.ToUserID == userID);
            foreach (var message in messagesSent)
            {
                if (message.FromUserID == null && message.ToUserID == null)
                    _messageRepository.Delete(message);
                else
                {
                    message.ToUserID = null;
                    _messageRepository.Update(message);
                }
            }

            _userRepository.Delete(userID);
            _unitOfWork.Commit();
        }

        public void DeletePasswordResetRequest(int userID)
        {
            _passwordResetRequestRepository.Delete(userID);
            _unitOfWork.Commit();
        }
    }
}