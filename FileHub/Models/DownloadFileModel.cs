using Data.Model;

namespace FileHub.Models
{
    public class DownloadFileModel
    {
        public File File { get; set; }
        public int? DownloadsCount { get; set; }
    }
}