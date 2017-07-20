using BTX.ReportViewer.DataLayer;
using BTX.ReportViewer.DataLayer.Dapper;
using BTX.ReportViewer.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTX.ReportViewer.DapperAPI.Controllers
{
    public class GridsController : ApiController
    {
        private IGridsRepository repository = new GridsRepository();

        #region Top 50 Sales
        [Route("Top50Sales")]
        [HttpGet]
        public List<MarketReportBE> GetTop50Sales()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving top 50 sales with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}...", periodkey, setnames, unitsizes, pricefrom, priceto));
                return repository.GetTop50SalesCube(periodkey, setnames, unitsizes, pricefrom, priceto);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get top 50 sales from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get top 50 sales.", ex);
                throw;
            }
        }
        #endregion

        #region Sales by Brands
        [Route("AgentSales")]
        [HttpGet]
        public List<MarketReportBE> GetSalesByBrands()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string agentname = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-AgentName", out headerValues))
            {
                agentname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving agent sales with agent = {0}...", agentname));
                return repository.GetSalesByAgentsCube(periodkey, agentname, setnames, unitsizes, pricefrom, priceto);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by brands from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by brands from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales by Products
        [Route("BrandSales")]
        [HttpGet]
        public List<MarketReportBE> GetSalesByProducts()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string agentname = string.Empty;
            string brandname = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-AgentName", out headerValues))
            {
                agentname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-BrandName", out headerValues))
            {
                brandname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving product sales with agent = {0} and brand = {1}...", agentname, brandname));
                return repository.GetSalesByProductsCube(periodkey, agentname, brandname, setnames, unitsizes, pricefrom, priceto);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for All Accounts
        [Route("SalesTeamSummaryAll")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesTeamSummaryAll()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store sales summary with period = {0}...", periodkey));
                return repository.GetSalesTeamSummaryAllCube(periodkey);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales team summary for store(s) from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales team summary for stores(s) from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for Store
        [Route("SalesTeamSummaryStore")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesTeamSummaryStore()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store sales summary with period = {0}...", periodkey));
                return repository.GetSalesTeamSummaryStoreCube(periodkey);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales team summary for store(s) from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales team summary for stores(s) from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for Licensee
        [Route("SalesTeamSummaryLicensee")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesTeamSummaryLicensee()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving licensee sales summary with period = {0}...", periodkey));
                return repository.GetSalesTeamSummaryLicenseeCube(periodkey);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales team summary for licensee(s) from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales team summary for licensee(s) from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for Store by Territory
        [Route("SalesSummaryStoreByTerritory")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesSummaryStoreByTerritory()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            int sortgroup = 0;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-SortGroup", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out sortgroup);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store sales details with period = {0} and user id = {1}...", periodkey, userid));
                return repository.GetSalesSummaryStoreByTerritoryCube(userid, periodkey, sortgroup);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get store sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get store sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for All Accounts by Territory
        [Route("SalesSummaryAllByTerritory")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesSummaryAllByTerritory()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            int sortgroup = 0;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-SortGroup", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out sortgroup);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving all sales details with period = {0} and user id = {1}...", periodkey, userid));
                return repository.GetSalesSummaryAllByTerritoryCube(userid, periodkey, sortgroup);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get all sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get all sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary for Licensee by Territory
        [Route("SalesSummaryLicenseeByTerritory")]
        [HttpGet]
        public List<SalesTeamReportBE> GetSalesSummaryLicenseeByTerritory()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            int sortgroup = 0;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-SortGroup", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out sortgroup);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving licensee sales details with period = {0} and user id = {1}...", periodkey, userid));
                return repository.GetSalesSummaryLicenseeByTerritoryCube(userid, periodkey, sortgroup);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get licensee sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get licensee sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Products Sales for Store
        [Route("SalesTeamStoreProduct")]
        [HttpGet]
        public List<SalesPersonalReportBE> GetSalesTeamStoreProduct()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            string accountnumber = string.Empty;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-AccountNumber", out headerValues))
            {
                accountnumber = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store product sales with period = {0}, user id = {1} and account = {2}...", periodkey, userid, accountnumber));
                return repository.GetSalesTeamStoreProductCube(userid, periodkey, accountnumber);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get store product sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get store product sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Category Sales for Store
        [Route("SalesTeamStoreCategory")]
        [HttpGet]
        public List<SalesPersonalReportBE> GetSalesTeamStoreCategory()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            string accountnumber = string.Empty;
            int clientonly = 0;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-AccountNumber", out headerValues))
            {
                accountnumber = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-ClientOnly", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out clientonly);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store category sales with period = {0}, user id = {1} and account = {2}...", periodkey, userid, accountnumber));
                return repository.GetSalesTeamStoreCategoryCube(userid, periodkey, accountnumber, clientonly);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get store category sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get store category sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Product Sales Not In Store
        [Route("SalesTeamProductsNotInStore")]
        [HttpGet]
        public List<SalesPersonalReportBE> GetSalesTeamProductsNotInStore()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            string accountnumber = string.Empty;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-AccountNumber", out headerValues))
            {
                accountnumber = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving sales of products not in store with period = {0}, user id = {1} and account = {2}...", periodkey, userid, accountnumber));
                return repository.GetSalesTeamProductsNotInStoreCube(userid, periodkey, accountnumber);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get sales of products not in store for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get sales of products not in store for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Sales Category Details for Store
        [Route("SalesCategoryDetails")]
        [HttpGet]
        public List<SalesPersonalReportBE> GetSalesCateogryDetails()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string category = string.Empty;
            int clientonly = 0;
            string accountnumber = string.Empty;
            
            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-ClientOnly", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out clientonly);
            }

            if (Request.Headers.TryGetValues("X-AccountNumber", out headerValues))
            {
                accountnumber = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving store category sales details with period = {0}, category = {1}, accountnumber = {2} and clientonly = {3}...", periodkey, category, accountnumber, clientonly));
                return repository.GetSalesCategoryDetails(periodkey, accountnumber, category, clientonly);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get store category sales details for category = {0} and accountnumber = {1} from database.", category, accountnumber), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get store category sales details for category = {0} from database.", category), ex);
                throw;
            }
        }
        #endregion

        #region Licensee Details
        [Route("GetLicenseeDetails")]
        [HttpGet]
        public List<LicenseeDetailsBE> GetLicenseeDetails()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string accountname = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-AccountName", out headerValues))
            {
                accountname = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving sales of products not in licensee with period = {0}, account = {1}...", periodkey, accountname));
                return repository.GetLicenseeDetails(periodkey, accountname);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get sales of products not in licensee for account = {0} from database.", accountname), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to get sales of products not in licensee for account = {0} from database.", accountname), ex);
                throw;
            }
        }
        #endregion

        #region Market Sales
        [Route("MarketSalesBySetCode")]
        [HttpGet]
        public List<MarketReportBE> GetMarketSalesBySetCode()
        {
            IEnumerable<string> headerValues;
            int userid = 0;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;

            if (Request.Headers.TryGetValues("X-UserId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out userid);
            }

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving market sales with period = {0}, user id = {1} and setnames = {2}...", periodkey, userid, setnames));
                return repository.GetMarketSalesBySetCode(userid, periodkey, setnames, unitsizes, pricefrom, priceto);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException(String.Format("Unable to connect to get market sales for user id = {0} from database.", userid), ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(String.Format("Unable to connect to get market sales for user id = {0} from database.", userid), ex);
                throw;
            }
        }
        #endregion

        #region Search
        [Route("Search")]
        [HttpGet]
        public List<SearchBE> GetSearch()
        {
            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Search..."));
                return repository.GetSearch();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to db.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get data.", ex);
                throw;
            }
        }
        #endregion

        #region LicSearch
        [Route("LicSearch")]
        [HttpGet]
        public List<LicSearchBE> LicGetSearch()
        {
            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Search..."));
                return repository.GetLicSearch();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to db.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get data.", ex);
                throw;
            }
        }
        #endregion

        #region CSPCSearch
        [Route("CSPCSearch")]
        [HttpGet]
        public List<CSPCSearchBE> GetCSPCSearch()
        {
            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Search..."));
                return repository.GetCSPCSearch();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to db.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get data.", ex);
                throw;
            }
        }
        #endregion

        #region CSPCSearchStore
        [Route("CSPCSearchStore")]
        [HttpGet]
        public List<CSPCStoreLicenseeBE> GetCSPCSearchStore()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string cspc = string.Empty;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-CSPC", out headerValues))
            {
                cspc = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-GroupId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Search..."));
                return repository.GetCSPCStoreCube(periodkey, cspc, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to db.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get data.", ex);
                throw;
            }
        }
        #endregion

        #region CSPCSearchLicensee
        [Route("CSPCSearchLicensee")]
        [HttpGet]
        public List<CSPCStoreLicenseeBE> GetCSPCSearchLicensee()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string cspc = string.Empty;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-CSPC", out headerValues))
            {
                cspc = headerValues.FirstOrDefault();
            }
            if (Request.Headers.TryGetValues("X-GroupId", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Search..."));
                return repository.GetCSPCLicenseeCube(periodkey, cspc, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to db.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get data.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary by Color
        [Route("SalesSummaryByColor")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryByColor()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Sales Summary by Color with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}, groupid = {5}...", periodkey, setnames, unitsizes, pricefrom, priceto, groupid));
                return repository.GetSalesSummaryByColorCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get Sales Summary by Country from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get Sales Summary by Country.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary by Country
        [Route("SalesSummaryByCountry")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryByCountry()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Sales Summary by Color with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}, groupid = {5}...", periodkey, setnames, unitsizes, pricefrom, priceto, groupid));
                return repository.GetSalesSummaryByCountryCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get Sales Summary by Country from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get Sales Summary by Country.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary by Varietal
        [Route("SalesSummaryByVarietal")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryByVarietal()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Sales Summary by Varietal with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}, groupid = {5}...", periodkey, setnames, unitsizes, pricefrom, priceto, groupid));
                return repository.GetSalesSummaryByVarietalCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get Sales Summary by Varietal from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get Sales Summary by Varietal.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary by My Category
        [Route("SalesSummaryByMyCategory")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryByMyCategory()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Sales Summary by My category with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}, groupid = {5}...", periodkey, setnames, unitsizes, pricefrom, priceto, groupid));
                return repository.GetSalesSummaryByMyCategoryCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get Sales Summary by My Category from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get Sales Summary by My Category.", ex);
                throw;
            }
        }
        #endregion

        #region Sales Summary by Price Band
        [Route("SalesSummaryByPriceBand")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryByPriceBand()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            int groupid = 0;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Sales Summary by price band with period = {0}, setnames = {1}, unitsizes = {2}, price from {3} to {4}, groupid = {5}...", periodkey, setnames, unitsizes, pricefrom, priceto, groupid));
                return repository.GetSalesSummaryByPriceBandCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get Sales Summary by price band from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get Sales Summary by price band.", ex);
                throw;
            }
        }
        #endregion
        
        #region Sales summary by Color Country Detail
        [Route("SalesSummaryColorCountry")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryColorCountryDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string colorname = string.Empty;
            int groupid = 0;
            string category = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-ColorName", out headerValues))
            {
                colorname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving country sales with color = {0}...", colorname));
                return repository.GetSalesSummaryByColorCountryDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, colorname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Color Country Product Detail
        [Route("SalesSummaryColorCountryProduct")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryColorCountryProductDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string colorname = string.Empty;
            int groupid = 0;
            string category = string.Empty;
            string countryname = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-ColorName", out headerValues))
            {
                colorname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-CountryName", out headerValues))
            {
                countryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving product sales with color = {0}, country = {1}...", colorname, countryname));
                return repository.GetSalesSummaryByColorCountryProductDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, colorname, countryname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Country Color Detail
        [Route("SalesSummaryCountryColor")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryCountryColorDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string countryname = string.Empty;
            int groupid = 0;
            string category = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-CountryName", out headerValues))
            {
                countryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving country sales with country = {0}...", countryname));
                return repository.GetSalesSummaryByCountryColorDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, countryname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Color Country Product Detail
        [Route("SalesSummaryCountryColorProduct")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryCountryColorProductDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string colorname = string.Empty;
            int groupid = 0;
            string category = string.Empty;
            string countryname = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-ColorName", out headerValues))
            {
                colorname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-CountryName", out headerValues))
            {
                countryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving product sales with color = {0}, country = {1}...", colorname, countryname));
                return repository.GetSalesSummaryByCountryColorProductDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, countryname, colorname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Varietal Product Detail
        [Route("SalesSummaryVarietalProduct")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryVarietalProductDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string colorname = string.Empty;
            int groupid = 0;
            string category = string.Empty;
            string varietal = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-VarietalName", out headerValues))
            {
                varietal = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving products sales with varietal = {0}...", varietal));
                return repository.GetSalesSummaryByVarietalProductDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, varietal, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by My Category Country Detail
        [Route("SalesSummaryMyCategoryCountry")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryMyCategoryCountryDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string mycategoryname = string.Empty;
            int groupid = 0;
            string category = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-MyCategoryName", out headerValues))
            {
                mycategoryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving country sales with my category = {0}...", mycategoryname));
                return repository.GetSalesSummaryByMyCategoryCountryDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, mycategoryname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by my category from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by my category from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by My Category Country product Detail
        [Route("SalesSummaryMyCategoryCountryProduct")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryMyCategoryCountryProductDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string mycategoryname = string.Empty;
            int groupid = 0;
            string category = string.Empty;
            string countryname = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-MyCategoryName", out headerValues))
            {
                mycategoryname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-CountryName", out headerValues))
            {
                countryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving product sales with my category = {0}, country = {1}...", mycategoryname, countryname));
                return repository.GetSalesSummaryByMyCategoryCountryProductDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, mycategoryname, countryname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Price Band Category Detail
        [Route("SalesSummaryPriceBandCategory")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryPriceBandCategoryDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string pricebandname = string.Empty;
            int groupid = 0;
            string category = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-PriceBandName", out headerValues))
            {
                pricebandname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving Category sales with price band = {0}...", pricebandname));
                return repository.GetSalesSummaryByPriceBandCategoryDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, pricebandname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by category from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by category from database.", ex);
                throw;
            }
        }
        #endregion

        #region Sales summary by Priceband Mycategory Product Detail
        [Route("SalesSummaryPriceBandCategoryProduct")]
        [HttpGet]
        public List<SalesSummarySegmentBE> GetSalesSummaryPriceBandMyCategoryProductDetail()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;

            string setnames = string.Empty;
            string unitsizes = string.Empty;
            decimal pricefrom = 0m;
            decimal priceto = 0m;
            string pricebandname = string.Empty;
            int groupid = 0;
            string category = string.Empty;
            string mycategoryname = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-UnitSizes", out headerValues))
            {
                unitsizes = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-PriceFrom", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out pricefrom);
            }

            if (Request.Headers.TryGetValues("X-PriceTo", out headerValues))
            {
                Decimal.TryParse(headerValues.FirstOrDefault(), out priceto);
            }

            if (Request.Headers.TryGetValues("X-Group", out headerValues))
            {
                int.TryParse(headerValues.FirstOrDefault(), out groupid);
            }

            if (Request.Headers.TryGetValues("X-PriceBandName", out headerValues))
            {
                pricebandname = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-MyCategoryName", out headerValues))
            {
                mycategoryname = headerValues.FirstOrDefault();
            }


            if (Request.Headers.TryGetValues("X-Category", out headerValues))
            {
                category = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving product sales with priceband = {0}, mycategory = {1}...", pricebandname, mycategoryname));
                return repository.GetSalesSummaryByPriceBandMyCategoryProductDetailCube(periodkey, setnames, unitsizes, pricefrom, priceto, groupid, pricebandname, mycategoryname, category);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get sales by products from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get sales by products from database.", ex);
                throw;
            }
        }
        #endregion
    }
}