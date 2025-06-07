using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleDetailController : BaseController<RoleDetail>
    {
        public RoleDetailController(IRoleDetailBL roleDetail, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("role_detail");
            this.BL = roleDetail;
            this.curentType = typeof(RoleDetail);
        }
        [HttpGet("list-role/{id}")]
        public async Task<ServiceResponse> GetListRoleDetail(string id)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IRoleDetailBL).GetListRoleDetail(id);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpPost("save-list-role")]
        public async Task<ServiceResponse> AddRoleDetail([FromBody] Role role)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                res.Data = await (this.BL as IRoleDetailBL).AddRoleDetail(role);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
    }
}
