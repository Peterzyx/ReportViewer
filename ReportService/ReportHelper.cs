using System;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTX.ReportService.EntityModel;
using BTX.ReportService.SSRS;

namespace BTX.ReportService
{
    public class ReportHelper
    {
        ReportingService2010 rs = new ReportingService2010();
        private ReportServer ReportServerConn = new ReportServer();
        public List<CatalogItem> GetReports()
        {
            rs.Credentials = new System.Net.NetworkCredential(ReportServerConn.UserName, ReportServerConn.Password, ReportServerConn.Domain);
            CatalogItem[] ciList = null;
            try
            {
                ciList = rs.ListChildren(ReportServerConn.FolderPath,false);

            }
            catch (SoapException ex)
            {

            }

            return ciList.ToList();
        }
    }
}
