using ChatTU.Data.Models;
using ChatTU.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.Mappings
{
    public class MessageMapping
    {
        public static Message ToMessageDto(MessageEntity message, string currentUser)
        {
            return new Message
            {
                Id = message.Id,
                ConversationId = message.Conversation.Id,
                Sender = message.SendBy,
                Content = message.Content,
                Time = message.Time,
                IsMine = message.SendBy == currentUser ? true : false,
                IsAttachment = message.IsAttachment
            };
        }
    }
}