using System.Web.Mvc;
using BTX.ReportViewer.ViewModels;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Threading;
using System.Configuration;
using System.IO;
using System;
using System.Reflection;

namespace BTX.ReportViewer.Controllers
{
    public class GridsController : Controller
    {
        public ActionResult Index(int headeroption = 1)
        {
            var vm = new GridsModel();
            UserPrincipal user = GetCurrentPrincipal();
            vm = SetReportViewModel(user);
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

        [HttpPost]
        [Route("Grids/SalesByBrands/{headeroption?}")]
        public ActionResult SalesByBrands(string period, int agentid, string agentname, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, int headeroption = 1)
        {
            var vm = new GridsModel(period, agentid, agentname, setnames, unitsizes, pricefrom, priceto, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesByProducts/{headeroption?}")]
        public ActionResult SalesByProducts(string period, int agentid, string agentname, string setnames, int brandid, string brandname, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, int headeroption = 1)
        {
            var vm = new GridsModel(period, agentid, agentname, setnames, brandid, brandname, unitsizes, pricefrom, priceto, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesTeamStoreLicensee/{headeroption?}")]
        public ActionResult SalesTeamStoreLicensee(int userid, string salesname, string period, int selectedgroupid, string storelicensee, int headeroption = 1)
        {
            var vm = new GridsModel(userid, salesname, period, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            ViewBag.StoreLicensee = storelicensee;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/StoreLicenseeForProduct/{headeroption?}")]
        public ActionResult StoreLicenseeForCSPC(string cspc, string productname, string period, int selectedgroupid, int headeroption = 1)
        {
            var vm = new GridsModel(productname, cspc, period, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesPersonalStore/{headeroption?}")]
        public ActionResult SalesPersonalStore(int userid, string period, string accountnumber, string salesname, string accountname, int selectedgroupid, int headeroption = 1)
        {
            var vm = new GridsModel(userid, period, accountnumber, salesname, accountname, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/CategoryDetails/{headeroption?}")]
        public ActionResult CategoryDetails(string category, string accountnumber, string accountname, string period, int isclientonly, int headeroption = 1)
        {
            var vm = new GridsModel(category, period, isclientonly, accountnumber, accountname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesPersonalLicensee/{headeroption?}")]
        public ActionResult SalesPersonalLicensee(int userid, string period, string accountnumber, string salesname, string accountname, int selectedgroupid, int headeroption = 1)
        {
            var vm = new GridsModel(userid, period, accountnumber, salesname, accountname, selectedgroupid);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }


        [HttpPost]
        [Route("Grids/SalesSummaryColorCountry/{headeroption?}")]
        public ActionResult SalesSummaryColorCountry(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string colorname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 1, colorname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryColorCountryProduct/{headeroption?}")]
        public ActionResult SalesSummaryColorCountryProduct(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string colorname, string countryname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 1, colorname, countryname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryCountryColor/{headeroption?}")]
        public ActionResult SalesSummaryCountryColor(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string countryname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 2, countryname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryCountryColorProduct/{headeroption?}")]
        public ActionResult SalesSummaryCountryColorProduct(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string countryname, string colorname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 2, countryname, colorname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryColorProductDetail/{headeroption?}")]
        public ActionResult SalesSummaryColorProductDetail(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string colorname, string category, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, colorname,  category);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryVarietalProduct/{headeroption?}")]
        public ActionResult SalesSummaryVarietalProduct(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string varietalname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 3, varietalname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryMyCategoryCountry/{headeroption?}")]
        public ActionResult SalesSummaryMyCategoryCountry(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string mycategoryname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 4, mycategoryname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryMyCategoryCountryProduct/{headeroption?}")]
        public ActionResult SalesSummaryMyCategoryCountryProduct(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string mycategoryname, string countryname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 4, mycategoryname, countryname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryPriceBandCategory/{headeroption?}")]
        public ActionResult SalesSummaryPriceBandCategory(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string pricebandname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 5, pricebandname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
        }

        [HttpPost]
        [Route("Grids/SalesSummaryPriceBandCategoryProduct/{headeroption?}")]
        public ActionResult SalesSummaryPriceBandCategoryProduct(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string pricebandname, string mycategoryname, int headeroption = 1)
        {
            var vm = new GridsModel(period, setnames, unitsizes, pricefrom, priceto, selectedgroupid, 5, pricebandname, mycategoryname);
            ViewBag.HeaderOption = headeroption;
            return View(vm);
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

        private GridsModel SetReportViewModel(UserPrincipal user)
        {
            GridsModel vm = new GridsModel();
            vm.CurrentUser = user.UserPrincipalName;
            vm.SamAccount = user.SamAccountName;
            vm.CurrentUserFirstName = user.GivenName;
            vm.CurrentUserSurName = user.Surname;
            return vm;
        }
    }
}