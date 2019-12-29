using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using ChatTU.Data.Base;

namespace ChatTU.Data.Models
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Required, MaxLength(30)]
        [Index(IsUnique = true)]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required, MaxLength(30)]
        [StringLength(50, ErrorMessage = "The password must be atleast 5 characters long", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [DefaultValue(false)]
        public bool LoggedInStatus { get; set; }

        public virtual ICollection<UserRolesEntity> Roles { get; set; }
    }
}
