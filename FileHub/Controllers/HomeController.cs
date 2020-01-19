using Data;
using Data.Model;
using FileHub.Models;
using System.Linq;
using System.Web.Mvc;

namespace FileHub.Controllers
{
    public class HomeController : Controller
    {
        private FileHubContext _context;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult InitializeDatabase()
        {
            _context = new FileHubContext(ConnectionHelper.connStr);
            if (!_context.Users.Any())
            {
                var adminUser = new User
                {
                    Email = "qwe@qwe.ru",
                    PasswordHash = "202cb962ac59075b964b07152d234b70",
                    IsAdmin = true
                };

                _context.Users.Add(adminUser);
                _context.SaveChanges();
            }
            ViewBag.Title = "DB is initialised";

            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Разработан для учебных целей.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Мои контактные данные.";

            return View();
        }
    }
}
