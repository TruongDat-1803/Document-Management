using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System;
using Upload.Interface;
using DocumentFormat.OpenXml.VariantTypes;
using Google.Apis.Drive.v3.Data;

namespace Upload.Implement
{
    public class GoogleBL : IGoogleBL
    {
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string ApplicationName = "DocumentSearch";

        public string UploadFile(string filePath)
        {
            UserCredential credential;

            var pathCredentials = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");

            using (var stream = new FileStream(pathCredentials, FileMode.Open, FileAccess.Read))
            {
                var folderName = Path.Combine("Resources", "GogoogleDriverAPI");
                var credPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Tạo dịch vụ Google Drive
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Tạo tệp tin để upload
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath)
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                if (filePath.Contains(".txt"))
                {
                    request = service.Files.Create(
                    fileMetadata, stream, "text/plain");
                    request.Fields = "id";
                    request.Upload();
                }
                else if (filePath.Contains(".png"))
                {
                    request = service.Files.Create(
                        fileMetadata, stream, "image/png");
                    request.Fields = "id";
                    request.Upload();
                }
                else if (filePath.Contains(".docx"))
                {
                    request = service.Files.Create(
                        fileMetadata, stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    request.Fields = "id";
                    request.Upload();
                }
                else if(filePath.Contains(".xlsx"))
                {
                    request = service.Files.Create(
                        fileMetadata, stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    request.Fields = "id";
                    request.Upload();
                }
                else
                {
                    request = service.Files.Create(
                        fileMetadata, stream, "application/pdf");
                    request.Fields = "id";
                    request.Upload();
                }
            }

            var file = request.ResponseBody;
            //Console.WriteLine("File ID: " + file.Id);

            // Lấy liên kết chia sẻ
            var permission = new Permission()
            {
                Type = "anyone",
                Role = "reader"
            };
            service.Permissions.Create(permission, file.Id).Execute();

            // Tạo liên kết chia sẻ
            string fileLink = $"https://drive.google.com/uc?id={file.Id}";
            string previewLink = $"https://drive.google.com/file/d/{file.Id}/preview";

            return previewLink;
        }
    }
}
