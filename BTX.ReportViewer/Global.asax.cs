using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BTX.ReportViewer.Controllers;

namespace BTX.ReportViewer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Enabling Iframe
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    if (Response.StatusCode == 401)
        //    {
        //        Response.ClearContent();
        //        Response.RedirectToRoute("ErrorHandler", (RouteTable.Routes["ErrorHandler"] as Route).Defaults);
        //    }
        //}

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(this.Context);

            // If we're an ajax request and forms authentication caused a 302, 
            // then we actually need to do a 401
            if (FormsAuthentication.IsEnabled && context.Response.StatusCode == 302
                && context.Request.IsAjaxRequest())
            {
                context.Response.Clear();
                context.Response.StatusCode = 401;
            }

            if (context.Response.StatusCode == 404)
            {
                context.Response.Clear();

                var rd = new RouteData();
                rd.DataTokens["area"] = "AreaName"; // In case controller is in another area
                rd.Values["controller"] = "Errors";
                rd.Values["action"] = "NotFound";

                IController c = new ErrorController();
                c.Execute(new RequestContext(context, rd));
            }
        }
    }
}