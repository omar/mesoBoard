using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using System;

namespace mesoBoard.Services
{
    public class ForumServices 
    {
        IRepository<Thread> _threadRepository;
        IRepository<Category> _categoryRepository;
        IRepository<Post> _postRepository;
        IRepository<Forum> _forumRepository;
        ThreadServices _threadServices;
        IRepository<ThreadViewStamp> _threadViewStampRepository;
        PermissionServices _permissionServices;
        IRepository<User> _userRepository;

        public ForumServices(
            IRepository<Thread> threads,
            IRepository<Category> categories,
            IRepository<Post> posts,
            IRepository<Forum> forums,
            ThreadServices threadServices,
            IRepository<ThreadViewStamp> threadViewStampRepository,
            PermissionServices permissionServices,
            IRepository<User> userRepository)
        {
            _threadRepository = threads;
            _categoryRepository = categories;
            _postRepository = posts;
            _forumRepository = forums;
            _threadServices = threadServices;
            _threadViewStampRepository = threadViewStampRepository;
            _permissionServices = permissionServices;
            _userRepository = userRepository;
        }

        public Forum CreateForum(int categoryID, string name, string description, bool visibleToGuests, bool allowGuestDownloads)
        {
            int order = GetLastOrder();
            Forum forum = new Forum()
            {
                CategoryID = categoryID,
                Description = description,
                Name = name,
                Order = order + 1,
                VisibleToGuests = visibleToGuests,
                AllowGuestDownloads = allowGuestDownloads
            };
            _forumRepository.Add(forum);
            return forum;
        }

        public int GetLastOrder()
        {
            Forum lastForum = _forumRepository.Get().OrderByDescending(item => item.Order).FirstOrDefault();
            if (lastForum == null)
                return 0;
            else
                return lastForum.Order;
        }

        public Post GetLastPost(int forumID)
        {
            var query = from post in _postRepository.Get()
                        join thread in _threadRepository.Get() on post.ThreadID equals thread.ThreadID
                        where thread.ForumID == forumID
                        orderby post.Date descending
                        select post;

            return query.FirstOrDefault();
        }

        public void ChangeForumOrder(int forumID, int categoryID, int direction)
        {
            List<Forum> forums = GetForumsByCategory(categoryID).OrderBy(x => x.Order).ToList();
            Forum forum = forums.FirstOrDefault(x => x.ForumID == forumID);
            int forumIndex = forums.IndexOf(forum);
            forum.Order += direction;

            Forum displaced = forums.ElementAt(forumIndex + direction);
            displaced.Order -= direction;

            _forumRepository.Update(forum);
            _forumRepository.Update(displaced);
        }

        public void ChangeCategoryOrder(int categoryID, int direction)
        {
            List<Category> categories = _categoryRepository.Get().OrderBy(x => x.Order).ToList();
            Category category = categories.FirstOrDefault(x => x.CategoryID == categoryID);
            int categoryIndex = categories.IndexOf(category);
            category.Order += direction;

            Category displaced = null;

            switch (direction)
            {
                case 1:
                    displaced = categories.ElementAt(categoryIndex + 1);
                    displaced.Order--;
                    break;
                case -1:
                    displaced = categories.ElementAt(categoryIndex - 1);
                    displaced.Order++;
                    break;
            }

            _categoryRepository.Update(category);
            _categoryRepository.Update(displaced);
        }

        public Forum GetForum(int forumID)
        {
            return _forumRepository.Get(forumID);
        }

        public IEnumerable<Forum> GetForumsByCategory(int categoryID)
        {
            return _forumRepository.Get().Where(x => x.CategoryID == categoryID);
        }

        public IEnumerable<Forum> GetViewableForums(int userID)
        {
            IEnumerable<Forum> forums = _forumRepository.Get().Where(x => _permissionServices.CanView(x.ForumID, userID));
            return forums;
        }

        public IEnumerable<Category> GetViewableCategories(int userID)
        {
            IEnumerable<Forum> viewableForums = GetViewableForums(userID);

            return viewableForums.Select(x => x.Category).Distinct();
        }

        public bool HasNewPost(int forumID, int userID)
        {
            if (userID == 0)
                return false;
            User user = _userRepository.Get(userID);

            return _postRepository
                .Where(item => item.Thread.ForumID == forumID)
                .Any(item =>
                    item.Date > user.LastLogoutDate &&
                    !item.Thread.ThreadViewStamps.Any(x => x.Date > user.LastLogoutDate) &&
                    item.UserID != user.UserID);
        }
    }
}
