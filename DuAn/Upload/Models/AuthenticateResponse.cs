using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Models
{
    public class AuthenticateResponse
    {
        public Guid UserID { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            UserID = user.UserID;
            Token = token;
        }
    }
}
