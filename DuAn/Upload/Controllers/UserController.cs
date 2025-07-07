using DocumentFormat.OpenXml.EMMA;
using Elastic.Clients.Elasticsearch.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Implement;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<User>
    {
        public IUserBL _userBL;
        public UserController(IUserBL userBL, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("user");
            this.BL = userBL;
            this.curentType = typeof(User);
        }

        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody]AuthenticateRequest model)
        {
            var response = await (this.BL as IUserBL).Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpGet("user-info")]
        [Models.Authorize]
        public async Task<ServiceResponse> UserInfo()
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IUserBL).UserInfo();
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpGet("user-profile")]
        [Models.Authorize]
        public async Task<ServiceResponse> UserProfile()
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IUserBL).UserProfile();
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpPost("signup")]
        public async Task<ServiceResponse> Signup([FromBody] Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IUserBL).Signup(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpPost("insert-user")]
        public async Task<ServiceResponse> InsertUser(User _user)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IUserBL).InsertUser(_user);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }

        [HttpDelete("delete-user")]
        public async Task<ServiceResponse> Delete_User(string id)
        {

            int _idInt = int.Parse(id);
            try
            {
                ServiceResponse res = new ServiceResponse();
                res.Data = await (this.BL as IUserBL).Delete_User(_idInt);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
        }
    }
}
