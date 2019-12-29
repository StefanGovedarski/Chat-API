using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs
{
    public class Message
    {
        public int ConversationId { get; set; }

        public string Sender { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }

        public bool IsMine { get; set; }

        public int Id { get; set; }

        public bool IsAttachment { get; set; }

        public File Attachment { get; set; }
    }
}