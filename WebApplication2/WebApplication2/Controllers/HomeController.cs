using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Web.Mvc;
using WebApplication2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        quanlydogoEntities db = new quanlydogoEntities();
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string UserName = HttpContext.User.Identity.Name;
                var objectUser = (from u in db.Users where u.UserName == UserName select u).FirstOrDefault();
                
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Login", "Home");
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
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string UserName, string Password)
        {
            var oneUser = (from t in db.Users where t.UserName == UserName select t).FirstOrDefault();
            if (oneUser == null)
            {
                ViewBag.Message = "Account not found";
                return View();
            }
            if (oneUser.Password == Password)
            {
                var ident = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, oneUser.UserName),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider","ASP.NET Identity","http://www.w3.org/XMLSchema#string"),
                    new Claim(ClaimTypes.Name,oneUser.UserName),
                    new Claim(ClaimTypes.Email,oneUser.Name),
                    new Claim(ClaimTypes.Role,oneUser.Role)

                }, "ApplicationCookie");

                //var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();


                HttpContext.GetOwinContext().Authentication.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = false }, ident);
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}