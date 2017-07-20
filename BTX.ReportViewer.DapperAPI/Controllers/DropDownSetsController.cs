using BTX.ReportViewer.DataLayer;
using BTX.ReportViewer.DataLayer.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTX.ReportViewer.Logging;

namespace BTX.ReportViewer.DapperAPI.Controllers
{
    public class DropDownSetsController : ApiController
    {
        private IDropDownSetsRepository repository = new DropDownSetsRepository();

        #region Set Names
        [Route("SetNames")]
        [HttpPost]
        public List<DropDownSetBE> GetAllSetNames()
        {
            try
            {
                LoggingHelper.LogInformation("Retrieving setnames...");
                return repository.GetAllSetNames();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get setnames from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get setnames from database", ex);
                throw;
            }
        }
        #endregion

        #region UnitSizes
        [Route("UnitSizeMLs")]
        [HttpPost]
        public List<DropDownSetBE> GetAllUnitSizeMLs()
        {
            IEnumerable<string> headerValues;
            string periodkey = string.Empty;
            string setnames = string.Empty;

            if (Request.Headers.TryGetValues("X-PeriodKey", out headerValues))
            {
                periodkey = headerValues.FirstOrDefault();
            }

            if (Request.Headers.TryGetValues("X-SetNames", out headerValues))
            {
                setnames = headerValues.FirstOrDefault();
            }

            try
            {
                LoggingHelper.LogInformation(String.Format("Retrieving unitsizes with period = {0} and setnames = {1}...", periodkey, setnames));
                return repository.GetAllUnitSizeMLs(periodkey, setnames);
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get unitsizes from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get unitsizes from database.", ex);
                throw;
            }
        }
        #endregion

        #region Periods
        [Route("PeriodKeys")]
        [HttpPost]
        public List<DropDownSetBE> GetAllPeriodKeys()
        {
            try
            {
                LoggingHelper.LogInformation("Retrieving periods...");
                return repository.GetAllPeriodKeys();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get periods from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get periods from database.", ex);
                throw;
            }
        }
        #endregion

        #region My Categories
        [Route("MyCategories")]
        [HttpPost]
        public List<DropDownSetBE> GetMyCategories()
        {
            try
            {
                LoggingHelper.LogInformation("Retrieving categories...");
                return repository.GetMyCategories();
            }
            catch (SqlException ex)
            {
                LoggingHelper.LogException("Unable to connect to get categories from database.", ex);
                throw;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("Unable to get categories from database.", ex);
                throw;
            }
        }
        #endregion
    }
}