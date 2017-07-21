using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTX.ReportViewer.DataLayer
{
    public interface IGridsRepository
    {
        List<MarketReportBE> GetTop50SalesCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto);

        List<MarketReportBE> GetSalesByAgentsCube(string periodkey, string agentname, string setnames, string unitsizes, decimal pricefrom, decimal priceto);

        List<MarketReportBE> GetSalesByProductsCube(string periodkey, string agentname, string brandname, string setnames, string unitsizes, decimal pricefrom, decimal priceto);

        List<SalesTeamReportBE> GetSalesTeamSummaryAllCube(string periodkey);

        List<SalesTeamReportBE> GetSalesTeamSummaryStoreCube(string periodkey);

        List<SalesTeamReportBE> GetSalesTeamSummaryLicenseeCube(string periodkey);

        List<SalesTeamReportBE> GetSalesSummaryStoreByTerritoryCube(int userid, string periodkey, int sortgroup);

        List<SalesTeamReportBE> GetSalesSummaryLicenseeByTerritoryCube(int userid, string periodkey, int sortgroup);

        List<SalesTeamReportBE> GetSalesSummaryAllByTerritoryCube(int userid, string periodkey, int sortgroup);

        List<SalesPersonalReportBE> GetSalesTeamStoreProductCube(int userid, string periodkey, string accountnumber, int clientonly);

        List<SalesPersonalReportBE> GetSalesTeamStoreCategoryCube(int userid, string periodkey, string accountnumber, int clientonly);

        List<SalesPersonalReportBE> GetSalesTeamProductsNotInStoreCube(int userid, string periodkey, string accountnumber);

        List<SalesPersonalReportBE> GetSalesCategoryDetails(string periodkey, string accountnumber, string category, int isclientonly);

        List<LicenseeDetailsBE> GetLicenseeDetails(string periodkey, string accountname);

        List<SearchBE> GetSearch();

        List<LicSearchBE> GetLicSearch();

        List<CSPCSearchBE> GetCSPCSearch();

        List<CSPCStoreLicenseeBE> GetCSPCStoreCube(string periodkey, string cspc, int groupid);

        List<CSPCStoreLicenseeBE> GetCSPCLicenseeCube(string periodkey, string cspc, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByColorCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByCountryCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByVarietalCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByColorCountryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string color, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByColorCountryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string color, string country, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByCountryColorDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string country, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByCountryColorProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string country, string color, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByVarietalProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string varietal, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCountryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string colormycategory, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCountryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string color, string country, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid);

        List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandCategoryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string priceband, string category);

        List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandMyCategoryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string priceband, string mycategory, string category);

    }
}
