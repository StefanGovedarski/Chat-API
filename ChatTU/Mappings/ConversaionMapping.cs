using ChatTU.Data.Models;
using ChatTU.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ChatTU.Mappings
{
    public static class ConversationMapping
    {
        public static Conversation ToConversationDto(ConvesationEntity convEntity, string currentUsername)
        {
            return new Conversation
            {
                Id = convEntity.Id,
                ToUser = convEntity.ToUser.Username != currentUsername ? convEntity.ToUser.Username : convEntity.FromUser.Username,
                ToUserFirstName = convEntity.ToUser.Username != currentUsername ? convEntity.ToUser.Firstname : convEntity.FromUser.Lastname,
                ToUserLastName = convEntity.ToUser.Username != currentUsername ? convEntity.ToUser.Lastname : convEntity.FromUser.Lastname,
                ConversationId = convEntity.Id,
                LastUpdated = convEntity.LastUpdated.ToString("g", CultureInfo.CurrentCulture),
                PendingMessages = false
            };
        }
    }
}