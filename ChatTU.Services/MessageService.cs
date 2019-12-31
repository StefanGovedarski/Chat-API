using ChatTU.Data;
using ChatTU.Data.Infrastructure;
using ChatTU.Data.Models;
using ChatTU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChatTU.Services
{
    public class MessageService : IMessageService
    {
        private IUnitOfWork _unitOfWork;
        private IUserService _userService;

        public MessageService(IUserService userService)
        {
            _unitOfWork = new UnitOfWork(new ChatTuContext());
            _userService = userService;

        }

        public List<ConvesationEntity> GetConversationHistory(string username)
        {
            var user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username);
            var results = new List<ConvesationEntity>();
            if (user != null)
            {
                results = _unitOfWork.Conversations.GetAll().Where(x => (user.Id == x.FromUserId || user.Id == x.ToUserId) && x.IsDeleted == false).OrderByDescending(x => x.LastUpdated).ToList();
                _userService.MarkLoggedInStatusAs(username, true);
            }

            return results;
        }

        public List<MessageEntity> GetMessages(int conversationId, int count)
        {
            return _unitOfWork.Messages.GetAll().Where(x => x.Conversation.Id == conversationId).OrderByDescending(x => x.Time).Take(count).OrderBy(x => x.Time).ToList();
        }

        public ConvesationEntity GetOrCreateConversation(string currentUser, string targetUser)
        {
            ConvesationEntity conversation;

            var users = _unitOfWork.Users.GetAll().Where(x => x.Username == currentUser || x.Username == targetUser);
            var currUserId = users.First(x => x.Username == currentUser).Id;
            var targetUserId = users.First(x => x.Username == targetUser).Id;

            conversation = _unitOfWork.Conversations.GetAll().FirstOrDefault(x => (x.FromUser.Id == currUserId && x.ToUser.Id == targetUserId) ||
                                                                                  (x.FromUser.Id == targetUserId && x.ToUser.Id == currUserId));

            if (conversation == null)
            {
                conversation = new ConvesationEntity()
                {
                    CreatedDate = DateTime.Now,
                    FromUser = users.First(x => x.Username == currentUser),
                    ToUser = users.First(x => x.Username == targetUser),
                    LastUpdated = DateTime.Now
                };
                _unitOfWork.Conversations.Add(conversation);
                _unitOfWork.Save();
            }

            return conversation;
        }

        public void AddMessage(int conversationId, string sendBy, string content, DateTime time)
        {
            var convEntity = _unitOfWork.Conversations.GetAll().FirstOrDefault(x => x.Id == conversationId);

            MessageEntity messageEntity = new MessageEntity
            {
                SendBy = sendBy,
                Conversation = convEntity,
                Time = DateTime.UtcNow,
                Content = content,
                Status = Data.Enums.MessageStatus.DELIVERED
            };

            convEntity.LastUpdated = DateTime.Now;
            _unitOfWork.Messages.Add(messageEntity);
            _unitOfWork.Save();
        }

        public bool IsFullHistory(int conversationId, int messageCount)
        {
            var total = _unitOfWork.Messages.GetAll().Where(x => x.Conversation.Id == conversationId).Count();

            return messageCount > total;
        }

        public MessageEntity SaveFile(HttpPostedFile postedFile, string currentUsername, int conversationId)
        {
            //Convert the File data to Byte Array.
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }

            var conv = _unitOfWork.Conversations.GetAll().FirstOrDefault(x => x.Id == conversationId);
            var timeNow = DateTime.Now;

            MessageEntity newMessage = new MessageEntity()
            {
                IsAttachment = true,
                Status = Data.Enums.MessageStatus.DELIVERED,
                SendBy = currentUsername,
                Time = timeNow,
                Content = postedFile.FileName,
                Conversation = conv
            };

            _unitOfWork.Messages.Add(newMessage);

            FileEntity newFile = new FileEntity()
            {
                Message = newMessage,
                Name = Path.GetFileName(postedFile.FileName),
                ContentType = postedFile.ContentType,
                Data = bytes
            };

            _unitOfWork.Files.Add(newFile);
            _unitOfWork.Save();

            return newMessage;
        }

        public FileEntity GetFile(int messageId)
        {
            return _unitOfWork.Files.GetAll().FirstOrDefault(x => x.Message.Id == messageId);
        }

        public string GetUserNameForConversation(int conversationId, string currentUsername)
        {
            var conv = _unitOfWork.Conversations.GetAll().FirstOrDefault(x => x.Id == conversationId);
            if (conv.ToUser.Username == currentUsername)
            {
                return conv.FromUser.Username;
            }
            else
            {
                return conv.ToUser.Username;
            }
        }

        public void DeleteMessage(int messageId)
        {
            var message = _unitOfWork.Messages.GetAll().FirstOrDefault(x => x.Id == messageId);
            var file = _unitOfWork.Files.GetAll().FirstOrDefault(x => x.Message.Id == message.Id);

            if (message != null)
            {
                if (file != null)
                {
                    _unitOfWork.Files.Remove(file);
                }
                _unitOfWork.Messages.Remove(message);
                _unitOfWork.Save();
            }
        }
    }
}
