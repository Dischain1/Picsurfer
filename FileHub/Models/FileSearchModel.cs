using Data.Model;
using PagedList;
namespace FileHub.Models
{
    public class FileSearchModel
    {
        public IPagedList<File> Files { get; set; }
        public string query { get; set; }
    }
}