using Data;
using System.Configuration;
using System.Linq;
using System.Security.Principal;

namespace FileHub.Models
{
    public static class ConnectionHelper
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["FileHubContext"].ConnectionString;
    }

    public static class UserIdExtension
    {
        public static int GetUserId(this FileHubContext db, IPrincipal User)
        {
            return db.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
        }
    }
}