using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class ThreadServices : BaseService
    {
        private IRepository<Thread> _threadRepository;
        private IRepository<ThreadView> _threadViewRepository;
        private IRepository<Post> _postRepository;
        private IRepository<User> _userRepository;
        private IRepository<ThreadViewStamp> _threadViewStampRepository;
        private IRepository<Subscription> _subscriptionRepository;
        private IRepository<Attachment> _attachmentRepository;
        private PollServices _pollServices;
        private FileServices _fileServices;
        private ParseServices _parseServices;
        private RoleServices _roleServices;

        public ThreadServices(
            IRepository<Thread> threadRepository,
            IRepository<ThreadView> threadViewRepository,
            IRepository<Post> postRepository,
            IRepository<User> userRepository,
            IRepository<ThreadViewStamp> threadViewStampRepository,
            IRepository<Subscription> subscriptionRepository,
            IRepository<Attachment> attachmentRepository,
            PollServices pollServices,
            FileServices fileServices,
            ParseServices parseServices,
            RoleServices roleServices,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _threadRepository = threadRepository;
            _threadViewRepository = threadViewRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _threadViewStampRepository = threadViewStampRepository;
            _subscriptionRepository = subscriptionRepository;
            _attachmentRepository = attachmentRepository;
            _pollServices = pollServices;
            _fileServices = fileServices;
            _parseServices = parseServices;
            _roleServices = roleServices;
        }

        public Thread GetThread(int threadID)
        {
            return _threadRepository.Get(threadID);
        }

        public int TotalThreads()
        {
            return _threadRepository.Get().Count();
        }

        public IEnumerable<Thread> GetGlobalAnnouncements()
        {
            return _threadRepository.Where(item => item.Type == (int)ThreadType.GlobalAnnouncement).ToList();
        }

        public void UpdateThread(int threadID, string title, ThreadType threadType, string image)
        {
            var thread = _threadRepository.Get(threadID);
            thread.Title = title;
            thread.Type = (int)threadType;
            thread.Image = image;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
        }

        public void ThreadViewed(int threadID, int userID)
        {
            ThreadView view = _threadViewRepository.First(item => item.UserID == userID && item.ThreadID == threadID);
            if (view == null)
            {
                _threadViewRepository.Add(new ThreadView()
                {
                    ThreadID = threadID,
                    UserID = userID,
                });
            }

            ThreadViewStamp viewStamp = _threadViewStampRepository.First(item => item.UserID == userID && item.ThreadID == threadID);

            if (viewStamp == null)
            {
                _threadViewStampRepository.Add(new ThreadViewStamp()
                {
                    Date = DateTime.UtcNow,
                    UserID = userID,
                    ThreadID = threadID
                });
            }
            _unitOfWork.Commit();
        }

        public Thread CreateThread(
            int forumID,
            int userID,
            string threadTitle,
            ThreadType threadType,
            string threadImage,
            string message,
            string pollText,
            string[] pollOptions,
            HttpPostedFileBase[] files)
        {
            Thread thread = new Thread
            {
                ForumID = forumID,
                Type = (int)threadType,
                Image = threadImage,
                Title = threadTitle,
                HasPoll = false
            };

            string parsedMessage = _parseServices.ParseBBCodeText(message);

            var firstPost = new Post()
            {
                UserID = userID,
                Date = DateTime.UtcNow,
                ThreadID = thread.ThreadID,
                TextOnly = _parseServices.GetTextOnly(message),
                Text = message,
                ParsedText = parsedMessage
            };

            thread.Posts.Add(firstPost);

            if (!string.IsNullOrWhiteSpace(pollText))
            {
                Poll poll = new Poll
                {
                    Question = pollText,
                    PollID = thread.ThreadID
                };

                thread.Poll = poll;

                var options = new System.Data.Objects.DataClasses.EntityCollection<PollOption>();
                foreach (string po in pollOptions)
                {
                    poll.PollOptions.Add(new PollOption
                    {
                        Text = po
                    });
                }
                thread.HasPoll = true;
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    string savedName = _fileServices.UploadFile(file);
                    firstPost.Attachments.Add(new Attachment()
                    {
                        Downloaded = 0,
                        SavedName = savedName,
                        DownloadName = file.FileName,
                        Size = file.ContentLength,
                        Type = file.ContentType
                    });
                }
            }
            _threadRepository.Add(thread);
            _unitOfWork.Commit();
            return thread;
        }

        public IEnumerable<Post> GetPosts(int threadID)
        {
            return _postRepository.Where(item => item.ThreadID.Equals(threadID)).ToList();
        }

        /// <summary>
        /// Toggles subscription to a thread. Returns true if the a subscription was made to the thread, otherwise returns false
        /// </summary>
        /// <param name="threadID">ThreaID of the thread to toggle subscription to</param>
        /// <param name="userID">UserID of the user to toggle subscription for</param>
        /// <returns>Returns true if the a subscription was made to the thread, otherwise returns false</returns>
        public bool ToggleThreadSubscription(int threadID, int userID)
        {
            if (IsSubscribed(threadID, userID))
            {
                Unsubscribe(threadID, userID);
                return false;
            }
            else
            {
                Subscribe(threadID, userID);
                return true;
            }
        }

        public void Subscribe(int threadID, int userID)
        {
            string authCode = Randoms.CleanGUID();

            _subscriptionRepository.Add(new Subscription
            {
                ThreadID = threadID,
                UserID = userID,
            });
            _unitOfWork.Commit();
        }

        public void Unsubscribe(int threadID, int userID)
        {
            Subscription subscription = _subscriptionRepository.First(item => item.ThreadID == threadID && item.UserID == userID);
            _subscriptionRepository.Delete(subscription);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Toggles lock on a thread. Returns true if the thread is toggled to locked, otherwise returns false
        /// </summary>
        /// <param name="threadID">ThreaID of the thread to toggle lock</param>
        /// <returns>Returns true if the thread is toggled to locked, otherwise returns false</returns>
        public bool ToggleThreadLock(int threadID)
        {
            Thread thread = GetThread(threadID);
            thread.IsLocked = !thread.IsLocked;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
            return thread.IsLocked;
        }

        public void Lock(int threadID)
        {
            Thread thread = _threadRepository.Get(threadID);
            thread.IsLocked = true;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
        }

        public void Unlock(int threadID)
        {
            Thread thread = _threadRepository.Get(threadID);
            thread.IsLocked = false;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
        }

        public IEnumerable<Thread> GetPagedThreads(int forumID, int pageNumber, int pageSize, OrderBy orderBy = OrderBy.Date, Direction direction = Direction.Descending)
        {
            IEnumerable<Thread> forumThreads = _threadRepository.Where(item => item.ForumID.Equals(forumID));

            forumThreads = forumThreads.OrderByDescending(item => item.Type).ThenByDescending(x => x.LastPost.Date);

            if (direction == Direction.Ascending)
                forumThreads.Reverse();

            forumThreads = forumThreads.TakePage(pageNumber, pageSize);

            return forumThreads.ToList();
        }

        public bool HasAttachment(int threadID)
        {
            var attachment = _attachmentRepository.First(item => item.Post.ThreadID == threadID);
            return attachment != null;
        }

        public bool HasNewPost(int threadID, int userID)
        {
            Thread thread = GetThread(threadID);

            Post lastPost = thread.LastPost;

            User user = _userRepository.Get(userID);

            if (lastPost.UserID == userID)
                return false;

            ThreadViewStamp FoundView = _threadViewStampRepository.First(item => item.UserID == userID && item.ThreadID == threadID);

            if (FoundView != null)
                return DateTime.Compare(lastPost.Date, FoundView.Date) > 0;
            else
                return DateTime.Compare(lastPost.Date, user.LastLogoutDate) > 0;
        }

        public string[] GetThreadImages()
        {
            string[] validExtensions = new string[] { ".gif", ".png", ".jpg", ".jpeg" };
            DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~/Images/ThreadImages"));
            return di.GetFiles().Select(x => x.Name).Where(x => validExtensions.Contains(Path.GetExtension(x))).OrderBy(x => x).ToArray();
        }

        public void DeleteThread(int threadID)
        {
            Thread thread = _threadRepository.Get(threadID);
            var attachments = _attachmentRepository.Where(item => item.Post.ThreadID == threadID);
            _fileServices.DeleteAttachments(attachments);
            var posts = thread.Posts.ToList();
            _threadRepository.Delete(threadID);
            _unitOfWork.Commit();
        }

        public bool CanLock(User user)
        {
            if (user == null)
                return false;
            return _roleServices.UserHasSpecialPermissions(user.UserID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator);
        }

        public bool HasVoted(int threadID, int userID)
        {
            if (userID == 0)
                return true;

            // threadID is used because polls have a one-to-one mapping with threads on 'ThreadID' column
            return this._pollServices.HasVoted(threadID, userID);
        }

        public bool IsSubscribed(int threadID, int userID)
        {
            if (userID == 0)
                return false;
            IEnumerable<Subscription> subscriptions = _subscriptionRepository.Where(item => item.ThreadID.Equals(threadID));
            return subscriptions.FirstOrDefault(x => x.ThreadID == threadID && x.UserID == userID) != null;
        }
    }
}