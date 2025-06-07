using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Models
{
    public class FileInfomation
    {
        public int FileInfomationID { get; set; }
        public string FileName { get; set; }

        public string Type { get; set; }

        public int? Size { get; set; }

        public string Path { get; set; }

        public string Content { get; set; }

    }
}
