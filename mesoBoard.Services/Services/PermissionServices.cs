using System.Linq;
using mesoBoard.Data;
using mesoBoard.Common;
using System.Collections.Generic;

namespace mesoBoard.Services
{
    public class PermissionServices : BaseService
    {
        IRepository<Thread> _threadRepository;
        IRepository<User> _userRepository;
        IRepository<Forum> _forumRepository;
        IRepository<ForumPermission> _forumPermissionRepository;
        ThreadServices _threadServices;
        RoleServices _roleServices;

        public PermissionServices(
            IRepository<Thread> threads,
            IRepository<User> users,
            IRepository<Forum> forums,
            IRepository<ForumPermission> forumPermissions,
            ThreadServices threadServices,
            RoleServices rolesService,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _threadRepository = threads;
            _userRepository = users;
            _forumRepository = forums;
            _forumPermissionRepository = forumPermissions;
            _threadServices = threadServices;
            _roleServices = rolesService;
        }


        public bool CanView(int forumID, int userID)
        {
            return HasVisibilityPermission(forumID, userID, VisibilityPermissionValue.Visible);
        }

        public bool CanReply(int forumID, int userID)
        {
            return HasPostingPermission(forumID, userID, PostingPermissionValue.Reply);
        }

        public bool CanCreateThread(int forumID, int userID)
        {
            return HasPostingPermission(forumID, userID, PostingPermissionValue.Thread);
        }

        public bool CanCreatePoll(int forumID, int userID)
        {
            return HasPollingPermission(forumID, userID, PollingPermissionValue.Create);
        }

        public bool CanCastVote(int forumID, int userID)
        {
            return HasPollingPermission(forumID, userID, PollingPermissionValue.Vote);
        }

        public bool CanCreateAttachment(int forumID, int userID)
        {
            return HasAttachmentPermission(forumID, userID, AttachmentPermissionValue.Upload);
        }

        public bool CanDownloadAttachment(int forumID, int userID)
        {
            return HasAttachmentPermission(forumID, userID, AttachmentPermissionValue.Download);
        }

        public bool CanLock(int forumID, int userID)
        {
            if (userID == 0)
                return false;
            return _roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Moderator, SpecialPermissionValue.Administrator);
        }

        public bool HasNewPost(int forumID, int userID)
        {
            if (userID == 0)
                return false;

            IEnumerable<Thread> thread = _threadRepository.Where(item => item.ForumID.Equals(forumID)).ToList();

            return thread.Any(x => _threadServices.HasNewPost(x.ThreadID, userID));
        }

        public IEnumerable<ThreadTypes> GetAllowedThreadTypes(int forumID, int userID)
        {
            int posting = GetPermissionValue(forumID, userID, Permission.Posting);
            return ThreadTypes.List.Where(item => item.Value <= posting - 1);
        }

        public bool CanCreateThreadType(int forumID, int userID, ThreadType type)
        {
            int posting = GetPermissionValue(forumID, userID, Permission.Posting);
            switch (type)
            {
                case ThreadType.Regular:
                case ThreadType.Sticky:
                case ThreadType.Announcement:
                case ThreadType.GlobalAnnouncement:
                    return posting >= (int)type;
                default:
                    return false;
            }
        }

        public UserPermissions GetUserPermissions(int forumID, int userID)
        {
            UserPermissions permission = new UserPermissions()
            {
                Posting = GetPermissionValue(forumID, userID, Permission.Posting),
                Polling = GetPermissionValue(forumID, userID, Permission.Polling),
                Attachment = GetPermissionValue(forumID, userID, Permission.Attachment),
                Visible = HasVisibilityPermission(forumID, userID, VisibilityPermissionValue.Visible)
            };

            return permission;
        }


        public int GetPermissionValue(int forumID, int userID, Permission permissionType)
        {
            Forum forum = _forumRepository.Get(forumID);

            if (userID == 0)
            {
                switch (permissionType)
                {
                    case Permission.Visibility:
                        return forum.VisibleToGuests ? VisibilityPermissions.Visible.Value : VisibilityPermissions.Hidden.Value;
                    case Permission.Posting:
                    case Permission.Polling:
                    case Permission.Attachment:
                        return forum.AllowGuestDownloads ? AttachmentPermissions.Download.Value : AttachmentPermissions.None.Value;
                    default:
                        return 0;
                }
            }
            else
            {
                IEnumerable<ForumPermission> forumPermissions = _forumPermissionRepository.Where(item => item.ForumID == forumID).ToList();

                if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator))
                    return 10;
                else if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Moderator))
                    return 5;

                User user = _userRepository.Get(userID);

                var permissions = from p in forumPermissions.AsQueryable()
                            where p.Role.InRoles.Any(item => item.UserID == userID)
                            select p;
                if (permissions.Count() == 0)
                    return 0;

                switch (permissionType)
                {
                    case Permission.Posting:
                        return permissions.Select(item => item.Posting).Max();
                    case Permission.Polling:
                        return permissions.Select(item => item.Polling).Max();
                    case Permission.Attachment:
                        return permissions.Select(item => item.Attachments).Max();
                    case Permission.Visibility:
                        return permissions.Select(item => item.Visibility).Max();
                    default:
                        return 0;
                }
            }
        }

        public bool HasAttachmentPermission(int forumID, int userID, AttachmentPermissionValue permission)
        {
            if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator))
                return true;

            var permissionValue = GetPermissionValue(forumID, userID, Permission.Attachment);
            return permissionValue >= (int)permission;
        }

        public bool HasPostingPermission(int forumID, int userID, PostingPermissionValue permission)
        {
            if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator))
                return true;

            var permissionValue = GetPermissionValue(forumID, userID, Permission.Posting);
            return permissionValue >= (int)permission;
        }

        public bool HasVisibilityPermission(int forumID, int userID, VisibilityPermissionValue permission)
        {
            if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator))
                return true;

            var permissionValue = GetPermissionValue(forumID, userID, Permission.Visibility);
            return permissionValue >= (int)permission;
        }

        public bool HasPollingPermission(int forumID, int userID, PollingPermissionValue permission)
        {
            if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator))
                return true;

            var permissionValue = GetPermissionValue(forumID, userID, Permission.Polling);
            return permissionValue >= (int)permission;
        }

    }
}