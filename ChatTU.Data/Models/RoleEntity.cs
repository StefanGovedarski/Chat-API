using ChatTU.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data.Models
{
    [Table("Role")]
    public class RoleEntity : BaseEntity
    {
        public string RoleName { get; set; }

        public virtual ICollection<UserRolesEntity> Users { get; set; }
    }
}
