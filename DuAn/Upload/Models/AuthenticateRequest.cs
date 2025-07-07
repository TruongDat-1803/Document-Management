using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Models
{
    public class AuthenticateRequest
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public AuthenticateRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
