using Data;
using Data.Model;
using FileHub.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FileHub.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private FileHubContext db = new FileHubContext(ConnectionHelper.connStr);

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hash = GetMd5Hash(model.Password);
                User user = db.Users.FirstOrDefault(x => x.Email == model.Email 
                    && x.PasswordHash == hash);

                if (user != null)
                {
                    var authTicket = new FormsAuthenticationTicket(
                        1,                          // version
                        user.Email,                 // user name
                        DateTime.Now,               // created
                        DateTime.Now.AddDays(1),    // expires
                        true,                       // persistent
                        user.IsAdmin ? "Admin" : string.Empty     // storing roles
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                    return RedirectToAction(nameof(FilesController.FileList), "Files");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(x => x.Email == model.Email);

                if (user == null)
                {
                    var newSavedUser = SaveUser(model);

                    if (newSavedUser != null)
                    {
                        FormsAuthentication.SetAuthCookie(newSavedUser.Email.ToString(), true);
                        return RedirectToAction(nameof(FilesController.FileList), "Files");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Введенный email уже используется");
                }
            }

            return View(model);
        }

        private User SaveUser(RegisterModel model)
        {
            var passworHash = GetMd5Hash(model.Password);
            var newUser = new User
            {
                Email = model.Email,
                PasswordHash = passworHash
            };
           
            db.Users.Add(newUser);
            db.SaveChanges();

            return db.Users
                .Where(u => u.Email == model.Email && u.PasswordHash == passworHash)
                .FirstOrDefault();
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(nameof(Login));
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,SecondName,Patronimic,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,SecondName,Patronimic,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
