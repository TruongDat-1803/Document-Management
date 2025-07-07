using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.Interface
{
    public interface IBaseUploadBL: IBaseBL
    {
        public object ReadExcel(string path);

        public object ReadDocx(string path);
    }
}
