using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class WeekSalesSummaryBE
    {
        public string Color { get; set; }
        public string Country { get; set; }
        public string Product { get; set; }
        public string Variatel { get; set; }

        public string PriceBand { get; set; }

        public string  MyCategory { get; set; }
        public decimal Week1 { get; set; }

        public decimal Week2 { get; set; }
        public decimal Week3 { get; set; }
        public decimal Week4 { get; set; }

    }
}
