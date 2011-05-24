using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;
using System;

namespace mesoBoard.Services
{
    public class MessageServices 
    {
        IRepository<Message> _messageRepository;

        public MessageServices(IRepository<Message> messageRepository)
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
            return message;
        }

        public IEnumerable<Message> GetUnreadMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.IsRead == false && item.ToDelete == false);
        }

        public IEnumerable<Message> GetSentMessages(int userID)
        {
            return _messageRepository.Where(item => item.FromUserID == userID && item.FromDelete == false);
        }

        public IEnumerable<Message> GetReceivedMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.ToDelete == false);
        }

        public IEnumerable<Message> GetReadMessages(int userID)
        {
            return _messageRepository.Where(item => item.ToUserID == userID && item.IsRead == true && item.ToDelete == false);
        }

        public void MarkAsRead(int messageID)
        {
            var message = _messageRepository.Get(messageID);
            message.IsRead = true;
            _messageRepository.Update(message);
        }

        public void DeleteMessage(int messageID)
        {
            _messageRepository.Delete(messageID);
        }
    }
}