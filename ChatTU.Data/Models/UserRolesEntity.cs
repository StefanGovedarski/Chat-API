using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data.Models
{
    [Table("UserRoles")]
    public class UserRolesEntity
    {
        [Key, Column(Order = 0)]
        public int UserID { get; set; }
        [Key, Column(Order = 1)]
        public int RoleID { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
