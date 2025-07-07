using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Threading.Tasks;
using Upload.Enum;
using Upload.Interface;
using Upload.Models;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Dapper;
using DocumentFormat.OpenXml.EMMA;

namespace Upload.Implement
{
    public class FileBL : BaseBL, IFileBL
    {
        public readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;
        private readonly IGoogleBL _googleBL;

        public FileBL(IConfiguration configuration, IElasticClient elasticClient, ILogger<FileBL> logger, IHttpContextAccessor httpContextAccessor, IGoogleBL googleBL) : base(configuration, httpContextAccessor)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _googleBL = googleBL;
        }

        public async Task<object> InsertPersonal(Models.File file)
        {
            var res = await Insert(file, typeof(Models.File));
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            int currentEmployeeId = user != null ? user.EmployeeID : 0;
            if (res.ToString() != "0")
            {
                file.CreatedBy = currentEmployeeId; 
                file.FileID = int.Parse(res.ToString());
                _ = SaveSingleAsync(file);
            }
            return res;
        }

        public  async Task<List<Models.File>> SearchFile(string param)
        {
            var response = await _elasticClient.SearchAsync<Models.File>(s =>
                s.Query(q =>
                    q.MatchPhrase(m => m
                        .Field(f => f.Content) 
                        .Query(param) 
                    )
                )
            );
            if (!response.IsValid)
            {
                _logger.LogError("Failed to search documents");
                return new List<Models.File>();
            }
            List<Models.File> files = new List<Models.File>();
            foreach (var hit in response.Hits)
            {
                files.Add(hit.Source);
            }
            return files;
        }

        public async Task<object> GetAllPersonal(Dictionary<string, object> param)
        {

            string where = string.Empty;
            if (param.ContainsKey("ParentID"))
            {
                where = $" AND ParentID = {param["ParentID"]}";
            }
            if (param.ContainsKey("OrganizationUnitID") && param["OrganizationUnitID"] != null)
            {
                where = where + $" AND OrganizationUnitID = {param["OrganizationUnitID"]}";
            }

            string sql = $"SELECT * FROM file WHERE TenantID = '{_tenantID}' AND CreatedBy = {param["EmployeeID"]} AND SharedIDs IS NULL {where} ORDER BY TypeFileEnum";

            return await QueryAsync<Models.File>(sql);
        }
        public async Task<object> GetUnitFile(Dictionary<string, object> param)
        {
            string where = string.Empty;
            if (param.ContainsKey("ParentID"))
            {
                where = $" AND ParentID = {param["ParentID"]}";
            }
            if (param.ContainsKey("OrganizationUnitID") && param["OrganizationUnitID"] != null)
            {
                where = where + $" AND OrganizationUnitID = {param["OrganizationUnitID"]}";
            }

            string sql = $"SELECT * FROM file WHERE TenantID = '{_tenantID}' AND SharedIDs IS NULL {where} ORDER BY TypeFileEnum";

            return await QueryAsync<Models.File>(sql);
        }
        public async Task<object> GetShareFile(Dictionary<string, object> param)
        {
            string where = string.Empty;
            if (param.ContainsKey("OrganizationUnitID") && param["OrganizationUnitID"] != null)
            {
                where = where + $" AND OrganizationUnitID = {param["OrganizationUnitID"]}";
            }

            //string sql = $"SELECT * FROM file WHERE TenantID = '{_tenantID}' AND CreatedBy = {param["EmployeeID"]}" + where + " Order By TypeFileEnum";
            string sql = $"SELECT * FROM file WHERE TenantID = '{_tenantID}' AND CreatedBy = {param["EmployeeID"]} AND TypeFile <> 'Folder' AND SharedIDs IS NULL {where} ORDER BY TypeFileEnum";

            return await QueryAsync<Models.File>(sql);
        }
        public async Task<object> NormalSearch(Dictionary<string, object> param)
        {
            string where = string.Empty;
            string sql = $"SELECT * FROM file WHERE (FileName = '{param["Search"]}' OR FileName LIKE CONCAT('{param["Search"]}', ' (%')) AND CreatedBy = {param["EmployeeID"]}";
            return await QueryAsync<Models.File>(sql);
        }

        public async Task<object> GetReceiveFile(Dictionary<string, object> param)
        {
            string where = string.Empty;
            string sql = $"SELECT * FROM file WHERE SharedIDs = {param["EmployeeID"]}" + where + " Order By TypeFileEnum";
            return await QueryAsync<Models.File>(sql);
        }
        public async Task<object> DeleteElastic(int _id)
        {
            string sql = $"DELETE FROM file WHERE fileID = '{_id}';";
            var response = await _elasticClient.DeleteByQueryAsync<Models.File>(q => q
                .Index("demoda")
                .Query(query => query
                    .Term(t => t
                        .Field(f => f.FileID)
                        .Value(_id)
                    )
                )
            );
            return await QueryAsync<Models.File>(sql);
        }

        /// <summary>
        /// Hàm lưu file bắn lên
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public object UploadFile(IFormFile file)
        {
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
                string content = string.Empty;
                string type = string.Empty;
                int typeFileEnum = 0;
                switch (typeFile)
                {
                    case "XLSX":
                        content = ReadExcel(dbPath.ToString());
                        type = "Excel";
                        typeFileEnum = 2;
                        break;
                    case "DOCX":
                        content = ReadDocx(dbPath.ToString());
                        type = "Word";
                        typeFileEnum = 1;
                        break;
                    case "PDF":
                        content = ReadPdf(dbPath.ToString());
                        type = "Pdf";
                        typeFileEnum = 3;
                        break;
                    case "TXT":
                        using (var reader = new StreamReader(fullPath))
                        {
                            content = reader.ReadToEnd();
                        }
                        type = "Text";
                        typeFileEnum = 4; 
                        break;
                    default:
                        break;
                }

                string _path = string.Empty;
                var drivePath = _googleBL.UploadFile(fullPath);
                //if (type == "Word")
                //{
                //    _path = drivePath;
                //}
                //else
                //{
                //    _path = _configuration.GetConnectionString("HostUpload") + dbPath.Replace("\\", "/");
                //}

                Models.File fileResponse = new Models.File()
                {
                    FileName = fileName,
                    TypeFile = type,
                    //Path = _configuration.GetConnectionString("HostUpload") + dbPath.Replace("\\", "/"),
                    Path = drivePath,
                    Size = (int)file.Length,
                    TypeFileEnum = typeFileEnum,
                    Content = content
                };
                //SaveSingleAsync(fileResponse);
                return fileResponse;
            }
            return null;
        }

        //public override async Task AfterSave(object param)
        //{
        //    await base.AfterSave(param);
        //    _ = SaveSingleAsync((Models.File)param);
        //}
        public override async Task AfterDelete(object model)
        {
            if (model is Models.File file)
            {
                string pathValue = file.Path;
                var settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("demoda")
                        .BasicAuthentication("tdat1803", "kQE4Hwre6RmYhRciAkqFCmsZ"); // Sử dụng thông tin đăng nhập
                var client = new ElasticClient(settings);
                var searchResponse = await client.SearchAsync<Models.File>
                    (s => s.Query(q => q.Term(t => t.Field(f => f.Path).Value(pathValue))).Size(1));
                if (searchResponse.Documents.Any())
                {
                    var document = searchResponse.Documents.First();
                    await client.DeleteAsync<Models.File>(document.FileID);
                }
            }
        }
        #region elasticsearch
        public async Task SaveSingleAsync(Models.File file)
        {
            var res = await _elasticClient.IndexDocumentAsync(file);
            return;
        }
        public async Task SaveManyAsync(Models.File[] files)
        {
            var result = await _elasticClient.IndexManyAsync(files);
            if (result.Errors)
            {
                // the response can be inspected for errors
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                }
            }
        }

        public async Task SaveBulkAsync(Models.File[] files)
        {
            var result = await _elasticClient.BulkAsync(b => b.Index("files").IndexMany(files));
            if (result.Errors)
            {
                // the response can be inspected for errors
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                }
            }
        }

        public async Task DeleteAsync(Models.File file)
        {
            await _elasticClient.DeleteAsync<Models.File>(file);
        }

        public async Task<List<Models.File>> Find(string query)
        {
            var response = await _elasticClient.SearchAsync<Models.File>(
                s => s.Query(q => q.Match(d => d.Query(query))));
            if (!response.IsValid)
            {
                _logger.LogError("Failed to search documents");
                return new List<Models.File>();
            }
            List<Models.File> files = new List<Models.File>();
            foreach(var hit in response.Hits)
            {
                files.Add(hit.Source);
            }
            return files;
        }

        private static string GetSearchUrl(string query, int page, int pageSize)
        {
            return $"/search?query={Uri.EscapeDataString(query ?? "")}&page={page}&pagesize={pageSize}/";
        }
        #endregion

        #region đọc các loại file ra dạng string
        public string ReadExcel(string path)
        {
            string content = string.Empty;
            try
            {
                DataTable dtTable = new DataTable();
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                    {
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                        SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();
                        for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                        {
                            List<string> rowList = new List<string>();
                            for (int rCnt1 = 0; rCnt1 < thesheetdata.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
                            {
                                Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                if (rCnt == 0)
                                                {
                                                    dtTable.Columns.Add(item.Text.Text);
                                                }
                                                else
                                                {
                                                    rowList.Add(item.Text.Text);
                                                }
                                                content = content + item.Text.Text + " ";
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (rCnt != 0)
                                    {
                                        rowList.Add(thecurrentcell.InnerText);
                                    }
                                }
                            }
                            if (rCnt != 0)
                                dtTable.Rows.Add(rowList.ToArray());

                        }

                    }
                    return content;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public string ReadDocx(string path)
        {
            string content = string.Empty;
            List<object> data = new List<object>();
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(path, true))
            {
                var body = wordprocessingDocument.MainDocumentPart.Document.Body;
                var paragraphs = new List<Paragraph>();
                foreach (Paragraph para in body.Descendants<Paragraph>().ToList())
                {
                    if (para != null)
                    {
                        content = content + para.InnerText + " ";
                    }
                }
            }
            return content;
        }

        public static string ReadPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();
            string content = string.Empty;

            using (PdfReader pdfReader = new PdfReader(filePath))
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    // Lấy nội dung của trang
                    var page = pdfDocument.GetPage(i);
                    string pageText = PdfTextExtractor.GetTextFromPage(page);
                    text.AppendLine(pageText);
                }
            }
            content = text.ToString();
            return content;
        }
        #endregion
    }
}
