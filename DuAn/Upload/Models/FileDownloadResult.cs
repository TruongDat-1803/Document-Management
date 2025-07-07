using System.IO;

namespace Upload.Models
{
    public class FileDownloadResult
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
} 