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
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService()
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
            _unitOfWork.Messages.RemoveRange(messages);
            _unitOfWork.Conversations.RemoveRange(conversations);
            _unitOfWork.Users.Remove(user);
            _unitOfWork.Save();
        }

        public IEnumerable<UserEntity> FindUsers(string searchQuery, string currentUser)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.Firstname.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) ||
                                                         x.Lastname.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) ||
                                                         x.Username.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) &&
                                                         x.Username != currentUser)
                                                         .Take(10);

            return users;
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

        public bool GetUsersLoggedInStatus(string username)
        {
            return _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username).LoggedInStatus;
        }

        public async Task MarkLoggedInStatusAs(string username, bool status)
        {
            var user = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                user.LoggedInStatus = status;
                _unitOfWork.Save();
            }
        }

        public void Register(string username, string password, string firstname, string lastname, string roleName)
        {
            var user = new UserEntity()
            {
                Username = username,
                Password = password,
                Firstname = firstname,
                Lastname = lastname
            };

            var role = _unitOfWork.Roles.GetAll().Where(x => x.RoleName == roleName).SingleOrDefault();

            if (role == null)
            {
                throw new ArgumentException("Invalid role provided.");
            }

            var userWithRole = new UserRolesEntity()
            {
                User = user,
                Role = role
            };

            // This also adds the user.
            _unitOfWork.UserRoles.Add(userWithRole);
            _unitOfWork.Save();

        }
    }
}
