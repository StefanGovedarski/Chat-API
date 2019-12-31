using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Services.Interfaces
{
    public interface IAdminService : IService
    {
        IEnumerable<UserEntity> ADMIN_GetAllUsers();

        UserEntity ADMIN_GetUser(string username, int id);

        void ADMIN_EditUser(int id, string username, string password, string firstname, string lastname);

        void ADMIN_DeleteUser(string username, int id);

        List<ConvesationEntity> ADMIN_GetConvesationsForUser(string username);

        List<MessageEntity> ADMIN_GetMessagesForConversation(int conversationId);

        void ADMIN_EditMessage(int messageId, string content);

        void ADMIN_DeleteConversation(int conversationId);

        void ADMIN_DeleteFile(int id);
    }
}
