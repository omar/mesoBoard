using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;
using System;
using System.Linq;

namespace mesoBoard.Services
{
    public class MessageServices : BaseService 
    {
        IRepository<Message> _messageRepository;

        public MessageServices(
            IRepository<Message> messageRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _messageRepository = messageRepository;
        }

        public bool CanViewMessage(int userID, int messageID)
        {
            var message = _messageRepository.First(item => item.MessageID == messageID && (item.FromUserID == userID || item.ToUserID == userID));
            return message != null;
        }

        public Message GetMessage(int messageID)
        {
            return _messageRepository.Get(messageID);
        }

        public Message SendMessage(int fromUserID, int toUserID, string subject, string text)
        {
            var message = new Message()
            {
                ToUserID = toUserID,
                FromUserID = fromUserID,
                DateSent = DateTime.UtcNow,
                Subject = subject ?? "<No Subject>",
                Text = text,
                IsRead = false,
                ToDelete = false,
                FromDelete = false
            };

            _messageRepository.Add(message);
            _unitOfWork.Commit();
            return message;
        }

        public IEnumerable<Message> GetUnreadMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.IsRead == false && item.ToDelete == false).ToList();
        }

        public IEnumerable<Message> GetSentMessages(int userID)
        {
            return _messageRepository.Where(item => item.FromUserID == userID && item.FromDelete == false).ToList();
        }

        public IEnumerable<Message> GetReceivedMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.ToDelete == false).ToList();
        }

        public IEnumerable<Message> GetReadMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.IsRead == true && item.ToDelete == false).ToList();
        }

        public void MarkAsRead(int messageID)
        {
            var message = _messageRepository.Get(messageID);
            message.IsRead = true;
            _messageRepository.Update(message);
            _unitOfWork.Commit();
        }

        public void DeleteMessage(int messageID)
        {
            _messageRepository.Delete(messageID);
            _unitOfWork.Commit();
        }
    }
}