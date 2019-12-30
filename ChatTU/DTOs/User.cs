using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs
{
    public class User
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int Id { get; set; }
    }

    public class User_Admin
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int Id { get; set; }
    }
}