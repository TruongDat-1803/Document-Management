
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Upload.Entity;
using Upload.Interface;
using Upload.Models;

namespace Upload.Implement
{
    public class BaseUploadBL : BaseBL, IBaseUploadBL
    {
        public BaseUploadBL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public ServiceResponse OnSuccess(object data)
        {
            ServiceResponse res = new ServiceResponse();
            res.Data = data;
            res.Success = true;
            res.Code = 200;
            return res;
        }

        public ServiceResponse OnError(string error)
        {
            ServiceResponse res = new ServiceResponse();
            res.Data = null;
            res.Success = false;
            res.Code = 99;
            res.Message = error;
            return res;
        }

        public object ReadExcel(string path)
        {
            var data = new Object();
            try
            {
                DataTable dtTable = new DataTable();
                //Đọc file excel được gửi lên
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, false))
                {
                    //Lấy ra danh sách sheet
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    //Đọc các trang sheet để lấy ra dữ liệu
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
                                                //Lấy danh sách cột đầu làm trường khóa
                                                if (rCnt == 0)
                                                {
                                                    dtTable.Columns.Add(item.Text.Text);
                                                }
                                                //Nếu không phải cột đầu thì gán giá trị vào
                                                else
                                                {
                                                    rowList.Add(item.Text.Text);
                                                }
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

                    return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public object ReadDocx(string path)
        {
            List<object> data = new List<object>();
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(path, true))
            {
                var body = wordprocessingDocument.MainDocumentPart.Document.Body;
                var paragraphs = new List<Paragraph>();
                foreach (Paragraph para in body.Descendants<Paragraph>().ToList())
                {
                    if(para != null)
                    {
                        var item = new object();
                        if (para.ParagraphProperties == null)
                        {
                            item = new
                            {
                                Level = "Body",
                                Content = para.InnerText
                            };
                        }
                        else
                        {
                            item = new
                            {
                                Type = para.ParagraphProperties.ParagraphStyleId.Val.Value,
                                Content = para.InnerText
                            };
                        }
                        data.Add(item);
                    }
                }
            }
            return data;
        }
    }
}
