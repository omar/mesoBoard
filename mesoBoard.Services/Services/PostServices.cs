using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mesoBoard.Common;
using mesoBoard.Data;
using Microsoft.AspNetCore.Http;

namespace mesoBoard.Services
{
    public class PostServices : BaseService
    {
        private IRepository<Attachment> _attachmentRepository;
        private IRepository<Post> _postRepository;
        private IRepository<ReportedPost> _reportedPostRepository;

        private FileServices _fileServices;
        private ParseServices _parseServices;
        private RoleServices _roleServices;

        public PostServices(
            IRepository<Attachment> attachmentRepository,
            IRepository<Post> postRepository,
            IRepository<ReportedPost> reportedPostRepository,
            FileServices fileServices,
            ParseServices parseServices,
            RoleServices roleServices,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _postRepository = postRepository;
            _attachmentRepository = attachmentRepository;
            _reportedPostRepository = reportedPostRepository;
            _fileServices = fileServices;
            _roleServices = roleServices;
            _parseServices = parseServices;
        }

        public async Task<Post> CreatePostAsync(int threadID, int userID, string message, bool useSignature, IFormFile[] files)
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
            if (files != null)
                await _fileServices.CreateAttachmentsAsync(files, post.PostID);
            _unitOfWork.Commit();
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

        public async Task UpdatePostAsync(int postID, string text, IFormFile[] files)
        {
            Post post = _postRepository.Get(postID);
            post.Text = text;
            post.ParsedText = _parseServices.ParseBBCodeText(text);
            post.TextOnly = _parseServices.GetTextOnly(text);
            _postRepository.Update(post);
            _unitOfWork.Commit();
            await _fileServices.CreateAttachmentsAsync(files, post.PostID);
        }

        public IEnumerable<Post> GetPagedPosts(int threadID, int page, int pageSize)
        {
            IEnumerable<Post> posts = _postRepository.Where(item => item.ThreadID == threadID);
            posts = posts.OrderBy(x => x.Date);
            posts = posts.TakePage(page, pageSize);
            return posts.ToList();
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
            _unitOfWork.Commit();
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
            _unitOfWork.Commit();
            return reportedPost;
        }
    }
}