using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private UserServices _userServices;
        private MessageServices _messageServices;
        private ParseServices _parseServices;
        private EmailServices _emailServices;
        private User _currentUser;

        public MessagesController(
            UserServices userServices,
            MessageServices messageServicess,
            IRepository<Message> messageRepository,
            ParseServices parseServices,
            EmailServices emailServices,
            User currentUser)
        {
            _userServices = userServices;
            _messageServices = messageServicess;
            _parseServices = parseServices;
            _emailServices = emailServices;
            _currentUser = currentUser;
            SetCrumbs("User CP", "User CP");
        }

        [HttpGet]
        public ActionResult SendMessage(string ToUsername)
        {
            SetBreadCrumb("Send Message");
            var model = new SendMessageViewModel() { Username = ToUsername };
            return View(model);
        }

        [HttpPost]
        public ActionResult SendMessage(SendMessageViewModel SendMessageViewModel, int? messageID)
        {
            var user = _userServices.GetUser(SendMessageViewModel.Username);

            if (user == null)
                ModelState.AddModelError("Username", "User doesn't exist.");
            else if (user.UserID == _currentUser.UserID)
                ModelState.AddModelError("Username", "You can't send a message to yourself!");

            if (IsModelValidAndPersistErrors())
            {
                var message = _messageServices.SendMessage(_currentUser.UserID, user.UserID, SendMessageViewModel.Subject, SendMessageViewModel.Message);
                SetSuccess("Message sent to <b>" + user.Username + "</b>");
                string messageURL = Url.Action("ViewMessage", "Messages", new { MessageID = message.MessageID });
                _emailServices.NewMessage(message, user, messageURL);
                return RedirectToAction("ViewMessage", new { messageID = message.MessageID });
            }

            if (messageID.HasValue)
                return RedirectToAction("ViewMessage", new { messageID = messageID.Value });
            else
                return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult ViewMessage(int messageID)
        {
            if (!_messageServices.CanViewMessage(_currentUser.UserID, messageID))
                return RedirectToAction("Inbox");

            SetBreadCrumb("View Message");
            Message message = _messageServices.GetMessage(messageID);

            if (message.ToUserID == _currentUser.UserID && !message.IsRead)
            {
                _messageServices.MarkAsRead(messageID);
            }

            var model = new ViewMessageViewModel()
            {
                Message = message,
                ParsedText = _parseServices.ParseBBCodeText(message.Text),
                SendMessageViewModel = new SendMessageViewModel() { Username = message.FromUser.Username, Subject = "RE:" + message.Subject }
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Inbox(string Box, int page = 1, int pageSize = 25)
        {
            IEnumerable<Message> messages;
            string view;
            if (Box == "Sent")
            {
                SetBreadCrumb("Sentbox");
                messages = _messageServices.GetSentMessages(_currentUser.UserID).OrderByDescending(x => x.DateSent);
                view = "Sentbox";
            }
            else
            {
                SetBreadCrumb("Inbox");
                messages = _messageServices.GetReceivedMessages(_currentUser.UserID).OrderBy(item => item.IsRead).ThenByDescending(x => x.DateSent);
                view = "Inbox";
            }

            Pagination pagination = new Pagination(page, messages.Count(), pageSize, new { action = "Inbox", controller = "Messages", Box = Box });
            messages = messages.TakePage(page, pageSize);
            ViewData["Pagination"] = pagination;
            return View(view, messages);
        }

        [HttpPost]
        public ActionResult Inbox(int[] msgID, string Box, string DoAction)
        {
            SetBreadCrumb("Inbox");

            if (msgID == null)
            {
                SetNotice("You must select a message(s) to " + DoAction);
                return RedirectToAction("Inbox", new { Box = Box });
            }
            else if (DoAction != "Delete" && DoAction != "Mark As Read")
            {
                SetNotice("You must specify an action to take");
                return RedirectToAction("Inbox", new { Box = Box });
            }

            if (DoAction == "Delete")
            {
                foreach (int mid in msgID)
                {
                    if (_messageServices.CanViewMessage(_currentUser.UserID, mid))
                        _messageServices.DeleteMessage(mid);
                }

                SetSuccess("Message(s) deleted");
            }
            else if (DoAction == "Mark As Read")
            {
                foreach (int mid in msgID)
                {
                    if (_messageServices.CanViewMessage(_currentUser.UserID, mid))
                        _messageServices.MarkAsRead(mid);
                }

                SetSuccess("Message(s) marked as read");
            }

            return RedirectToAction("Inbox", new { Box = Box });
        }
    }
}