using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Picsurfer.Controllers
{
    public class HomeController : Controller
    {
        private PictureSurferContext _context;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult InitializeDatabase()
        {
            _context = new PictureSurferContext();
            if (!_context.Users.Any())
            {
                var user = new User
                {
                    FirstName = "test",
                    SecondName = "test",
                    Patronimic = "test",

                    IsAdmin = false
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            ViewBag.Title = "DB is initialised";

            return View("Index");
        }
    }
}
