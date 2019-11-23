using Data;
using Data.Model;
using PagedList;
using Picsurfer.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Picsurfer
{
    [Authorize]
    public class RatesController : Controller
    {
        private PicsurferContext db = new PicsurferContext(ConnectionHelper.connStr);

        [HttpPost]
        public void Like(int pictureId)
        {
            RatePicture(pictureId);
        }

        [HttpPost]
        public void Dislike(int pictureId)
        {
            RatePicture(pictureId, rate: false);
        }

        [HttpGet]
        public ActionResult RatedPictures(int? page = 1)
        {
            var ratedPictures = db.Pictures.Select(picture => new RatedPicture
            {
                Picture = picture,
                Likes = picture.Rates.Where(r => r.Like == true).Count(),
                Dislikes = picture.Rates.Where(r => r.Like == false).Count(),
            })
            .ToList();

            foreach (var picture in ratedPictures)
            {
                picture.Rating = picture.Likes - picture.Dislikes;
            }

            return View(ratedPictures.OrderByDescending(x => x.Rating).ToPagedList(page.Value, 5));
        }

        private void RatePicture(int pictureId, bool rate = true)
        {
            var userId = db.GetUserId(User);

            var previousRate = db.Rates
                .Where(x => x.PictureId == pictureId && x.UserId == userId)
                .FirstOrDefault();

            if (previousRate != null)
            {
                previousRate.Like = rate;
                db.SaveChanges();
            }
            else
            {
                var newRate = new Rate
                {
                    Like = rate,
                    PictureId = pictureId,
                    UserId = userId
                };
                db.Rates.Add(newRate);
                db.SaveChanges();
            }
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
