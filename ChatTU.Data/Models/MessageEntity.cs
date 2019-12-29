using ChatTU.Data.Base;
using ChatTU.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatTU.Data.Models
{
    [Table("Message")]
    public class MessageEntity : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string SendBy { get; set; }

        [Required]
        public MessageStatus Status { get; set; }

        [Required]
        public virtual ConvesationEntity Conversation { get; set; }

        [DefaultValue(false)]
        public bool IsAttachment { get; set; }

        public virtual FileEntity File { get; set; }
    }
}
