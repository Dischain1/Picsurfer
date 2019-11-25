using Data;
using Data.Model;
using PagedList;
using Picsurfer.Models;
using Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        const int PicturesPerPage = 9;

        public ActionResult PictureList(int? page = 1)
        {
            var pictures = GetPaigedUserRatedPictures(page.Value);
            return View(pictures);
        }

        public ActionResult FavouriteList(int? page = 1)
        {
            var userId = db.GetUserId(User);

            Expression<Func<Picture, bool>> likedExpression = x => x.Rates
                .Any(r => r.Like == true && r.UserId == userId);

            var pictures = GetPaigedUserRatedPictures(page.Value, likedExpression);
            return View(nameof(PictureList), pictures);
        }

        private IPagedList<UserRatedPicture> GetPaigedUserRatedPictures(int page, Expression<Func<Picture,bool>> expression = null)
        {
            var userId = db.GetUserId(User);

            var pictures = db.Pictures
                .Include(p => p.Rates);

            if (expression != null)
            {
                pictures = pictures.Where(expression);
            }

            return pictures.Select(
                p => 
                    new UserRatedPicture
                    {
                       Picture = p,
                       UserRate = p.Rates.Any(r => r.UserId == userId && r.Like == true)
                           ? UserRate.Like
                           : p.Rates.Any(r => r.UserId == userId && r.Like == false)
                           ? UserRate.Dislike
                           : UserRate.NotRated,
                   })
               .ToList()
               .ToPagedList(page, PicturesPerPage);
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

        [HttpPost]
        public void Delete(int pictureId)
        {
            pictureService.Delete(pictureId);
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
