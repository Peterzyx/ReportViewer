using BTX.ReportViewer.ReportExecution;
using BTX.ReportViewer.ReportService;
using BTX.ReportViewer.Logging;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;
using BTX.ReportViewer.DataLayer;
using System.IO;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Collections.Generic;

namespace BTX.ReportViewer.Helpers
{
    public class ReportServer
    {
        public static readonly string REPORTSERVERNAME = "ReportServer";
        #region Private Members
        private CatalogItem[] _catalogItem;
        #endregion

        #region Members
        public string URL
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsURL"];
            }
        }

        public string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsDomain"];
            }
        }

        public string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsUserName"];
            }
        }

        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsPassword"];
            }
        }

        public string DefaultReport
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsDefaultReportPath"];
            }
        }

        public string FolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsFolderPath"];
            }
        }

        public CatalogItem[] CatalogItems
        {

            set
            {
                _catalogItem = value;
            }
            get
            {
                return _catalogItem;
            }
        }
        #endregion

        #region Constructor
        public ReportServer(System.Web.HttpContextBase httpContext)
        {
            var rs = ReportWebServiceInstance(httpContext); // as ReportingService2010;
            try
            {
                if (rs == null)
                {
                    throw new Exception("Unable to connect to server");
                }

                this.CatalogItems = rs.ListChildren(FolderPath, true);
                if (this.CatalogItems.Count() > 0)
                    LoggingHelper.LogInformation("Items returned:" + this.CatalogItems.Count().ToString() + "at " + DateTime.Now.ToString());
                else
                    LoggingHelper.LogInformation("No Items Found");
            }
            catch (SoapException Ex)
            {
                LoggingHelper.LogException("Unable to connect to Web Service.", Ex);

                //Log Error
            }
            catch (WebException Ex)
            {
                LoggingHelper.LogException("Unable to connect to SQL Database.  Please contact your System Administrator for more information.", Ex);
            }
            catch (Exception Ex)
            {
                LoggingHelper.LogException(Ex);
            }
        }
        #endregion

        private static ReportingService2010 instance { get; set; }

        public static ReportingService2010 ReportWebServiceInstance(System.Web.HttpContextBase httpContext)
        {

            
                if (instance == null)
                {

                    instance = (ReportingService2010)httpContext.Session[REPORTSERVERNAME];

                }
                return instance;
            

        }

        #region function
        public static bool IsSessionExpired(System.Web.HttpContextBase httpContext)
        {
            var rs = ReportWebServiceInstance(httpContext) ;

            if (rs == null)
            {
                return true;
            }
            return false;
        }

     

    public static ItemParameter[] GetItemParameters(System.Web.HttpContextBase httpContext, string reportPath, string historyID, bool forRendering, ReportService.ParameterValue[] values, ReportService.DataSourceCredentials[] credentials)
        {
            var rs = ReportWebServiceInstance(httpContext);//as ReportingService2010;

            ReportService.ItemParameter[] parameters = null;
            try
            {
                parameters = rs.GetItemParameters(reportPath, historyID, forRendering, values, credentials);
                
            }
            catch (SoapException ex)
            {
                LoggingHelper.LogException("Unable to connect to Web Service.", ex);
            }
            return parameters;
        }

        public static Byte[] DownloadExcel(System.Web.HttpContextBase httpContext, ReportExecutionBE reportexecution)
        {
            var rs = ReportWebServiceInstance(httpContext);
            var rsExec = new ReportExecutionService();

            rsExec.Timeout = System.Threading.Timeout.Infinite;
            rsExec.Credentials = rs.Credentials;
            byte[] result = null;
            string format = "EXCEL";
            string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
            string extension;
            string encoding;
            string mimeType;
            ReportExecution.Warning[] warnings = null;
            string[] streamIDs = null;

            ExecutionInfo execInfo = new ExecutionInfo();

            //var output = new HttpResponseMessage();
            try
            {
                
                ReportExecution.ExecutionInfo ei = rsExec.LoadReport(reportexecution.ReportPath, null);
                List<ReportExecution.ParameterValue> parameters = new List<ReportExecution.ParameterValue>();
                
                for (int i = 0; i < reportexecution.Parameters.Count; ++i)
                {
                    for (int j = 0; j < reportexecution.Parameters[i].Value.Length; ++j)
                    {
                        ReportExecution.ParameterValue item = new ReportExecution.ParameterValue();
                        item.Name = reportexecution.Parameters[i].Name;
                        item.Label = reportexecution.Parameters[i].Label;
                        item.Value = reportexecution.Parameters[i].Value[j];
                        parameters.Add(item);
                    }
                }
                ReportExecution.ParameterValue[] parametersExec = parameters.ToArray();
                rsExec.SetExecutionParameters(parametersExec, "en-us");
                result = rsExec.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);

                execInfo = rsExec.GetExecutionInfo();
                
            }
            catch (SoapException ex)
            {
                //output = new HttpResponseMessage(HttpStatusCode.BadRequest);
                LoggingHelper.LogException("Unable to generate the excel file.", ex);
            }
            //return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportexecution.ReportName + "-" + execInfo.ExecutionDateTime + ".xlsx");
            return result;
        }
        #endregion
    }
}
