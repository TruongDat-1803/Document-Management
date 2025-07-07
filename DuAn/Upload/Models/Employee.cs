using System;
using System.Collections.Generic;
using System.Text;
using Upload.Entity;

namespace Upload.Models
{
    public class Employee :BaseModel
    {
        public int EmployeeID { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public DateTime BirthDay { get; set; }

        public string Gender { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int OrganizationUnitID { get; set; }

        public string OrganizationUnitName { get; set; }

        public int? JobPositionID { get; set; }

        public string JobPositionName { get; set; }

        public int Status { get; set; }

        public Guid? TenantID { get; set; }
    }
}
