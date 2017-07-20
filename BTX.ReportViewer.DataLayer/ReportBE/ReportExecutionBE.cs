

using System.Collections.Generic;

namespace BTX.ReportViewer.DataLayer
{
    public class ReportExecutionBE
    {
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public List<ReportParameterBE> Parameters { get; set; }
    }
}
