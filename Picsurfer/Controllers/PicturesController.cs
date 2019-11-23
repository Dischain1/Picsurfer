using Data;
using Data.Model;
using PagedList;
using Picsurfer.Models;
using Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Picsurfer.Controllers
{
    [Authorize]
    public class PicturesController : Controller
    {
        private PicsurferContext db = new PicsurferContext(ConnectionHelper.connStr);
        private PictureDataService pictureService = new PictureDataService(new PicsurferContext(ConnectionHelper.connStr));

        const int PicturesPerPage = 5;

        public ActionResult PictureList(int? page = 1)
        {
            var userId = db.GetUserId(User);

            var pictures = db.Pictures
                .Include(p => p.Rates)
                .Select(p => new UserRatedPicture
                {
                    Picture = p,
                    UserRate = p.Rates.Any(r => r.UserId == userId && r.Like == true) 
                        ? UserRate.Like 
                        : p.Rates.Any(r => r.UserId == userId && r.Like == false) 
                        ? UserRate.Dislike 
                        :UserRate.NotRated,
                })
                .ToList()
                .ToPagedList(page.Value, PicturesPerPage);

            return View(pictures);
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFiles(HttpPostedFileBase[] filesToUpload)
        {
            try
            {
                if (!pictureService.AreImages(filesToUpload))
                {
                    throw new ArgumentException("Не все загруженные файлы являются изображениями.");
                }

                var currentAppDir = AppDomain.CurrentDomain.BaseDirectory;

                pictureService.SavePicturesFromFiles(filesToUpload, currentAppDir);
                return RedirectToAction(nameof(PictureList));
            }
            catch (Exception e)
            {
                //todo сообщение об ошибке
                return View("Error", model: new Error(e.Message));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
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
