using ChatTU.Data.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatTU.Data.Models
{
    [Table("Conversations")]
    public class ConvesationEntity : BaseEntity
    {
        [Required]
        public int FromUserId { get; set; }

        [Required]
        public int ToUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public virtual UserEntity FromUser { get; set; }

        public virtual UserEntity ToUser { get; set; }
    }
}
