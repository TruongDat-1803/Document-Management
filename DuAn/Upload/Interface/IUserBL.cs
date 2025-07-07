using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Models;

namespace Upload.Interface
{
    public interface IUserBL : IBaseBL
    {
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        public Task<object> Signup(Dictionary<string, object> param);

        public Task<object> UserInfo();

        public Task<object> UserProfile();

        public Task<object> InsertUser(User _user);

        public Task<object> Delete_User(int id);
    }
}
