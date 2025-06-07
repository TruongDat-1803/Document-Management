using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Entity
{
        public class ServiceResponse
        {
            public ServiceResponse()
            {
                Success = true;
            }
            public bool Success { get; set; }

            public object Data { get; set; }

            public string Message { get; set; }

            public int Code { get; set; }

        }
}
