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

        public IEnumerable<UserEntity> FindUsers(string searchQuery, string currentUser)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.Firstname.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) ||
                                                         x.Lastname.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) ||
                                                         x.Username.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase) &&
                                                         x.Username != currentUser)
                                                         .Take(10);

            return users;
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
