using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BTX.ReportService.EntityModel
{
    public partial class ReportServer
    {
        #region Constructor
        public ReportServer()
        {

        }
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
        #endregion 
    }
}
