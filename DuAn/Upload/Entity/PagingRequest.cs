using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Entity
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Filter { get; set; }
    }
}
