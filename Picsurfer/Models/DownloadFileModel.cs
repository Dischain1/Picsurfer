using Data.Model;

namespace Picsurfer.Models
{
    public class DownloadFileModel
    {
        public File File { get; set; }
        public int? DownloadsCount { get; set; }
    }
}