using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs
{
    public class ChatData
    {
        public Conversation Conversation {get;set;}
        public List<Message> Messages { get; set; }
        public string TargetUsername { get; set; }
        public bool TargetUserOnline { get; set; }
        public bool IsFullChatHistory { get; set; }
    }
}