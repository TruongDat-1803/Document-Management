using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Interface
{
    public interface IRoleBL : IBaseBL
    {
        public Task<object> DeleteRole(string id);
    }
}
