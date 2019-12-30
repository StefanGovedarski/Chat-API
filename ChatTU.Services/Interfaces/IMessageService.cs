using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace ChatTU.Services.Interfaces
{
    public interface IMessageService : IService
    {
        List<ConvesationEntity> GetConversationHistory(string currentUsername);

        ConvesationEntity GetOrCreateConversation(string currentUser, string targetUser);

        List<MessageEntity> GetMessages(int conversationId, int count);

        void AddMessage(int conversationId, string sendBy, string content, DateTime time);

        bool IsFullHistory(int conversationId, int messageCount);

        int SaveFile(HttpPostedFile file);

        List<ConvesationEntity> ADMIN_GetConvesationsForUser(string username);

        List<MessageEntity> ADMIN_GetMessagesForConversation(int conversationId);

        void ADMIN_EditMessage(int messageId, string content);

        void ADMIN_DeleteMessage(int messageId);

        void ADMIN_DeleteConversation(int conversationId);
    }
}
