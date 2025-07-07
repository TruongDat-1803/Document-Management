using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class RoleController : BaseController<Role>
    {
        public RoleController(IRoleBL roleBL, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("role");
            this.BL = roleBL;
            this.curentType = typeof(Role);
        }

        [HttpDelete("delete-role/{id}")]
        public async Task<ServiceResponse> DeleteRole(string id)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await (this.BL as IRoleBL).DeleteRole(id);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
    }
}
