using ChatTU.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.Mappings
{
    public static class UserAuthMapping
    {
        public static object UserAuthMap(UserEntity userEntity)
        {
            if(userEntity == null)
            {
                return null;
            }

            return new
            {
                Username = userEntity.Username,
                Password = userEntity.Password,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname,
                Roles = userEntity.Roles.ToList()
            };
        }
    }
}