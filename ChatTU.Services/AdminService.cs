using ChatTU.Data;
using ChatTU.Data.Infrastructure;
using ChatTU.Data.Models;
using ChatTU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork _unitOfWork;

        public AdminService()
        {
            _unitOfWork = new UnitOfWork(new ChatTuContext());
        }
        public void ADMIN_DeleteUser(string username, int id)
        {
            UserEntity user;
            if (string.IsNullOrEmpty(username))
            {
                user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Id == id);
            }
            else
            {
                user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username);
            }

            if (user == null)
            {
                throw new ArgumentException("Invalid values for username and id provided. No such user exists in the database");
            }

            var conversations = _unitOfWork.Conversations.GetAll().Where(x => x.FromUser.Id == user.Id || x.ToUser.Id == user.Id);
            var conversationIds = conversations.Select(x => x.Id).ToList();
            var messages = _unitOfWork.Messages.GetAll().Where(x => conversationIds.Contains(x.Conversation.Id));
            var files = _unitOfWork.Files.GetAll().Where(x => messages.Select(m => m.Id).Contains(x.Message.Id));
            _unitOfWork.Files.RemoveRange(files);
            _unitOfWork.Messages.RemoveRange(messages);
            _unitOfWork.Conversations.RemoveRange(conversations);
            _unitOfWork.Users.Remove(user);
            _unitOfWork.Save();
        }

        public IEnumerable<UserEntity> ADMIN_GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAll();

            return users;
        }

        public UserEntity ADMIN_GetUser(string username, int id)
        {
            UserEntity user;
            if (string.IsNullOrEmpty(username))
            {
                user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Id == id);
            }
            else
            {
                user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username);
            }

            return user;
        }

        public void ADMIN_EditUser(int id, string username, string password, string firstname, string lastname)
        {
            var user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Id == id);
            if(user != null)
            {
                if (!string.IsNullOrEmpty(username))
                {
                    user.Username = username;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = password;
                }
                if (!string.IsNullOrEmpty(firstname))
                {
                    user.Firstname = firstname;
                }
                if (!string.IsNullOrEmpty(lastname))
                {
                    user.Lastname = lastname;
                }
            }
            _unitOfWork.Save();
        }

        public void ADMIN_DeleteConversation(int conversationId)
        {
            var conv = _unitOfWork.Conversations.GetAll().FirstOrDefault(x => x.Id == conversationId);
            var messages = _unitOfWork.Messages.GetAll().Where(x => x.Conversation.Id == conversationId);
            var files = _unitOfWork.Files.GetAll().Where(x => messages.Select(m => m.Id).Contains(x.Message.Id));
            if (conv != null)
            {
                _unitOfWork.Files.RemoveRange(files);
                _unitOfWork.Messages.RemoveRange(messages);
                _unitOfWork.Conversations.Remove(conv);
                _unitOfWork.Save();
            }
        }

        public void ADMIN_EditMessage(int messageId, string content)
        {
            var message = _unitOfWork.Messages.GetAll().FirstOrDefault(x => x.Id == messageId);
            if (message != null)
            {
                message.Content = content;
                _unitOfWork.Save();

            }
        }

        public List<ConvesationEntity> ADMIN_GetConvesationsForUser(string username)
        {
            var conversations = new List<ConvesationEntity>();
            var user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                conversations = _unitOfWork.Conversations.GetAll().Where(x => x.FromUserId == user.Id || x.ToUserId == user.Id).ToList();
            }
            return conversations;
        }

        public void ADMIN_DeleteFile(int id)
        {
            var file = _unitOfWork.Files.GetAll().FirstOrDefault(x => x.Id == id);
            if (file != null)
            {
                _unitOfWork.Files.Remove(file);
                _unitOfWork.Save();
            }
        }

        public List<MessageEntity> ADMIN_GetMessagesForConversation(int conversationId)
        {
            return _unitOfWork.Messages.GetAll().Where(x => x.Conversation.Id == conversationId).ToList();
        }
    }
}
