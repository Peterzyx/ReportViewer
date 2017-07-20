using System.Web.Mvc;

namespace BTX.ReportViewer.Controllers
{
    public class ErrorController : Controller
    {
        //Generic error handling
        public ViewResult Index()
        {
            return View("Error");
        }

        //Not found error page

        public ActionResult NotFound()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;

            if (!Request.IsAjaxRequest())
                result = View(model);
            else
                result = PartialView("_NotFound", model);

            return result;
        }

        //Not authorized error page
        [AllowAnonymous]
        public ViewResult NotAuthorized()
        {
            return View("NotAuthorized");
        }

        //Forbidden error page
        [AllowAnonymous]
        public ViewResult Forbidden()
        {
            return View("Forbidden");
        }
    }
}