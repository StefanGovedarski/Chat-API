using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data
{
    public class ChatTuContext : DbContext
    {
        public ChatTuContext() : base("name=ChatTuDatabase")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRolesEntity> UserRoles { get; set; }

        public DbSet<ConvesationEntity> Conversations { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }
}
