﻿using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class WeekColorCountryBE
    {

        public string Country { get; set; }
        public decimal Week1 { get; set; }

        public decimal Week2 { get; set; }
        public decimal Week3 { get; set; }
        public decimal Week4 { get; set; }

    }
}