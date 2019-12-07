using Data;
using Data.Model;
using PagedList;
using Picsurfer.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Picsurfer
{
    [Authorize]
    public class DownloadsController : Controller
    {
        private FileHubContext db = new FileHubContext(ConnectionHelper.connStr);
        private const int FilesPerPage = 10;

        [HttpGet]
        public ActionResult DownloadStatisctics(int? page = 1)
        {
            var model = db.Files
                .Include(p => p.Downloads)
                .Select(
                p =>
                    new DownloadFileModel
                    {
                        File = p,
                        DownloadsCount = p.Downloads.Sum(x => x.Count)
                   })
                   .OrderByDescending(dfm => dfm.DownloadsCount)
               .ToList()
               .ToPagedList(page.Value, FilesPerPage);

            return View(model);
        }

        public FileResult Download(int fileId)
        {
            var file = db.Files.FirstOrDefault(f => f.Id == fileId);

            if (file!= null)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(file.Path);
                string fileName = file.Name;

                IncreaseDownloads(file.Id);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return null;
        }

        private void IncreaseDownloads(int fileId)
        {
            var userId = db.GetUserId(User);

            var doneDownloads = db.Downloads
                .Where(x => x.FileId == fileId && x.UserId == userId)
                .FirstOrDefault();

            if (doneDownloads == null)
            {
                var newDownload = new Download
                {
                    FileId = fileId,
                    UserId = userId,
                    Count = 1
                };
                db.Downloads.Add(newDownload);
            }
            else
            {
                doneDownloads.Count += 1;
            }

            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
