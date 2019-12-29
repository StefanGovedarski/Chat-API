using ChatTU.Data.Models;
using ChatTU.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.Mappings
{
    public class UserMappings
    {
        public static User ToUserDto(UserEntity userEntity)
        {
            return new User
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname
            };
        }
    }
}