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

        public ActionResult PictureList(int? page)
        {
            var pageNumber = page ?? 1;

            var pictures = db.Pictures
                .ToList()
                .ToPagedList(pageNumber, PicturesPerPage);

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

        // GET: Pictures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //todo сообщение об ошибке
                return View("Error", model: new Error(e.Message));
            }
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Path,Name,Extension")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Pictures/Delete/5
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
