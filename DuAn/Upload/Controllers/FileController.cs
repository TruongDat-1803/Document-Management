using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : BaseController<File>
    {
        public IFileBL _fileBL;
        public FileController(IFileBL fileBL, IBaseBL baseBL) : base(baseBL)
        {
            baseBL.SetTableName("file");
            this.BL = fileBL;
            _fileBL = fileBL;
            this.curentType = typeof(File);
        }

        [HttpPost("personal")]
        public async Task<ServiceResponse> GetAllPersonal(Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.GetAllPersonal(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }

        [HttpPost("search")]

        public async Task<ServiceResponse> SearchFile(Dictionary<string, object> param)
        {
            string _param = string.Empty;
            if (param.TryGetValue("searchInput", out object searchInput))
            {
                _param = searchInput.ToString(); // Chuyển đổi sang chuỗi
            }
            //string _param = param.ToString();
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.SearchFile(_param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }

            if (param.TryGetValue("employeeID", out object employeeId) && res.Data is List<File> files)
            {
                int id = JsonSerializer.Deserialize<int>(employeeId.ToString());
                res.Data = files.Where(file => file.CreatedBy == id).ToList(); // Lọc file
            }
            return res;
        }

        [HttpPost("insert-personal")]
        public async Task<ServiceResponse> InsertPersonal(Models.File file)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.InsertPersonal(file);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }

        [HttpPost("upload-file"), DisableRequestSizeLimit]
        public ServiceResponse UploadFile()
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                FileInfomation data = new FileInfomation();
                var file = Request.Form.Files[0];
                res.Data = _fileBL.UploadFile(file);
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);
            }
            return res;
        }
        [HttpPost("receive-file")]
        public async Task<ServiceResponse> GetRecieveFile(Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.GetReceiveFile(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpPost("search-file")]
        public async Task<ServiceResponse> NormalSearch(Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.NormalSearch(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }
        [HttpPost("share-file")]
        public async Task<ServiceResponse> GetShareFile(Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.GetShareFile(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }

        [HttpPost("unit")]
        public async Task<ServiceResponse> GetUnitFile(Dictionary<string, object> param)
        {
            ServiceResponse res = new ServiceResponse() { };
            try
            {
                res.Data = await _fileBL.GetUnitFile(param);
            }
            catch (Exception ex)
            {
                return _baseBL.Error(ex);
            }
            return res;
        }

        [HttpDelete("file/{id}")]
        public async Task<ServiceResponse> DeleteElastic(string id)
        {
            try
            {
                int _id = int.Parse(id.ToString());
                ServiceResponse res = new ServiceResponse();
                res.Data = await _fileBL.DeleteElastic(_id);
                return res;
            }
            catch (Exception e)
            {
                return _baseBL.Error(e);

            }
        }
    }
}
