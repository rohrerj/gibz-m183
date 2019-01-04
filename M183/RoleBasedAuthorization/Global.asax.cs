using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RoleBasedAuthorization
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string[] Roles { get; private set; }
        public static Dictionary<string, object> UserRoles { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Roles = new string[] { "Administrator", "User" };

            UserRoles = new Dictionary<string, object>();
            UserRoles.Add("admin", "Administrator");
            UserRoles.Add("user", "User");
        }
    }
}
