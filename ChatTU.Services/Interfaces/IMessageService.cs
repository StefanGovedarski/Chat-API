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

        MessageEntity AddMessage(int conversationId, string sendBy, string content, DateTime time);

        bool IsFullHistory(int conversationId, int messageCount);

        MessageEntity SaveFile(HttpPostedFile file, string currentUsername, int conversationId);

        string GetUserNameForConversation(int conversationId, string currentUsername);

        FileEntity GetFile(int messageId);

        void DeleteMessage(int messageId);
    }
}
