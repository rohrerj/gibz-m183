using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBasedAuthorization.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            if(CheckAccess())
            {
                return View();
            }
            else
            {
                Session["username"] = null;
                return RedirectToAction("Login", "Home");
            }
        }
        private bool CheckAccess()
        {
            object username = Session["username"];
            string name;
            if(username != null)
            {
                name = (string)username;
                object role = MvcApplication.UserRoles[name];
                return role.ToString() == "Administrator";
            }
            return false;
        }
    }
}