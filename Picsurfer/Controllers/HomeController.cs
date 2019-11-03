using Data;
using Data.Model;
using Picsurfer.Models;
using System.Linq;
using System.Web.Mvc;

namespace Picsurfer.Controllers
{
    public class HomeController : Controller
    {
        private PicsurferContext _context;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult InitializeDatabase()
        {
            _context = new PicsurferContext(ConnectionHelper.connStr);
            if (!_context.Users.Any())
            {
                var user = new User
                {
                    FirstName = "test1",
                    SecondName = "test2",
                    Patronimic = "test3",

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
