using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Models;

namespace Upload.Interface
{
    public interface IRoleDetailBL : IBaseBL
    {
        public Task<object> GetListRoleDetail(string id);

        public Task<object> AddRoleDetail(Role role);
    }
}
