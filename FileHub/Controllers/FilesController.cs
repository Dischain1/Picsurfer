using Data;
using FileHub.Models;
using PagedList;
using Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileHub.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private FileHubContext db = new FileHubContext(ConnectionHelper.connStr);
        private FileDataService fileService = new FileDataService(new FileHubContext(ConnectionHelper.connStr));

        const int FilesPerPage = 10;

        public ActionResult FileList(int? page = 1)
        {
            var files = db.Files.ToList().ToPagedList(page.Value, FilesPerPage);
            return View(files);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UploadPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFiles(HttpPostedFileBase[] filesToUpload)
        {
            try
            {
                var currentAppDir = AppDomain.CurrentDomain.BaseDirectory;

                fileService.SaveFiles(filesToUpload, currentAppDir);
                return RedirectToAction(nameof(FileList));
            }
            catch (Exception e)
            {
                //todo сообщение об ошибке
                return View("Error", model: new Error(e.Message));
            }
        }

        [HttpPost]
        public void Delete(int fileId)
        {
            fileService.Delete(fileId);
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
