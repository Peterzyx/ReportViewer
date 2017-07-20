using BTX.ReportViewer.Models;
using BTX.ReportViewer.ViewModels;
using BTX.ReportViewer.Logging;
using BTX.ReportViewer.ReportService;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace BTX.ReportViewer.Controllers
{
    public class AccountController : Controller
    {
        private string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsDomain"];
            }
        }

        //Action for non-authorized users
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LoggingHelper.LogInformation("User signing in...");
            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                LoginUser.UserName = model.UserName;
                LoginUser.Password = model.Password;
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                ReportingService2010 reportingSvc = new ReportingService2010();
                reportingSvc.Credentials = new System.Net.NetworkCredential(model.UserName, model.Password, Domain);
                HttpContext.Session.Add(Helpers.ReportServer.REPORTSERVERNAME, reportingSvc);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    LoggingHelper.LogInformation("User redirecting...");
                    return Redirect(returnUrl);
                }
                LoggingHelper.LogInformation("User goes to the main page...");
                return RedirectToAction("Index", "Grids");
            }

            LoginUser.UserName = string.Empty;
            LoginUser.Password = string.Empty;
            ModelState.AddModelError(string.Empty, @"The user name or password is incorrect");
            return View(model);
        }

        //Log-off
        public ActionResult LogOff(string returnUrl)
        {
            LoginUser.UserName = string.Empty;
            LoginUser.Password = string.Empty;
            LoggingHelper.LogInformation("User logging off...");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { returnUrl = "/" + returnUrl });
        }

        [HttpPost]
        public ActionResult ExpirationCheck()
        {
            var auth = HttpContext.User.Identity.IsAuthenticated;

            if (auth)
            {
                LoggingHelper.LogInformation("User is authorized...");
                return Json(new { success = true, responseText = "test for success" });
            }
            LoginUser.UserName = string.Empty;
            LoginUser.Password = string.Empty;
            LoggingHelper.LogInformation("User is not authorized...");
            return Json(new { success = false, responseText = "test for false" });
        }
    }
}
