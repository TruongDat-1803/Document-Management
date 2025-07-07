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
    public class OrganizationUnitController : BaseController<OrganizationUnit>
    {
        public OrganizationUnitController(IOrganizationUnitBL organizationUnitBL, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("organization_unit");
            this.BL = organizationUnitBL;
            this.curentType = typeof(OrganizationUnit);
        }
    }
}
