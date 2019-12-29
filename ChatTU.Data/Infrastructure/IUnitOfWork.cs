using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<UserEntity> Users { get; }

        IRepository<RoleEntity> Roles { get; }

        IRepository<UserRolesEntity> UserRoles { get; }

        IRepository<ConvesationEntity> Conversations { get; }

        IRepository<MessageEntity> Messages { get; }

        IRepository<FileEntity> Files { get; }

        int Save();
    }
}
