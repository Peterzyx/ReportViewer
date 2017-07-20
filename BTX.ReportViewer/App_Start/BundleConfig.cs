using System.Web.Optimization;

namespace BTX.ReportViewer
{
    public class BundleConfig
    {
        //SOLUTION BUNDLES CONFIGURATION

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery.js",
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/jquery-ui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-touch.js",
                "~/Scripts/ui-grid.js",
                "~/Scripts/ui-bootstrap.js",
                "~/Scripts/ui-bootstrap-tpls.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-sanitize.min.js",
                "~/Scripts/angularjs-dropdown-multiselect.js",
                "~/Scripts/angular-route.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular/Grid").Include(
                "~/Scripts/Grids/myApp.js",
                "~/Scripts/Grids/Top50.js",
                "~/Scripts/Grids/SalesSummaryBySegment.js",
                "~/Scripts/Grids/SalesSummaryColorDetail.js",
                "~/Scripts/Grids/SalesSummaryColorCountry.js",
                "~/Scripts/Grids/SalesSummaryColorCountryProduct.js",
                "~/Scripts/Grids/SalesSummaryCountryColor.js",
                "~/Scripts/Grids/SalesSummaryCountryColorProduct.js",
                "~/Scripts/Grids/SalesSummaryColorProductDetail.js",
                "~/Scripts/Grids/SalesSummaryVarietalProduct.js",
                "~/Scripts/Grids/SalesSummaryMyCategoryCountry.js",
                "~/Scripts/Grids/SalesSummaryMyCategoryCountryProduct.js",
                "~/Scripts/Grids/SalesSummaryPriceBandCategory.js",
                 "~/Scripts/Grids/SalesSummaryPriceBandCategoryProduct.js",
                "~/Scripts/Grids/AgentSales.js",
                "~/Scripts/Grids/BrandSales.js",
                "~/Scripts/Grids/SalesSummary.js",
                "~/Scripts/Grids/SalesSummaryStoreLicensee.js",
                "~/Scripts/Grids/SalesPersonal.js",
                "~/Scripts/Grids/CategoryDetails.js",
                "~/Scripts/Grids/SalesTeam.js",
                "~/Scripts/Grids/Landing.js",
                "~/Scripts/Grids/StoreLicenseeForCSPC.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/toastr").Include(
                "~/Scripts/toastr.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/custom.css",
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-theme.min.css",
                "~/Content/balloon.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/style.default.css",
                "~/Content/animate.css",
                "~/Content/select.min.css",
                "~/Content/ui-grid.min.css",
                "~/Content/toastr.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/angular/Grid/css").Include(
                "~/Content/myApp.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jquery/toastr").Include(
                "~/Content/toastr.min.css"
                ));
        }
    }
}
