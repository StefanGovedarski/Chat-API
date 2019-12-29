using ChatTU.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data.Models
{
    [Table("File")]
    public class FileEntity
    {
        [Key, ForeignKey("Message")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public virtual MessageEntity Message { get; set; }
    }
}
