using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Upload.Entity;
using Upload.Implement;
using Upload.Interface;
using Upload.Models;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController<FileInfomation>
    {
        public IBaseUploadBL _baseUploadBL;
        public IConfiguration _configuration;
        public UploadController(IBaseUploadBL baseUploadBL, IBaseBL baseBL, IConfiguration configuration) : base(baseBL)
        {
            _baseUploadBL = baseUploadBL;
            _configuration = configuration;
            baseBL.SetTableName("file_information");
            this.BL = baseUploadBL;
            this.curentType = typeof(FileInfomation);
        }
        [HttpPost, DisableRequestSizeLimit]
        public ServiceResponse Upload()
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                FileInfomation data = new FileInfomation();
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Document");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var listName = fileName.Split(".").ToList();
                    string typeFile = listName[listName.Count - 1].ToString().ToUpper();
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    data.FileName = fileName;
                    data.Type = typeFile;
                    data.Size = (int?)file.Length;
                    data.Path = _configuration.GetConnectionString("HostUpload") +  dbPath.Replace("\\","/");
                    var content = new Object();
                    switch (typeFile)
                    {
                        case "XLSX":
                            content = this._baseUploadBL.ReadExcel(dbPath.ToString());
                            data.Content = JsonConvert.SerializeObject(content);
                            break;
                        case "DOCX":
                            content = this._baseUploadBL.ReadDocx(dbPath.ToString());
                            data.Content = JsonConvert.SerializeObject(content);
                            break;
                        default:
                            break;
                    }
                    res.Data = data;
                    res.Success = true;
                    _ = Insert(data);
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Can't file upload";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.ToString();
                return res;
            }
        }
    }
}
