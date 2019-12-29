using ChatTU.Data;
using ChatTU.Data.Infrastructure;
using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatTU.Infrastructure
{
    public class Security
    {
        public static UserEntity Login(string username, string password)
        {
            UserEntity login;

            using (var unitOfWork = new UnitOfWork(new ChatTuContext()))
            {
                login = unitOfWork.Users.Include(a => a.Roles).FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password.Equals(password));
            }
            return login;
        }

        internal static string[] GetUserRoles(UserEntity user)
        {
            using (var unitOfWork = new UnitOfWork(new ChatTuContext()))
            {
                var userRoleIds = user.Roles.Select(x => x.RoleID);
                return unitOfWork.Roles.GetAll().Where(x => userRoleIds.Contains(x.Id)).Select(r => r.RoleName.ToUpper().Trim()).ToArray();
            }
        }
    }
}