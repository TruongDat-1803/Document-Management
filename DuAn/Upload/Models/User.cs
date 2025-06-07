using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;

namespace Upload.Models
{
    public class User : BaseModel
    {
        public Guid UserID { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public int OrganizationUnitID { get; set; }

        public string OrganizationUnitName { get; set; }

        public Guid TenantID { get; set; }

    }
}
