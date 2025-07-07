using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Enum;

namespace Upload.Models
{
    public class File : BaseModel
    {
        public int FileID { get; set; }

        public string FileName { get; set; }

        public int? OrganizationUnitID { get; set; }

        public string OrganizationUnitName { get; set; }

        public int ParentID { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string SharedIDs { get; set; }

        public string SharedNames { get; set; }

        public DateTime SharedDate { get; set; }

        public string Note { get; set; }

        public string NoteShared { get; set; }

        public string NoteOrganizationUnit { get; set; }

        public string TypeFile { get; set; }

        public string Path { get; set; }

        public string Content { get; set; }

        public Guid TenantID { get; set; }

        public int Size { get; set; }

        public int TypeFileEnum { get; set; }
    }
}
