using Google.Authenticator;
using GoogleAuthTOTP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleAuthTOTP.Controllers
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
            if (ModelState.IsValid)
            {
                if (model.Username == "user" && model.Password == "password")
                {
                    TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
                    if(twoFactor.ValidateTwoFactorPIN("secret", model.Token))
                    {
                        ViewBag.Result = "Token valid";
                    }
                    else
                    {
                        ViewBag.Result = "Token invalid";
                    }
                }
                else
                {
                    ViewBag.Title = "Username / password invalid";
                }
            }
            return View(model);
        }


        public ActionResult SetupAuthentication()
        {
            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            SetupCode code = twoFactor.GenerateSetupCode("TestApp", "user", "secret", 300, 300);
            string url = code.QrCodeSetupImageUrl;
            string manualCode = code.ManualEntryKey;

            ViewBag.AuthMessage = "<h2>QR-Code</h2><br/><br/><img src='" + url + "'/><br/><br/><h2>Token for manual entry</h2><br/>" + manualCode;
            return View();
        }
    }
}