using SelfMadeSessions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SelfMadeSessions.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string sessionKey = Request.Cookies["M183-Session-Cookie"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey) && _Session.Exists(sessionKey))
            {
                ViewBag.Username = _Session.GetSession(sessionKey)["username"];
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "user" && model.Password == "password")
                {
                    string key = _Session.CreateSession();
                    _Session.GetSession(key).Add("username", "user");
                    Response.Cookies.Add(_Session.GetCoookie(key));
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            string sessionKey = Request.Cookies["M183-Session-Cookie"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey) && _Session.Exists(sessionKey))
            {
                _Session.DeleteSession(sessionKey);
            }
            return RedirectToAction("Login");
        }
    }
}