using EmailAndSmsOTP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailAndSmsOTP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
            if(ModelState.IsValid)
            {
                if (model.Username == "user" && model.Password == "password")
                {
                    string otp = "secret";

                    //here prepare and send request to sms or email endpoint and wait for response

                    //if response is successful redirect to ValidateToken Action
                    return RedirectToAction("ValidateToken");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ValidateToken()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ValidateToken(TokenViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Token == "secret")
                {
                    //Login successful
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}