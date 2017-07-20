using System.Web.Mvc;
using System.Web.Routing;

namespace BTX.ReportViewer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "GridsDefault",
                url: "{controller}/{action}/{headeroption}",
                defaults: new { controller = "Grids", action = "Index", headeroption = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReportsDefault",
                url: "{controller}/{action}/{headeroption}",
                defaults: new { controller = "Reports", action = "Index", headeroption = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SalesByProducersUrl",
                url: "{controller}/{action}/{period}/{agentid}/{agentname}/{setnames}/{unitsizes}/{pricefrom}/{priceto}/{selectedgroupid}/{headeroption}",
                defaults: new { controller = "Grids", action = "SalesByBrands", period = UrlParameter.Optional, agentid = UrlParameter.Optional, agentname = UrlParameter.Optional, setnames = UrlParameter.Optional, unitsizes = UrlParameter.Optional, pricefrom = UrlParameter.Optional, priceto = UrlParameter.Optional, selectedgroupid = UrlParameter.Optional, headeroption = UrlParameter.Optional },
                constraints: new { agentid = @"\d+" }
            );

            routes.MapRoute(
                name: "SalesByProductsUrl",
                url: "{controller}/{action}/{period}/{agentid}/{agentname}/{setnames}/{brandid}/{brandname}/{unitsizes}/{pricefrom}/{priceto}/{selectedgroupid}/{headeroption}",
                defaults: new { controller = "Grids", action = "SalesByProducts", period = UrlParameter.Optional, agentid = UrlParameter.Optional, agentname = UrlParameter.Optional, setnames = UrlParameter.Optional, brandid = UrlParameter.Optional, brandname = UrlParameter.Optional, unitsizes = UrlParameter.Optional, pricefrom = UrlParameter.Optional, priceto = UrlParameter.Optional, selectedgroupid = UrlParameter.Optional, headeroption = UrlParameter.Optional },
                constraints: new { agentid = @"\d+", brandid = @"\d+" }
            );

            routes.MapRoute(
                "ErrorHandler",
                "Error/NotAuthorized",
                new { controller = "Error", action = "NotAuthorized" });
        }
    }
}