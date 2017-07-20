using BTX.ReportViewer.ReportService;

namespace BTX.ReportViewer.EntityModel
{
    public partial class ReportBE
    {
        public string ReportName { get; set; }

        public string ReportGroup { get; set; }

        public string ReportDescription { get; set; }

        public string ReportImage { get; set; }

        public string ReportImageURL { get; set; }

        public string ReportPath { get; set; }
        public string ReportType { get; set; }

        public ItemParameter[] ReportParameters { get; set; }
    }
}