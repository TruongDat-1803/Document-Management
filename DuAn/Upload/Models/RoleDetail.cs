using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;

namespace Upload.Models
{
    public class RoleDetail : BaseModel
    {
        public int RoleDetailID { get; set; }

        public int RoleID { get; set; }

        public string SubCode { get; set; }

        public string SubName { get; set; }

        public string Action { get; set; }

        public string ActionName { get; set; }

        public bool Value { get; set; }

        public Guid TenantID { get; set; }
    }
}
