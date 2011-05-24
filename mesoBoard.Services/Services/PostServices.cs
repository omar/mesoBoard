using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class PostServices
    {
        IRepository<Attachment> _attachmentRepository;
        IRepository<Post> _postRepository;
        IRepository<ReportedPost> _reportedPostRepository;

        FileServices _fileServices;
        ParseServices _parseServices;
        RoleServices _roleServices;

        public PostServices(
            IRepository<Attachment> attachmentRepository,
            IRepository<Post> postRepository,
            IRepository<ReportedPost> reportedPostRepository,

            FileServices fileServices,
            ParseServices parseServices,
            RoleServices roleServices)
        {
            _postRepository = postRepository;
            _attachmentRepository = attachmentRepository;
            _reportedPostRepository = reportedPostRepository;
            _fileServices = fileServices;
            _roleServices = roleServices;
            _parseServices = parseServices;
        }

        public Post CreatePost(int threadID, int userID, string message, bool useSignature, HttpPostedFileBase[] files)
        {
            string parsedText = _parseServices.ParseBBCodeText(message);
            Post post = new Post()
            {
                Date = DateTime.UtcNow,
                ParsedText = parsedText,
                Text = message,
                TextOnly = _parseServices.GetTextOnly(message),
                ThreadID = threadID,
                UserID = userID,
                UseSignature = useSignature
            };

            _postRepository.Add(post);
            if(files != null)
                _fileServices.CreateAttachments(files, post.PostID);

            return post;
        }

        public int TotalPosts()
        {
            return _postRepository.Get().Count();
        }

        public Post GetPost(int PostID)
        {
            return _postRepository.Get(PostID);
        }

        public void UpdatePost(int postID, string text, HttpPostedFileBase[] files)
        {
            Post post = _postRepository.Get(postID);
            post.Text = text;
            post.ParsedText = _parseServices.ParseBBCodeText(text);
            post.TextOnly = _parseServices.GetTextOnly(text);
            _postRepository.Update(post);

            _fileServices.CreateAttachments(files, post.PostID);
        }

        public IEnumerable<Post> GetPagedPosts(int threadID, int page, int pageSize)
        {
            IEnumerable<Post> posts = _postRepository.Where(item => item.ThreadID == threadID);
            posts = posts.OrderBy(x => x.Date);
            posts = posts.TakePage(page, pageSize);
            return posts;
        }


        public bool CanEditPost(int postID, int userID)
        {
            if (userID == 0)
                return false;

            if (_roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator))
                return true;

            Post post = GetPost(postID);
            Thread thread = post.Thread;

            if (thread.IsLocked)
                return false;
            else if (post.UserID == userID)
                return ((DateTime.UtcNow - post.Date).TotalSeconds < SiteConfig.TimeLimitToEditPost.IntValue());

            return false;
        }

        public void DeletePost(int postID)
        {
            var attachments = _attachmentRepository.Where(item => item.PostID == postID);
            _fileServices.DeleteAttachments(attachments);

            _postRepository.Delete(postID);
        }

        public bool CanDeletePost(int postID, int userID)
        {
            if (userID == 0)
                return false;

            return _roleServices.UserHasSpecialPermissions(userID, SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator);
        }

        public ReportedPost ReportPost(int postID)
        {
            ReportedPost reportedPost = new ReportedPost()
            {
                PostID = postID,
                Date = DateTime.UtcNow
            };

            _reportedPostRepository.Add(reportedPost);
            return reportedPost;
        }
    }
}