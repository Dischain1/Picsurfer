using Data;
using FileHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace FileHub
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest (object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    string email = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                    List<string> roles = new List<string>();

                    using (FileHubContext db = new FileHubContext(ConnectionHelper.connStr))
                    {
                        var user = db.Users.Where(u => u.Email == email).FirstOrDefault();

                        if (user == null)
                            return;

                        if (user.IsAdmin)
                            roles.Add("Admin");
                    }

                    // retrieve roles from UserData
                    HttpContext.Current.User = new GenericPrincipal(
                        new GenericIdentity(email, "Forms"), roles.ToArray());
                }
            }
        }
    }
}
