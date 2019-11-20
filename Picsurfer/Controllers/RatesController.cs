using Data;
using Data.Model;
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
            RatePicture(pictureId, like: false);
        }

        private void RatePicture(int pictureId, bool like = true)
        {
            if (int.TryParse(HttpContext.User.Identity.Name, out int userId))
            {
                var rate = new Rate
                {
                    Like = like,
                    PictureId = pictureId,
                    UserId = userId
                };

                if (!db.Rates.Any(x => x.PictureId == pictureId && x.UserId == userId))
                {
                    db.Rates.Add(rate);
                    db.SaveChanges();
                }
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
