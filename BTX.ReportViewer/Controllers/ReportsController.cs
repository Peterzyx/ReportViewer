using BTX.ReportViewer.Helpers;
using BTX.ReportViewer.ViewModels;
using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using BTX.ReportViewer.ReportService;
using BTX.ReportViewer.Logging;
using BTX.ReportViewer.DataLayer;
using System.Data.SqlClient;
using BTX.ReportViewer.ReportExecution;
using System.Net.Http;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace BTX.ReportViewer.Controllers
{
    public class ReportsController : Controller
    {
        [HandleError()]
        [AuthorizeAD]
        [OutputCache(Duration = Int32.MaxValue)]
        public ActionResult Index(int headeroption = 1)
        {
            LoggingHelper.LogInformation("User goes to the report page...");
            var vm = new ReportsModel();
            ReportHelper rptHelper = new ReportHelper();

            if (ReportServer.IsSessionExpired(HttpContext))
            {
                if (headeroption == 1)
                {
                    return RedirectToAction("LogOff", "Account");
                } else
                {
                    var returnUrl = "/Grids?headeroption=2";
                    return RedirectToAction("LogOff", "Account", new { returnUrl = returnUrl });
                }
                
            }

            ReportServer rs = new ReportServer(HttpContext);

            UserPrincipal user = GetCurrentPrincipal();

            if (user != null)
            {
                vm = SetReportViewModel(user);

                try
                {
                    vm.ReportDirectoryHierarchy = rptHelper.CreateReportDirectoryHierarchy(rs.CatalogItems.ToList(), HttpContext);

                }

                catch (ArgumentNullException Ex)
                {
                    LoggingHelper.LogException("No reports found", Ex);

                }
                catch (Exception Ex)
                {
                    LoggingHelper.LogException(Ex);

                }
            }
            else
            {
                throw new Exception("Unable to determine User Principal");

            }
            ViewBag.CurrentUserFirstName = vm.CurrentUserFirstName;
            ViewBag.CurrentUserLastName = vm.CurrentUserSurName;
            ViewBag.HeaderOption = headeroption;
            try
            {
                var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["CompanyName"]);
                using (var stream = new StreamReader(filepath))
                {
                    ViewBag.CompanyName = stream.ReadToEnd();
                }
            }
            catch (Exception exc)
            {
                return Content(exc.Message);
            }
            return View(vm);
        }

        [HandleError()]
        [AuthorizeAD]
        public ActionResult Report(string reportPath, int headeroption = 1)
        {
            var vm = new ReportsModel();

            if (!string.IsNullOrEmpty(reportPath))
            {
                UserPrincipal user = GetCurrentPrincipal();
                vm = SetReportViewModel(user);
                vm.ReportPath = reportPath;
                LoggingHelper.LogInformation("Retrieving Report - " + reportPath);
            }

            // getting report service from cache
            if (ReportServer.IsSessionExpired(HttpContext))
            {
                if (headeroption == 1)
                {
                    return RedirectToAction("LogOff", "Account");
                }
                else
                {
                    var returnUrl = "/Grids?headeroption=2";
                    return RedirectToAction("LogOff", "Account", new { returnUrl = returnUrl });
                }
            }
            //ReportServer rs = new ReportServer(HttpContext);

            //getting report parameters
            ItemParameter[] parameters = ReportServer.GetItemParameters(HttpContext, reportPath, null, true, null, null);
            
            ViewBag.CurrentUserFirstName = vm.CurrentUserFirstName;
            ViewBag.CurrentUserLastName = vm.CurrentUserSurName;
            ViewBag.HeaderOption = headeroption;
            ViewBag.ReportParameters = parameters;
            ViewBag.SamAccount = vm.SamAccount;
            ViewBag.Password = LoginUser.Password;
            return View(vm);
        }

        private static Stream GetResourceFileStream(string fileName)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            // Get all embedded resources
            var arrResources = currentAssembly.GetManifestResourceNames();
  
            return (from resourceName in arrResources where resourceName.Contains(fileName) select currentAssembly.GetManifestResourceStream(resourceName)).FirstOrDefault();
        }

        [HandleError()]
        [AuthorizeAD]
        public ActionResult GenerateExcelReport(ReportExecutionBE reportexecution)
        {
            try
            {
                LoggingHelper.LogInformation("Retrieving parameters...");
                if (reportexecution == null)
                {
                    return new EmptyResult();
                }

                Byte[] results = ReportServer.DownloadExcel(HttpContext, reportexecution);
                var fileName = string.Format("{0}.xls", reportexecution.ReportName);
                
                if (results != null && results.Length > 0)
                {
                    Session[fileName] = results;
                    return Json(new { success = true, fileName }, JsonRequestBehavior.AllowGet);
                } else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to generate excel bytes from report service.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogException("Unable to generate excel bytes from report service.", ex);
                throw;
            }
        }

        [HandleError()]
        [AuthorizeAD]
        public ActionResult DownloadExcelReport(string fileName)
        {
            var ms = Session[fileName] as Byte[];
            if(ms == null)
                return new EmptyResult();
            Session[fileName] = null;
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HandleError]
        [AuthorizeAD]
        public ActionResult GetDependentParameters(ReportExecutionBE reportexecution)
        {
            try
            {
                string reportPath = reportexecution.ReportPath;
                List<ReportService.ParameterValue> values = new List<ReportService.ParameterValue>();

                for (int i = 0; i < reportexecution.Parameters.Count; ++i)
                {
                    ReportParameterBE valueList = reportexecution.Parameters[i];

                    if (valueList.Value != null)
                    {
                        for (int j = 0; j < valueList.Value.Length; ++j)
                        {
                            ReportService.ParameterValue value = new ReportService.ParameterValue();
                            value.Name = valueList.Name;
                            value.Label = valueList.Label;
                            value.Value = valueList.Value[j];
                            values.Add(value);
                        }
                    }
                }
                ItemParameter[] output = ReportServer.GetItemParameters(HttpContext, reportPath, null, true, values.ToArray(), null);
                //var abcObj = new ReportParameterBE { Name = "test", Label = "test", Value = null };

                //return Json(abcObj);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to generate dependent parameters from report service.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogException("Unable to generate dependent parameters from report service.", ex);
                throw;
            }
            
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            LoggingHelper.LogException(filterContext.Exception);
           // //WriteLog(Settings.LogErrorFile, filterContext.Exception.ToString());
        }
        private UserPrincipal GetCurrentPrincipal()
        {
            ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            var ci = claimsPrincipal.Identity as ClaimsIdentity;

            foreach (var claim in ci.Claims)
            {

                if (claim.Type == @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                {
                    PrincipalContext domain = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["ReportsDomain"]);
                    UserPrincipal user = UserPrincipal.FindByIdentity(domain, claim.Value);
                    return user;
                }
            }

            return null;
        }

        private ReportsModel SetReportViewModel(UserPrincipal user)
        {
            ReportsModel vm = new ReportsModel();
            vm.CurrentUser = user.UserPrincipalName;
            vm.SamAccount = user.SamAccountName;
            vm.CurrentUserFirstName = user.GivenName;
            vm.CurrentUserSurName = user.Surname;
            return vm;
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }

    }
}
