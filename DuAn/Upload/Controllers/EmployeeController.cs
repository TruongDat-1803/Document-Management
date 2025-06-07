using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : BaseController<Employee>
    {
        public EmployeeController(IEmployeeBL employeeBL, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("employee");
            this.BL = employeeBL;
            this.curentType = typeof(Employee);
        }
    }
}
