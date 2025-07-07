using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;

namespace Upload.Models
{
    public class Role : BaseModel
    {
        public int RoleID { get; set; }

        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public string Note { get; set; }

        public Guid TenantID { get; set; }

        public List<RoleDetail> ListRole { get; set; }
    }
}
