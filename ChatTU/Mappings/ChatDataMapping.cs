using ChatTU.Data.Models;
using ChatTU.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.Mappings
{
    public class ChatDataMapping
    {
        public static ChatData ToChatDataDto(ConvesationEntity convEntity, List<MessageEntity> messageEntities,string username, string targetUsername, bool targetUserOnline, bool isFullChatHistory)
        {
            return new ChatData
            {
                TargetUsername = targetUsername,
                TargetUserOnline = targetUserOnline,
                Conversation = ConversationMapping.ToConversationDto(convEntity, username),
                Messages = messageEntities.Select(x => MessageMapping.ToMessageDto(x, username)).ToList(),
                IsFullChatHistory = isFullChatHistory
            };
        }
    }
}