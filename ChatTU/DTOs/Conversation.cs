using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs
{
    public class Conversation
    {
        public string ToUser { get; set; }

        public string ToUserFirstName { get; set; }

        public string ToUserLastName { get; set; }

        public int ConversationId { get; set; }

        public string LastUpdated { get; set; }

        public bool PendingMessages { get; set; }

        public int Id { get; set; }
    }
}