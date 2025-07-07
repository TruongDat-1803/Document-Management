using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Interface;

namespace Upload.Implement
{
    public class RoleBL : BaseBL, IRoleBL
    {
        public RoleBL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public async Task<object> DeleteRole(string id)
        {
            string sql = $"DELETE FROM role_detail WHERE RoleID = {int.Parse(id)} ;DELETE FROM role WHERE RoleID = {int.Parse(id)}; ";
            var res = await ExecuteAsync(sql);
            return res;
        }
    }
}
