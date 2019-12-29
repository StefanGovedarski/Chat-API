using ChatTU.Data.Infrastructure;
using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTU.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatTuContext _context;

        public UnitOfWork(ChatTuContext context)
        {
            _context = context;
            Users = new Repository<UserEntity>(_context);
            Roles = new Repository<RoleEntity>(_context);
            UserRoles = new Repository<UserRolesEntity>(_context);
            Conversations = new Repository<ConvesationEntity>(_context);
            Messages = new Repository<MessageEntity>(_context);
            Files = new Repository<FileEntity>(_context);
        }

        public IRepository<UserEntity> Users { get; }
        public IRepository<RoleEntity> Roles { get; }
        public IRepository<UserRolesEntity> UserRoles { get; }
        public IRepository<ConvesationEntity> Conversations { get; }
        public IRepository<MessageEntity> Messages { get; }
        public IRepository<FileEntity> Files{ get; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }
    }
}
