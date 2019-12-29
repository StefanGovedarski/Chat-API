using ChatTU.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTU.Services.Interfaces
{
    public interface IUserService : IService
    {
        void Register(string username, string password, string firstname, string lastname, string role);

        IEnumerable<UserEntity> FindUsers(string searchQuery, string currentUser);

        bool GetUsersLoggedInStatus(string username);

        Task MarkLoggedInStatusAs(string username, bool status);

        IEnumerable<UserEntity> ADMIN_GetAllUsers();

        UserEntity ADMIN_GetUser(string username, int id);

        void ADMIN_DeleteUser(string username, int id);


    }
}
