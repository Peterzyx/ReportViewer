using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class WeekBE
    {

        public string Segment { get; set; }
        public decimal Week1 { get; set; }

        public decimal Week2 { get; set; }
        public decimal Week3 { get; set; }
        public decimal Week4 { get; set; }

    }
}