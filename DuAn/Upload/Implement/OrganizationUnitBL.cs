using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Interface;

namespace Upload.Implement
{
    public class OrganizationUnitBL : BaseBL, IOrganizationUnitBL
    {
        public OrganizationUnitBL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
    }
}
