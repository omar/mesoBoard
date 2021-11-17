using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework;
using mesoBoard.Framework.Core;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private ForumServices _forumServices;
        private IRepository<Smiley> _smileyRepository;
        private PostServices _postServices;
        private ThreadServices _threadServices;
        private FileServices _fileServices;
        private PollServices _pollServices;
        private EmailServices _emailServices;
        private PermissionServices _permissionServices;
        private ParseServices _parseServices;
        private User _currentUser;

        public PostController(
            IRepository<Smiley> smileyRepository,
            ForumServices forumServices,
            PostServices postServices,
            ThreadServices threadServices,
            FileServices fileServices,
            PollServices pollServices,
            EmailServices emailServices,
            PermissionServices permissionServices,
            ParseServices parseServices,
            User currentUser)
        {
            _smileyRepository = smileyRepository;
            _forumServices = forumServices;
            _postServices = postServices;
            _threadServices = threadServices;
            _fileServices = fileServices;
            _pollServices = pollServices;
            _emailServices = emailServices;
            _permissionServices = permissionServices;
            _parseServices = parseServices;
            _currentUser = currentUser;
        }

        public ActionResult GetSmilies(int x, int z)
        {
            ViewData["x"] = x;
            ViewData["z"] = z;
            IEnumerable<Smiley> smilies = _smileyRepository.Get().ToList();
            if (smilies.Count() < z)
            {
                ViewData["z"] = smilies.Count();
                return View("_SmiliesTable", smilies);
            }
            else
            {
                return View("_SmiliesTable", smilies.Take(z));
            }
        }

        [HttpGet]
        public ActionResult CreateThread(int ForumID)
        {
            if (!_permissionServices.CanCreateThread(ForumID, _currentUser.UserID))
            {
                SetNotice("You don't have permission to create a thread in this forum");
                return RedirectToAction("ViewForum", "Board", new { ForumID = ForumID });
            }

            SetBreadCrumb("Create Thread");
            var model = GenerateThreadViewModel(ForumID, EditorType.Create);
            return View("Thread", model);
        }

        [HttpGet]
        public ActionResult EditThread(int ThreadID)
        {
            Thread thread = _threadServices.GetThread(ThreadID);
            Post post = thread.FirstPost;

            if (!_postServices.CanEditPost(post.PostID, _currentUser.UserID))
            {
                SetError("You can't edit this post");
                return RedirectToAction("ViewThread", "Board", new { ThreadID = thread.ThreadID });
            }

            SetBreadCrumb("Edit Thread");

            var model = GenerateThreadViewModel(thread.ForumID, EditorType.Edit, post.PostID);
            return View("Thread", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThreadValidateAsync(ThreadViewModel model)
        {
            EditorType editorType;
            if (model.PostEditor.PostID == 0)
                editorType = EditorType.Create;
            else
                editorType = EditorType.Edit;

            if (editorType == EditorType.Create)
            {
                if (!_permissionServices.CanCreateThread(model.ForumID, _currentUser.UserID))
                {
                    SetNotice("You don't have permission to create a thread in this forum");
                    return RedirectToAction("ViewForum", "Board", new { ForumID = model.ForumID });
                }
            }
            else
            {
                if (!_postServices.CanEditPost(model.PostEditor.PostID, _currentUser.UserID))
                {
                    SetError("You can't edit this post");
                    return RedirectToAction("ViewThread", "Board", new { ThreadID = model.ThreadEditor.ThreadID });
                }
            }

            if (!_permissionServices.CanCreateThreadType(model.ForumID, _currentUser.UserID, (ThreadType)model.ThreadEditor.ThreadType))
            {
                model.ThreadEditor.ThreadType = (int)ThreadType.Regular;
                ModelState.AddModelError("ThreadEditor.ThreadType", "You don't have permission to create a thread of this type");
            }

            if (!model.Preview.HasValue && _currentUser.LastPostDate.HasValue)
            {
                int MinTimeLimit = int.Parse(SiteConfig.TimeBetweenPosts.Value);
                double ValToCompare = (DateTime.UtcNow - _currentUser.LastPostDate.Value).TotalSeconds;
                if (ValToCompare < MinTimeLimit)
                    ModelState.AddModelError("TimeBetweenPosts", "You may only create new posts every " + MinTimeLimit + " seconds (" + Math.Round((MinTimeLimit - ValToCompare)) + " seconds remaining)");
            }

            ValidateThreadImage(model.ThreadEditor.Image);
            IFormFile[] files = null;

            if (_permissionServices.CanCreateAttachment(model.ForumID, _currentUser.UserID))
            {
                if (model.PostEditor.Files != null)
                {
                    ValidatePostedFiles(model.PostEditor.Files);
                    files = model.PostEditor.Files.Where(item => item != null && item.Length > 0 && !string.IsNullOrWhiteSpace(item.FileName)).ToArray();
                }
            }
            else
                model.PostEditor.Delete = null;

            if (!_permissionServices.CanCreatePoll(model.ForumID, _currentUser.UserID))
            {
                model.PollEditor.Text = string.Empty;
                model.PollEditor.Options = string.Empty;
            }

            if (IsModelValidAndPersistErrors() && !model.Preview.HasValue)
            {
                if (editorType == EditorType.Create)
                {
                    Thread thread = await _threadServices.CreateThreadAsync(
                        model.ForumID,
                        _currentUser.UserID,
                        model.ThreadEditor.Title,
                        (ThreadType)model.ThreadEditor.ThreadType,
                        model.ThreadEditor.Image,
                        model.PostEditor.Message,
                        model.PollEditor.Text,
                        model.PollEditor.OptionsSplit,
                        files);

                    if (_threadServices.IsSubscribed(thread.ThreadID, _currentUser.UserID) && model.PostEditor.SubscribeToThread)
                        _threadServices.Subscribe(thread.ThreadID, _currentUser.UserID);

                    SetSuccess("Thread created");
                    return RedirectToAction("ViewThread", "Board", new { ThreadID = thread.ThreadID });
                }
                else
                {
                    Thread thread = _threadServices.GetThread(model.ThreadEditor.ThreadID);
                    var poll = model.PollEditor;
                    bool createPoll = true;

                    if (thread.Poll != null)
                    {
                        if (thread.Poll.TotalVotes > 0)
                        {
                            if (poll != null && poll.Delete)
                            {
                                _pollServices.DeletePoll(model.ThreadEditor.ThreadID);
                            }
                            else
                                createPoll = false;
                        }
                        else
                        {
                            _pollServices.DeletePoll(model.ThreadEditor.ThreadID);
                        }
                    }

                    if (createPoll && poll != null && !string.IsNullOrWhiteSpace(poll.Text))
                        _pollServices.CreatePoll(poll.Text, poll.OptionsSplit, model.ThreadEditor.ThreadID);

                    if (model.PostEditor.Delete != null)
                        _fileServices.DeleteAttachments(model.PostEditor.Delete);

                    _threadServices.UpdateThread(model.ThreadEditor.ThreadID, model.ThreadEditor.Title, (ThreadType)model.ThreadEditor.ThreadType, model.ThreadEditor.Image);
                    await _postServices.UpdatePostAsync(model.PostEditor.PostID, model.PostEditor.Message, files);

                    SetSuccess("Thread edited");
                    return RedirectToAction("ViewThread", "Board", new { ThreadID = model.ThreadEditor.ThreadID });
                }
            }

            if (model.Preview.HasValue)
            {
                TempData["Preview_Text"] = model.PostEditor.Message;
                TempData["Preview_Title"] = model.ThreadEditor.Title;
            }

            if (editorType == EditorType.Create)
                return RedirectToAction("CreateThread", new { ForumID = model.ForumID });
            else
                return RedirectToAction("EditThread", new { ThreadID = model.ThreadEditor.ThreadID });
        }

        [HttpGet]
        public ActionResult CreatePost(int ThreadID, int? QuotePostID)
        {
            Thread thread = _threadServices.GetThread(ThreadID);
            Forum forum = thread.Forum;

            if (!_permissionServices.CanReply(forum.ForumID, _currentUser.UserID))
            {
                if (thread.IsLocked)
                {
                    SetNotice("You can't reply to this thread because it is locked");
                    return RedirectToAction("ViewThread", "Board", new { ThreadID = ThreadID });
                }
                else
                {
                    SetNotice("You don't have permission to reply in this forum");
                    return RedirectToAction("ViewForum", "Board", new { ForumID = forum.ForumID });
                }
            }

            SetBreadCrumb("Post Reply");

            var model = GeneratePostViewModel(thread.ForumID, thread.ThreadID, EditorType.Create, QuotePostID);

            return View("Post", model);
        }

        [HttpGet]
        public ActionResult EditPost(int PostID)
        {
            Post post = _postServices.GetPost(PostID);

            if (post.Thread.FirstPost.PostID == PostID)
                return RedirectToAction("EditThread", new { ThreadID = post.ThreadID });

            if (!_postServices.CanEditPost(PostID, _currentUser.UserID))
            {
                SetError("You can't edit this post");
                return RedirectToAction("ViewThread", "Board", new { ThreadID = post.ThreadID });
            }

            var model = GeneratePostViewModel(post.Thread.ForumID, post.ThreadID, EditorType.Edit, postID: PostID);

            SetBreadCrumb("Edit Post");

            return View("Post", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostValidateAsync(PostViewModel model)
        {
            EditorType editorType;
            if (model.PostEditor.PostID == 0)
                editorType = EditorType.Create;
            else
                editorType = EditorType.Edit;

            var thread = _threadServices.GetThread(model.ThreadID);
            var forum = thread.Forum;

            if (editorType == EditorType.Create)
            {
                if (!_permissionServices.CanReply(forum.ForumID, _currentUser.UserID))
                {
                    if (thread.IsLocked)
                    {
                        SetNotice("You can't reply to this thread because it is locked");
                        return RedirectToAction("ViewThread", "Board", new { ThreadID = thread.ThreadID });
                    }
                    else
                    {
                        SetNotice("You don't have permission to reply in this forum");
                        return RedirectToAction("ViewForum", "Board", new { ForumID = forum.ForumID });
                    }
                }
            }
            else
            {
                if (!_postServices.CanEditPost(model.PostEditor.PostID, _currentUser.UserID))
                {
                    SetError("You can't edit this post");
                    return RedirectToAction("ViewThread", "Board", new { ThreadID = thread.ThreadID });
                }
            }

            IFormFile[] files = null;
            if (_permissionServices.CanCreateAttachment(forum.ForumID, _currentUser.UserID))
            {
                if (model.PostEditor.Files != null)
                {
                    ValidatePostedFiles(model.PostEditor.Files);
                    files = model.PostEditor.Files.Where(item => item != null && item.Length > 0 && !string.IsNullOrWhiteSpace(item.FileName)).ToArray();
                }
            }
            else
                model.PostEditor.Files = null;

            if (IsModelValidAndPersistErrors() && !model.Preview.HasValue)
            {
                if (editorType == EditorType.Create)
                {
                    if (_threadServices.IsSubscribed(thread.ThreadID, _currentUser.UserID) && model.PostEditor.SubscribeToThread)
                        _threadServices.Subscribe(thread.ThreadID, _currentUser.UserID);

                    var post = await _postServices.CreatePostAsync(model.ThreadID, _currentUser.UserID, model.PostEditor.Message, model.PostEditor.ShowSignature, files);
                    string postUrl = Url.Action("ViewThread", "Board", new { ThreadID = post.ThreadID, PostID = post.PostID }) + "#" + post.PostID;
                    IEnumerable<Subscription> subscriptions = thread.Subscriptions;
                    _emailServices.NewPostEmail(subscriptions, post, post.Thread, postUrl);

                    SetSuccess("Your reply was posted");
                    return Redirect(Url.Action("ViewThread", "Board", new { ThreadID = model.ThreadID, LastPost = true, NewPost = true }) + "#" + post.PostID);
                }
                else
                {
                    if (model.PostEditor.Delete != null)
                        _fileServices.DeleteAttachments(model.PostEditor.Delete);

                    await _postServices.UpdatePostAsync(model.PostEditor.PostID, model.PostEditor.Message, files);
                    SetSuccess("Post edited");
                    return Redirect(Url.Action("ViewThread", "Board", new { ThreadID = thread.ThreadID, LastPost = true, PostID = model.PostEditor.PostID }) + "#" + model.PostEditor.PostID);
                }
            }

            if (model.Preview.HasValue)
            {
                TempData["Preview_Text"] = model.PostEditor.Message;
            }

            if (editorType == EditorType.Create)
                return RedirectToAction("CreatePost", new { ThreadID = model.ThreadID });
            else
                return RedirectToAction("EditPost", new { PostID = model.PostEditor.PostID });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CastVote(int ThreadID, int? PollOptionID)
        {
            Thread thread = _threadServices.GetThread(ThreadID);

            if (!_permissionServices.CanView(thread.ForumID, _currentUser.UserID))
            {
                SetError("You don't have permission to view this thread");
                return RedirectToAction("Board", "Index");
            }

            bool canCastVote = _permissionServices.CanCastVote(thread.ForumID, _currentUser.UserID);

            if (canCastVote)
            {
                var poll = thread.Poll;

                if (PollOptionID.HasValue)
                {
                    _pollServices.CastVote(_currentUser.UserID, PollOptionID.Value);

                    SetSuccess("Your vote has been cast");
                }
                else
                    SetError("Select a vote option to cast a vote");
            }
            else
                SetError("You don't have permission to cast a vote");

            return RedirectToAction("ViewThread", "Board", new { ThreadID = ThreadID });
        }

        public ActionResult ThreadReview(int ThreadID)
        {
            Thread thread = _threadServices.GetThread(ThreadID);
            return View("_ThreadReview", thread);
        }

        [PermissionAuthorize(SpecialPermissionValue.Administrator, SpecialPermissionValue.Moderator)]
        public ActionResult DeletePost(int PostID)
        {
            Post post = _postServices.GetPost(PostID);
            int threadID = post.ThreadID;
            int forumID = post.Thread.ForumID;
            if (post.Thread.FirstPost.PostID == PostID)
            {
                _threadServices.DeleteThread(post.ThreadID);
                SetSuccess("Thread deleted");
                return RedirectToAction("ViewForum", "Board", new { ForumID = forumID });
            }
            else
            {
                _postServices.DeletePost(PostID);
                SetSuccess("Post deleted");
                return RedirectToAction("ViewThread", "Board", new { ThreadID = threadID });
            }
        }

        public ActionResult ReportPost(int PostID)
        {
            Post post = _postServices.GetPost(PostID);

            if (!post.IsReported)
                _postServices.ReportPost(PostID);

            SetSuccess("Post reported");
            return RedirectToAction("ViewThread", "Board", new { ThreadID = post.ThreadID });
        }

        [NonAction]
        private PostEditorViewModel GeneratePostEditorViewModel(int threadID, EditorType editorType, Post post = null)
        {
            if (editorType == EditorType.Edit && post == null)
                throw new InvalidOperationException("postID is required to generate an edit PostViewModel");

            var model = new PostEditorViewModel();

            if (editorType == EditorType.Edit)
            {
                model.Message = post.Text;
                model.ShowSignature = post.UseSignature;
                model.PostID = post.PostID;
                model.SubscribeToThread = _threadServices.IsSubscribed(threadID, _currentUser.UserID);
                model.Attachments = _fileServices.GetPostAttachments(post.PostID);
            }

            return model;
        }

        [NonAction]
        private ThreadViewModel GenerateThreadViewModel(int forumID, EditorType editorType, int? postID = null)
        {
            if (editorType == EditorType.Edit && !postID.HasValue)
                throw new InvalidOperationException("postID is required to generate an edit ThreadViewModel");

            Forum forum = _forumServices.GetForum(forumID);

            var permittedThreadTypes = _permissionServices.GetAllowedThreadTypes(forumID, _currentUser.UserID).ToList();
            var model = new ThreadViewModel()
            {
                EditorType = editorType,
                Forum = forum,
                ForumID = forum.ForumID,
                ThreadEditor = new ThreadEditorViewModel()
                {
                    AllowThreadImages = SiteConfig.AllowThreadImage.ToBool(),
                    PermittedThreadTypes = permittedThreadTypes,
                    ThreadImages = _threadServices.GetThreadImages(),
                },
                CanUploadAttachments = _permissionServices.CanCreateAttachment(forumID, _currentUser.UserID),
                CanCreatePoll = _permissionServices.CanCreatePoll(forumID, _currentUser.UserID)
            };

            if (editorType == EditorType.Edit)
            {
                var post = _postServices.GetPost(postID.Value);
                var thread = post.Thread;
                model.ThreadEditor.ThreadType = thread.Type;
                model.ThreadEditor.Image = thread.Image;
                model.ThreadEditor.ThreadID = post.ThreadID;
                model.ThreadEditor.Title = thread.Title;
                model.PostEditor = GeneratePostEditorViewModel(thread.ThreadID, EditorType.Edit, post);

                if (post.Thread.Poll != null)
                {
                    Poll poll = post.Thread.Poll;
                    IEnumerable<PollOption> options = poll.PollOptions;
                    bool hasVotes = options.Any(item => item.PollVotes.Count > 0);
                    model.PollEditor = new PollEditorViewModel()
                    {
                        HasVotes = hasVotes,
                        Options = poll.PollOptionsAsString(),
                        Text = poll.Question,
                        PollID = poll.PollID
                    };
                }
            }

            if (TempData.ContainsKey("Preview_Text"))
            {
                string previewText = (string)TempData["Preview_Text"];
                model.PreviewText = _parseServices.ParseBBCodeText(previewText);
                model.Preview = true;
                model.PreviewTitle = (string)TempData["Preview_Title"];
            }

            return model;
        }

        [NonAction]
        private PostViewModel GeneratePostViewModel(int forumID, int threadID, EditorType editorType, int? quotePostID = null, int? postID = null)
        {
            var postEditor = GeneratePostEditorViewModel(threadID, EditorType.Create);

            if (editorType == EditorType.Create)
            {
                if (quotePostID.HasValue)
                {
                    Post quotePost = _postServices.GetPost(quotePostID.Value);
                    string quoteText = string.Format("[quote={0}]{1}[/quote]{2}", quotePost.User.Username, quotePost.Text, Environment.NewLine);
                    postEditor.Message = quoteText + postEditor.Message;
                }
            }
            else
            {
                Post post = _postServices.GetPost(postID.Value);
                postEditor.Message = post.Text;
                postEditor.PostID = post.PostID;
            }

            var model = new PostViewModel()
            {
                EditorType = editorType,
                PostEditor = postEditor,
                ThreadID = threadID,
                CanUploadAttachments = _permissionServices.CanCreateAttachment(forumID, _currentUser.UserID),
                Thread = _threadServices.GetThread(threadID)
            };

            if (TempData.ContainsKey("Preview_Text"))
            {
                string text = (string)TempData["Preview_Text"];
                model.PreviewText = _parseServices.ParseBBCodeText(text);
                model.Preview = true;
            }

            return model;
        }

        [NonAction]
        private void ValidatePostedFiles(IFormFile[] files)
        {
            if (files != null && files.Count() > 0)
            {
                int maxFileSize = SiteConfig.MaxFileSize.ToInt();
                List<string> invalidFileTypes = new List<string>();
                bool uploadLimitExceeded = false;

                foreach (var file in files.Where(item => item != null))
                {
                    if (file.Length > 0)
                    {
                        if (!_fileServices.ValidFileType(file.FileName))
                            invalidFileTypes.Add(System.IO.Path.GetExtension(file.FileName));
                        else if (file.Length > maxFileSize * 1024)
                            uploadLimitExceeded = true;
                    }
                }

                string error = "";

                if (uploadLimitExceeded)
                    error += "Max file size is " + maxFileSize + " kb.";
                if (invalidFileTypes.Count > 0)
                {
                    error += string.Format("File type not accepted ({0}).", string.Join(", ", invalidFileTypes));
                }

                if (!string.IsNullOrWhiteSpace(error))
                    ModelState.AddModelError("PostEditor.Files", error);
            }
        }

        [NonAction]
        private void ValidateThreadImage(string image)
        {
            if (!string.IsNullOrWhiteSpace(image))
            {
                var threadImages = _threadServices.GetThreadImages();

                if (!threadImages.Contains(image))
                    ModelState.AddModelError("ThreadEditor.Image", "Thread image not found.");
            }
        }
    }
}